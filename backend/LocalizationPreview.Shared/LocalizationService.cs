using Microsoft.Extensions.Logging;

namespace LocalizationPreview.Shared; 

public class LocalizationService : ILocalizationService 
{
    private readonly IRedisRepository _redisRepository;
    private readonly ILogger<LocalizationService> _logger;
    private const string KeyPrefix = "localization";
    
    public string Language { get; set; }

    public LocalizationService(IRedisRepository redisRepository, ILogger<LocalizationService> logger) 
    {
        _redisRepository = redisRepository ?? throw new ArgumentException(nameof(redisRepository));
        _logger = logger ?? throw new ArgumentException(nameof(logger));
    }

    public async Task<bool> SetAsync(Localization localization, TimeSpan? expiry = null) 
    {
        if (!Languages.Support.Any(x => x.Equals(localization.LanguageCode, StringComparison.OrdinalIgnoreCase))) 
        {
            _logger.LogWarning("Wrong language {language}", localization.LanguageCode);
            throw new ArgumentException("Not supported language. List of support languages: " + Languages.SupportLanguages);
        }

        var key = BuildKey(localization.LanguageCode.ToLowerInvariant(), localization.EntityName, Convert.ToString(localization.EntityId));
        return await _redisRepository.SetAsync(key, localization, expiry);
    }

    public async Task<bool> SetBatchAsync(List<Localization> localizations) 
    {
        var keyValuePairs = new Dictionary<string, Localization>();
        if (!Languages.Support.Any(x => localizations.All(l => l.LanguageCode.Equals(x, StringComparison.OrdinalIgnoreCase)))) 
        {
            throw new ArgumentException("Not supported language. List of support languages: " + Languages.SupportLanguages());
        }

        foreach (var item in localizations) 
        {
            var key = BuildKey(item.LanguageCode, item.EntityName, Convert.ToString(item.EntityId));
            keyValuePairs.Add(key, item);
        }

        return await _redisRepository.SetBatchAsync(keyValuePairs);
    }

    public async Task<Localization> FindLocalizationAsync(int entityId, string entityName, string? language = null) 
    {
        if (string.IsNullOrEmpty(language) && string.IsNullOrEmpty(Language))
            throw new ArgumentException("Language is null or empty");

        if (string.IsNullOrEmpty(entityName))
            throw new ArgumentException("Entity name is null or empty");

        if (entityId <= 0)
            throw new ArgumentException("Entity id must be more than 0");

        string key = BuildKey(language ?? Language, entityName, Convert.ToString(entityId));
        Localization result = null;
        try 
        {
            result = await _redisRepository.GetAsync<Localization>(key);
        }
        catch (Exception e) 
        {
            _logger.LogError("Localization service error: {0}", e.Message);
        }

        return result;
    }

    public async Task<TEntity> GetTranslatedEntityAsync<TEntity>(TEntity entity, string? language = null)
        where TEntity : IBaseEntity 
    {
        if (entity == null)
            throw new ArgumentException(typeof(TEntity).Name + " is null");

        if (entity.Id <= 0)
            throw new ArgumentException("Entity id must be more than 0");

        TEntity result = entity;
        if (string.IsNullOrEmpty(language) && string.IsNullOrEmpty(Language))
            return result;

        try 
        {
            string key = BuildKey(language ?? Language, typeof(TEntity).Name, Convert.ToString(entity.Id));
            var cacheResult = await _redisRepository.GetAsync<Localization>(key);
            if (cacheResult == null)
                return result;

            foreach (var translationKey in cacheResult.TranslationFields.Keys) 
            {
                foreach (var propertyInfo in result.GetType().GetProperties()) 
                {
                    if (!translationKey.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                        continue;
                    var transValue = cacheResult.TranslationFields[translationKey];
                    propertyInfo.SetValue(result, transValue);
                }
            }
        }
        catch (Exception e) 
        {
            _logger.LogError("Localization service error: {0}", e.Message);
        }

        return result;
    }

    public async Task<List<Localization>> FindLocalizationsAsync(int[] entityIds, string entityName, string? language = null) 
    {
        if (string.IsNullOrEmpty(language) && string.IsNullOrEmpty(Language))
            throw new ArgumentException("Language is null or empty");

        if (string.IsNullOrEmpty(entityName))
            throw new ArgumentException("Entity name is null or empty");

        if (entityIds.Length <= 0)
            throw new ArgumentException("Entity ids quantity must be more than 0");

        var keys = entityIds
            .Select(x => BuildKey(language ?? Language, entityName, Convert.ToString(x)))
            .ToArray();
        var result = new List<Localization>();

        try 
        {
            result = await _redisRepository.GetListAsync<Localization>(keys);
        }
        catch (Exception e) 
        {
            _logger.LogError("Localization service error: {0}", e.Message);
        }

        return result;
    }

    public async Task<List<TEntity>> GetTranslatedEntitiesAsync<TEntity>(List<TEntity> entities, string? language = null)
        where TEntity : IBaseEntity 
    {
        var result = new List<TEntity>(entities);

        if (string.IsNullOrEmpty(language) && string.IsNullOrEmpty(Language))
            return result;

        try 
        {
            var keys = entities
                .Select(entity => BuildKey(language ?? Language, entity.GetType().Name, Convert.ToString(entity.Id)))
                .ToArray();

            var cacheResult = await _redisRepository.GetListAsync<Localization>(keys);
            if (!cacheResult.Any())
                return result;

            var translations = cacheResult.Select(x => x.TranslationFields);
            foreach (var translation in translations) 
            {
                foreach (var key in translation.Keys) 
                {
                    foreach (var propertyInfo in result.GetType().GetProperties()) 
                    {
                        if (!key.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                            continue;
                        var transValue = translation[key];
                        propertyInfo.SetValue(result, transValue);
                    }
                }
            }
        }
        catch (Exception e) 
        {
            _logger.LogError("Localization service error: {0}", e.Message);
        }

        return result;
    }

    private string BuildKey(string language, string entityName, string entityId) => 
        string.Join("_", new[] { KeyPrefix, language, entityName, entityId }).ToLowerInvariant();
}