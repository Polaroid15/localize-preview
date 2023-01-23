using System.Data;
using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Entities;

namespace LocalizationPreview.Core.Interfaces; 

public interface IOutboxRepository 
{
    Task<long> CreateAsync(IntegrationItemOutboxDto dto, IDbConnection connection = null, IDbTransaction transaction = null);
    Task<List<IntegrationItemOutbox>> GetAllAsync(string nameItem, bool isDeleted = false, IDbConnection connection = null, IDbTransaction transaction = null);
    Task SetDeletedAsync(long id, IDbConnection connection = null, IDbTransaction transaction = null);
}