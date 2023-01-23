using LocalizationPreview.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LocalizationPreview.Infrastructure; 

public class ConnectionStringSettings : IConnectionStringSettings {
    public string MainDb { get; }
    public string Redis { get; }

    public ConnectionStringSettings(IConfiguration configuration) {
        MainDb = configuration.GetConnectionString("MainDb");
        Redis = configuration.GetConnectionString("Redis");
    }
}