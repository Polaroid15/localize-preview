using LocalizationPreview.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LocalizationPreview.Infrastructure.Logging; 

public class AppLoggerAdapter<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;
    public AppLoggerAdapter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<T>();
    }

    public void LogInfo(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogError(string message, params object[] args) {
        _logger.LogError(message, args);
    }

    public void LogCritical(string message, params object[] args) {
        _logger.LogCritical(message, args);
    }
}