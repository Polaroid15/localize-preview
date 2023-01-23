using System.Data;
using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Entities;

namespace LocalizationPreview.Core.Interfaces; 

public interface ITranslationsRepository {
    Task<long> CreateAsync(TranslationRepositoryDto dto, IDbConnection connection = null, IDbTransaction transaction = null);
    Task<long> UpdateAsync(long id, TranslationRepositoryDto dto, IDbConnection connection = null, IDbTransaction transaction = null);
    Task<Translation> FindByIdAsync(long id, IDbConnection connection = null, IDbTransaction transaction = null);
    Task<Translation> FindAsync(long entityId, string entityName, string languageCode, IDbConnection connection = null, IDbTransaction transaction = null);
    Task<List<Translation>> FindListAsync(string entityName, string languageCode, IDbConnection connection = null, IDbTransaction transaction = null);
}