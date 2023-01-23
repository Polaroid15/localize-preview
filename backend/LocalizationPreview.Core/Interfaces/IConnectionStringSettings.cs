namespace LocalizationPreview.Core.Interfaces; 

public interface IConnectionStringSettings {
    public string MainDb { get; }
    public string Redis { get; }
}