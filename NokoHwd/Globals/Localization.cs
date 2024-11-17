using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace NokoHwd.Globals;

public static class Localization
{
    private static Dictionary<string, string>? _localizationDictionary = new();
    public static CultureInfo CultureInfo { get; private set; } = CultureInfo.InvariantCulture;

    public static void JsonLoad(string directory, string cultureName)
    {
        CultureInfo = CultureInfo.GetCultureInfo(cultureName);
        
        var twoLetterIsoLanguageName = CultureInfo.TwoLetterISOLanguageName;
        var twoLetterIsoLanguageFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory, $"{twoLetterIsoLanguageName}.json");
        
        if (File.Exists(twoLetterIsoLanguageFilePath))
        {
            var json = File.ReadAllText(twoLetterIsoLanguageFilePath);
            _localizationDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        else
        {
            throw new FileNotFoundException($"Localization file not found: {twoLetterIsoLanguageFilePath}");
        }
    }

    public static string GetString(string key)
    {
        if (_localizationDictionary is null) return $"{{{key}}}";
        var result = _localizationDictionary.FirstOrDefault(kv => kv.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        return result.Value ?? $"{{{key}}}";
    }
}
