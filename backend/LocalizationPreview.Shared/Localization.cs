namespace LocalizationPreview.Shared; 

public class Localization 
{
    public long EntityId { get; set; }
    public string EntityName { get; set; }
    
    /// <summary>
    /// Support languages <see cref="Languages.Support"/>.
    /// </summary>
    public string LanguageCode { get; set; }
    
    /// <summary>
    /// For example {{"title","hello world"}}.
    /// </summary>
    public Dictionary<string, string> TranslationFields { get; set; }   
}