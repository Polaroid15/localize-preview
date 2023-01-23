using System.Data;
using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Entities;
using LocalizationPreview.Core.Interfaces;

namespace LocalizationPreview.Infrastructure.Translations; 

public class TranslationsRepository : ITranslationsRepository 
{
    private readonly ISqlRepositoryAsync _repository;
    public TranslationsRepository(ISqlRepositoryAsync repository) 
    {
        _repository = repository;
    }
    
    public async Task<long> CreateAsync(TranslationRepositoryDto dto, IDbConnection connection = null, IDbTransaction transaction = null) 
    {
        var sql = @"INSERT INTO translations(entity_id, entity_name, language_code, translation_fields) 
                    VALUES (@EntityId, @EntityName, @LanguageCode, @TranslationFields) RETURNING id;";
        
        var args = new 
        {
            EntityId = dto.EntityId,
            EntityName = dto.EntityName,
            LanguageCode = dto.LanguageCode,
            TranslationFields = dto.TranslationFields
        };
        
        var id = await _repository.QuerySingleOrDefaultAsync<long>(sql, args, connection, transaction);
        return id;
    }

    public async Task<long> UpdateAsync(long id, TranslationRepositoryDto dto, IDbConnection connection = null, IDbTransaction transaction = null) 
    {
        var sql = @"UPDATE translations 
                    SET entity_id = @EntityId, 
                        entity_name = @EntityName, 
                        language_code = @LanguageCode, 
                        translation_fields = @TranslationFields,
                        updated_date = @UpdatedDate
                    WHERE id = @Id;";
        
        var args = new {
            Id = id,
            EntityId = dto.EntityId,
            EntityName = dto.EntityName,
            LanguageCode = dto.LanguageCode,
            TranslationFields = dto.TranslationFields,
            UpdatedDate = DateTimeOffset.UtcNow
        };
        
        await _repository.ExecuteAsync(sql, args, connection, transaction);
        return id;
    }

    public async Task<Translation> FindByIdAsync(long id, IDbConnection connection = null, IDbTransaction transaction = null) 
    {
        var sql = @"SELECT id, entity_id, entity_name, language_code, translation_fields
                    FROM translations 
                    WHERE id = @Id;";
        
        var result = await _repository.QuerySingleOrDefaultAsync<Translation>(sql, new { Id = id }, connection, transaction);
        return result;
    }

    public async Task<Translation> FindAsync(long entityId, string entityName, string languageCode, IDbConnection connection = null,
        IDbTransaction transaction = null) 
    {
        var sql = @"SELECT id, entity_id, entity_name, language_code, translation_fields
                    FROM translations 
                    WHERE entity_id = @EntityId 
                      AND entity_name = @EntityName 
                      AND language_code = @LanguageCode;";
        
        var args = new {
            EntityId = entityId,
            EntityName = entityName,
            LanguageCode = languageCode
        };
        var result = await _repository.QuerySingleOrDefaultAsync<Translation>(sql, args, connection, transaction);
        return result;
    }

    public async Task<List<Translation>> GetListAsync(string entityName, string languageCode, IDbConnection connection = null,
        IDbTransaction transaction = null) 
    {
        var sql = @"SELECT *
                    FROM translations 
                    WHERE entity_name = @EntityName 
                      AND language_code = @LanguageCode;";
        var args = new {
            EntityName = entityName,
            LanguageCode = languageCode
        };
        var result = await _repository.QueryAsync<Translation>(sql, args, connection, transaction);
        return result.ToList();
    }
}