namespace LocalizationPreview.Shared; 

public interface ILocalizationService 
{
    /// <summary>
    /// Default language for search. Can be null.
    /// If this property is null, you must use query parameter <param name="language">string in methods.</param>
    /// It has more priority in all find methods than query parameter if property is not null.
    /// </summary>
    public string Language { get; set; }
    
    /// <summary>
    /// Set localization for any entity.
    /// </summary>
    /// <param name="localization"><see cref="Localization"/>.</param>
    /// <param name="expiry">The expiry to localization value.</param>
    /// <returns>Inserting result.</returns>
    Task<bool> SetAsync(Localization localization, TimeSpan? expiry = null);
    
    /// <summary>
    /// Set batch of entities.
    /// </summary>
    /// <param name="localizations">List of <see cref="Localization"/>.</param>
    /// <returns>Inserting result.</returns>
    Task<bool> SetBatchAsync(List<Localization> localizations);
    
    /// <summary>
    /// Get localization for entity by id and language.
    /// </summary>
    /// <param name="entityId">entity id.</param>
    /// <param name="entityName">entity name.</param>
    /// <param name="language">code of language like in <see cref="Localization"/>.</param>
    /// <returns><see cref="Localization"/> or null.</returns>
    Task<Localization> FindLocalizationAsync(int entityId, string entityName, string? language = null);

    /// <summary>
    /// Get entity with new language translation.
    /// </summary>
    /// <param name="entity">Entity for localization.</param>
    /// <param name="language">code of language like in <see cref="Localization"/>.</param>
    /// <typeparam name="TEntity">Type of entity for localization.</typeparam>
    /// <returns>Translated entity or entity with default localization.</returns>
    Task<TEntity> GetTranslatedEntityAsync<TEntity>(TEntity entity, string? language = null)
        where TEntity : IBaseEntity;

    /// <summary>
    /// Find list localisations for entities by id and language.
    /// </summary>
    /// <param name="entityIds">array of entity id.</param>
    /// <param name="entityName">entity name.</param>
    /// <param name="language">code of language like in <see cref="Localization"/>.</param>
    /// <returns>List of <see cref="Localization"/> or empty list.</returns>
    Task<List<Localization>> FindLocalizationsAsync(int[] entityIds, string entityName, string? language = null);

    /// <summary>
    /// Get entities and return list of entities with new language translations.
    /// </summary>
    /// <param name="entities">List of TEntity.</param>
    /// <param name="language">code of language like in <see cref="Localization"/>.</param>
    /// <typeparam name="TEntity">Type of entity for localization.</typeparam>
    /// <returns>Translated list of TEntity or list with default localizations.</returns>
    Task<List<TEntity>> GetTranslatedEntitiesAsync<TEntity>(List<TEntity> entities, string? language = null)
        where TEntity : IBaseEntity;
}