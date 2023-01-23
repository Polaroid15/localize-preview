using System.Data;
using LocalizationPreview.Core.Interfaces;
using Npgsql;

namespace LocalizationPreview.Infrastructure.DataAccess; 

public class ConnectionFactoryPostgreSQL : IConnectionFactory 
{
    private readonly string _connectionString;

    public ConnectionFactoryPostgreSQL(IConnectionStringSettings settings)
    {
        _connectionString = settings.MainDb;
    }

    public IDbConnection Create() => Create(_connectionString);

    public IDbConnection Create(string connectionString)
    {
        var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();
        return dbConnection;
    }

    public async Task<IDbConnection> CreateAsync() => await CreateAsync(_connectionString);

    public async Task<IDbConnection> CreateAsync(string connectionString)
    {
        var dbConnection = new NpgsqlConnection(connectionString);
        await dbConnection.OpenAsync().ConfigureAwait(false);
        return dbConnection;
    }
}