namespace LocalizationPreview.Shared;

public class Languages : List<string> {
    public static readonly Languages Support = new() {
        "pt", "es", "uk", "it", "be", "ru"
    };
    
    private static readonly Dictionary<string, string> LanguagesDictionary = new () {
        {"pt", "Portugal"},
        {"es", "Spanish"},
        {"uk", "Ukrainian"},
        {"it", "Italian"},
        {"be", "Belarusian"},
        {"ru", "Russian"}
    };
    
    public static string SupportLanguages() {
        var items = LanguagesDictionary.Select(kvp => kvp.ToString());
        return string.Join(',', items);
    }
    
    public override string ToString() => string.Join(',', Support);
}