using System.Runtime.Versioning;
using System.Text.Json;
using NokoHwd.Common;
using NokoHwd.Globals;
using NokoHwd.Schemas;

namespace NokoHwd.Commands;

public class NokoApplication
{
    private static JsonSerializerOptions? JsonSerializerOptions { get; set; }
    
    [SupportedOSPlatform("windows")]
    public static void Run(NokoApplicationOptions options)
    {
        // print waiting stdin
        // scan write stdout
        
        JsonSerializerOptions ??= new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            IndentSize = 0,
        };
        
        Console.WriteLine($"Config: {JsonSerializer.Serialize(options, JsonSerializerOptions)}");

        if (options.Scan)
        {
            var sScan = new SessionScanner(new SessionScannerOptions()
            {
                Serial = options.Serial!,
            });
        }

        if (!options.Print) return;
        Localization.JsonLoad("Assets/Languages", "id-ID");

        var sPrint = new SessionPrinter(new SessionPrinterOptions()
        {
            Name = options.Name!,
        });;

        var transaction = SessionPrinter.GetTransactionTest();
        
        Console.WriteLine($"Test: {JsonSerializer.Serialize(transaction, JsonSerializerOptions)}");
        
        if (!options.Test)
        {
            options.Json ??= NokoCommonMod.ConsoleReadLineByPrefix("Json:");
            JsonSerializer.Deserialize<Transaction>(options.Json, JsonSerializerOptions);
        }

        sPrint.Print(transaction);
    }
}
