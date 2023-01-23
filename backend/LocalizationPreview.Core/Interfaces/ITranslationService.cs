using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Entities;

namespace LocalizationPreview.Core.Interfaces; 

public interface ITranslationService {
    Task<long> CreateAsync(TranslationServiceDto dto);
    Task<long> UpdateAsync(long id, TranslationServiceDto dto);
    Task<Translation> FindByIdAsync(long id);
    Task<Translation> FindAsync(long entityId, string entityName, string languageCode);
    Task<List<Translation>> FindListAsync(string entityName, string language);
}