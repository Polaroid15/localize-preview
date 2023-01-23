using System.Data;

namespace LocalizationPreview.Core.Interfaces; 

public interface ISqlRepository 
{
    Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null);

    Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null)
        where T : class;

    Task<int> ExecuteAsync(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null);
}