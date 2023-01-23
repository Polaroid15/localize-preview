using Dapper;
using LocalizationPreview.Core.Interfaces;
using LocalizationPreview.Infrastructure.DataAccess;
using LocalizationPreview.Infrastructure.Translations;
using LocalizationPreview.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace LocalizationPreview.Infrastructure.Extensions; 

public static class ConfigureServices {
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        var connectionString = configuration["ConnectionStrings:Redis"];
        var multiplexer = ConnectionMultiplexer.Connect(connectionString);
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        services.AddTransient<IRedisRepository, RedisRepository>();
        services.AddScoped<ILocalizationService, LocalizationService>();

        DefaultTypeMap.MatchNamesWithUnderscores = true;
        SqlMapper.AddTypeHandler(JObjectHandler.Instance);
        services.AddTransient<IConnectionStringSettings, ConnectionStringSettings>();
        services.AddTransient<IConnectionFactory, ConnectionFactoryPostgreSQL>();
        services.AddTransient<ISqlRepository, RepositoryDapper>();
        services.AddTransient<IOutboxRepository, OutboxRepository>();
        services.AddTransient<ITranslationsRepository, TranslationsRepository>();
        services.AddTransient<ITranslationService, TranslationService>();

        services.AddSingleton<UpdateCacheService>();
        services.AddHostedService(provider => provider.GetService<UpdateCacheService>());        
    }
}