using System.ComponentModel.DataAnnotations;
using LocalizationPreview.Shared;

namespace LocalizationPreview.Core.Attributes; 

public class SupportLanguageAttribute : ValidationAttribute 
{
    public SupportLanguageAttribute() { ErrorMessage = "Not supported language. List of support languages: " + Languages.SupportLanguages(); }
    
    public override bool IsValid(object? value) 
    {
        if (value == null)
            return false;

        var isValid = Languages.Support.Any(x => x.Contains(value.ToString()!.ToLowerInvariant()));
        return isValid;
    }
}