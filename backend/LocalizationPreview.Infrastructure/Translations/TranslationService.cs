using System.Data;
using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Entities;
using LocalizationPreview.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LocalizationPreview.Infrastructure.Translations;

public class TranslationService : ITranslationService {
    private readonly ITranslationsRepository _translationRepository;
    private readonly IOutboxRepository _outboxRepository;
    private readonly UpdateCacheService _updateCacheService;
    private readonly IConnectionFactory _connectionFactory;
    private readonly IAppLogger<TranslationService> _logger;

    public TranslationService(
        ITranslationsRepository translationRepository,
        IOutboxRepository outboxRepository,
        UpdateCacheService updateCacheService,
        IConnectionFactory connectionFactory,
        IAppLogger<TranslationService> logger) {
        _translationRepository = translationRepository;
        _outboxRepository = outboxRepository;
        _updateCacheService = updateCacheService;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<long> CreateAsync(TranslationServiceDto dto) {
        IDbConnection connection = null;
        IDbTransaction transaction = null;

        try {
            connection = await _connectionFactory.CreateAsync();
            transaction = connection.BeginTransaction();

            var repoDto = new TranslationRepositoryDto {
                EntityId = dto.EntityId,
                EntityName = dto.EntityName,
                LanguageCode = dto.LanguageCode,
                TranslationFields = JObject.FromObject(dto.TranslationFields)
            };
            var result = await _translationRepository.CreateAsync(repoDto);

            var outboxItem = new IntegrationItemOutboxDto {
                NameItem = nameof(TranslationServiceDto),
                Data = JsonConvert.SerializeObject(dto)
            };

            _ = await _outboxRepository.CreateAsync(outboxItem);

            transaction.Commit();
            _updateCacheService.UpdateOutstandingCacheItems();
            return result;
        }
        catch (Exception e) {
            try {
                transaction?.Rollback();
                _logger.LogError("Transaction was rollback. Error: {transaction error}", e.Message);
            }
            catch (Exception ee) {
                _logger.LogError("Transaction rollback failed. Error: {transaction error}", ee.Message);
                throw;
            }

            throw;
        }
        finally {
            transaction?.Dispose();
            connection?.Dispose();
        }
    }

    public async Task<long> UpdateAsync(long id, TranslationServiceDto dto) {
        IDbConnection connection = null;
        IDbTransaction transaction = null;

        try {
            connection = await _connectionFactory.CreateAsync();
            transaction = connection.BeginTransaction();

            var repoDto = new TranslationRepositoryDto {
                EntityId = dto.EntityId,
                EntityName = dto.EntityName,
                LanguageCode = dto.LanguageCode,
                TranslationFields = JObject.FromObject(dto.TranslationFields)
            };
            var result = await _translationRepository.UpdateAsync(id, repoDto);

            var outboxItem = new IntegrationItemOutboxDto {
                NameItem = nameof(TranslationServiceDto),
                Data = JsonConvert.SerializeObject(dto)
            };

            _ = await _outboxRepository.CreateAsync(outboxItem);

            transaction.Commit();
            _updateCacheService.UpdateOutstandingCacheItems();
            return result;
        }
        catch (Exception e) {
            try {
                transaction?.Rollback();
                _logger.LogError("Transaction was rollback. Error: {transaction error}", e.Message);
            }
            catch (Exception ee) {
                _logger.LogError("Transaction rollback failed. Error: {transaction error}", ee.Message);
                throw;
            }

            throw;
        }
        finally {
            transaction?.Dispose();
            connection?.Dispose();
        }
    }

    public async Task<Translation> FindByIdAsync(long id) =>
        await _translationRepository.FindByIdAsync(id);

    public async Task<Translation> FindAsync(long entityId, string entityName, string language) =>
        await _translationRepository.FindAsync(entityId, entityName, language);

    public async Task<List<Translation>> GetListAsync(string entityName, string language) =>
        await _translationRepository.GetListAsync(entityName, language);
}