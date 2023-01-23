using System.Data;

namespace LocalizationPreview.Core.Interfaces; 

public interface IConnectionFactory {
    /// <summary>
    /// Create db connection
    /// </summary>
    /// <returns>Db connection object</returns>
    IDbConnection Create();

    /// <summary>
    /// Connect db connection with connection string
    /// </summary>
    /// <returns>Db connection object</returns>
    IDbConnection Create(string connectionString);

    /// <summary>
    /// Create db connection asynchronously
    /// </summary>
    /// <returns>Db connection object</returns>
    Task<IDbConnection> CreateAsync();

    /// <summary>
    /// Connect db connection with connection string asynchronously
    /// </summary>
    /// <returns>Db connection object</returns>
    Task<IDbConnection> CreateAsync(string connectionString);
}