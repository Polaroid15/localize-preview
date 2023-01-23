using System.Data;
using Dapper;
using LocalizationPreview.Core.Interfaces;

namespace LocalizationPreview.Infrastructure.DataAccess;

public class RepositoryDapper : ISqlRepositoryAsync
{
    private const int CommandTimeout = 300;
    private readonly IConnectionFactory _factory;

    public RepositoryDapper(IConnectionFactory factory)
    {
        _factory = factory;
    }

    /// <inheritdoc/>
    public async Task<int> ExecuteAsync(string sql, object param = null,
        IDbConnection connection = null, IDbTransaction transaction = null) 
    {
        if (connection == null)
        {
            using (connection = await _factory.CreateAsync())
            {
                return await connection.ExecuteAsync(sql, param, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        return await connection.ExecuteAsync(sql, param, transaction: transaction, commandTimeout: CommandTimeout);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null,
        IDbConnection connection = null, IDbTransaction transaction = null)
        where T : class 
    {
        if (connection == null)
        {
            using (connection = await _factory.CreateAsync())
            {
                return await connection.QueryAsync<T>(sql, param, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        return await connection.QueryAsync<T>(sql, param, transaction: transaction, commandTimeout: CommandTimeout);
    }

    public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null,
        IDbConnection connection = null, IDbTransaction transaction = null)
    {
        if (connection == null)
        {
            using (connection = await _factory.CreateAsync())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction: transaction, commandTimeout: CommandTimeout);
            }
        }

        return await connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction: transaction, commandTimeout: CommandTimeout);
    }
}