using System.Data;
using LocalizationPreview.Core.Dto;
using LocalizationPreview.Core.Entities;
using LocalizationPreview.Core.Interfaces;

namespace LocalizationPreview.Infrastructure;

public class OutboxRepository : IOutboxRepository {
    private readonly ISqlRepositoryAsync _repository;

    public OutboxRepository(ISqlRepositoryAsync repository) {
        _repository = repository;
    }
    
    public async Task<long> CreateAsync(
        IntegrationItemOutboxDto dto,
        IDbConnection connection = null,
        IDbTransaction transaction = null) 
    {
        var sql = @"INSERT INTO integration_items_outbox(name_item, data) 
                    VALUES (@NameItem, @Data) RETURNING id;";
        
        var args = new { NameItem = dto.NameItem, Data = dto.Data };
        
        var id = await _repository.QuerySingleOrDefaultAsync<long>(sql, args, connection, transaction);
        return id;
    }

    public async Task<List<IntegrationItemOutbox>> GetAllAsync(string nameItem, bool isDeleted = false,
        IDbConnection connection = null, IDbTransaction transaction = null) 
    {
        var sql = @"SELECT *
                    FROM integration_items_outbox 
                    WHERE name_item = @NameItem 
                      AND is_deleted = @IsDeleted;";
        
        var args = new { NameItem = nameItem, IsDeleted = isDeleted };
        var result = await _repository.QueryAsync<IntegrationItemOutbox>(sql, args, connection, transaction);
        return result.ToList();
    }

    public async Task SetDeletedAsync(long id, IDbConnection connection = null, IDbTransaction transaction = null) 
    {
        var sql = @"UPDATE integration_items_outbox 
                    SET is_deleted = true, 
                        deletion_date = @DeletionDate,
                        updated_date = @UpdatedDate
                    WHERE id = @Id;";

        var args = new 
        {
            Id = id,
            DeletionDate = DateTimeOffset.UtcNow,
            UpdatedDate = DateTimeOffset.UtcNow
        };

        await _repository.ExecuteAsync(sql, args, connection, transaction);
    }
}