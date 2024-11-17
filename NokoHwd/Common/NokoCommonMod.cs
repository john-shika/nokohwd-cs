using System.Text.RegularExpressions;

namespace NokoHwd.Common;

public static class NokoCommonMod
{
    public delegate void ForEachEntryHandler<in TKey, in TValue>(TKey key, TValue value);

    public static void ForEach<TValue>(IEnumerable<TValue> enumerable, ForEachEntryHandler<int, TValue> handler)
    {
        var i = 0;
        foreach (var value in enumerable)
        {
            handler.Invoke(i, value);
            i++;
        }
    }
    
    public static void ForEach<TKey, TValue>(IDictionary<TKey, TValue> dictionary, ForEachEntryHandler<TKey, TValue> handler)
    {
        foreach (var kv in dictionary)
        {
            handler.Invoke(kv.Key, kv.Value);
        }
    }
    
    public static void ForEachStringSplit(string value, string separator, ForEachEntryHandler<int, string> handler)
    {
        var i = 0;
        foreach (var word in value.Split(separator).Select((x) => x.Trim()))
        {
            if (word.Length == 0) continue;
            handler.Invoke(i, word);
            i++;
        }
    }

    public static void ForEachStringSplit(string value, Regex pattern, ForEachEntryHandler<int, string> handler)
    {
        var i = 0;
        foreach (var word in pattern.Split(value).Select((x) => x.Trim()))
        {
            if (word.Length == 0) continue;
            handler.Invoke(i, word);
            i++;
        }
    }

    public static string ConsoleReadLineByPrefix(string prefix)
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (input is null || !input.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;
            return input[prefix.Length..].Trim();
        }
    }

    /// <summary>
    /// Displays a fatal error message in red and formats the message if necessary.
    /// </summary>
    /// <param name="message">The main error message to display.</param>
    /// <param name="args">Optional parameters for additional formatted messages.</param>
    public static void FatalError(string message, params object?[] args)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        if (args is { Length: > 0 })
        {
            message = string.Format(message, args);
        }

        Console.WriteLine($"FATAL ERROR: {message}");
        Console.ResetColor();
        Environment.Exit(1);
    }
    
    /// <summary>
    /// Displays a error message in red and formats the message if necessary.
    /// </summary>
    /// <param name="message">The main error message to display.</param>
    /// <param name="args">Optional parameters for additional formatted messages.</param>
    public static void Error(string message, params object?[] args)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        if (args is { Length: > 0 })
        {
            message = string.Format(message, args);
        }

        Console.WriteLine($"ERROR: {message}");
        Console.ResetColor();
    }
}
