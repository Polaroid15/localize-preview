namespace LocalizationPreview.Core.Interfaces; 

public interface IAppLogger<T>
{
    void LogInfo(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(string message, params object[] args);
    void LogCritical(string message, params object[] args);
}