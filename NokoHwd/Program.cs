using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Runtime.Versioning;
using System.Text.Json;
using NokoHwd.Commands;
using NokoHwd.Globals;
using NokoHwd.Helpers;
using NokoHwd.Schemas;

namespace NokoHwd;

public static class Program
{
    [SupportedOSPlatform("windows")]
    public static int Main(string[] args)
    {
        var cmd = new RootCommand
        {
            new Option<bool>(
                aliases: ["/scan", "--scan"],
                description: "Scan. Requires --serial for connection. (Barcode, QR Code)"
            ),
            new Option<bool>(
                aliases: ["/print", "--print"],
                description: "Print. Requires --name for connection. (EscPos)"
            ),
            new Option<bool>(
                aliases: ["/test", "--test"],
                description: "Test printer or scanner."
            ),
            new Option<string>(
                aliases: ["/json", "--json"],
                description: "Json data output."
            ),
            new Option<string>(
                aliases: ["/serial", "--serial", "/S", "-S"],
                description: "Serial Port for scanner."
            ),
            new Option<string>(
                aliases: ["/name", "--name", "/N", "-N"],
                description: "Name of the Printer. Must be visible in Network Discovery."
            )
        };
        
        Console.WriteLine("Name: NokoHwd (Win64)");
        Console.WriteLine("GitHub: https://github.com/john-shika/nokohwd-cs");
        Console.WriteLine("Author: <ahmadasysyafiq@proton.me>");
        Console.WriteLine("License: Apache-2.0");
        Console.WriteLine("Version: 1.0.0");
        Console.WriteLine();
        
        cmd.Description = "NokoHwd Windows Application. (Barcode, QR Code Scanner, Receipt Printer)";

        var nokoHwdOptions = new NokoApplicationOptions();
        
        cmd.Handler = CommandHandler.Create<bool, bool, bool, string, string, string>((scan, print, test, json, serial, name) =>
        {
            if (scan) nokoHwdOptions.Scan = true;
            if (print) nokoHwdOptions.Print = true;
            if (test) nokoHwdOptions.Test = true;
            if (!string.IsNullOrEmpty(json)) nokoHwdOptions.Json = json;
            if (!string.IsNullOrEmpty(serial)) nokoHwdOptions.Serial = serial;
            if (!string.IsNullOrEmpty(name)) nokoHwdOptions.Name = name;
        });

        var retVal = cmd.InvokeAsync(args).Result;
        var help = args?.FirstOrDefault((x) =>
        {
            return x switch
            {
                "-?" or "-h" or "--help" => true,
                _ => false
            };
        });

        if (help is not null) return retVal;

        if (!NokoApplicationHelper.Validate(nokoHwdOptions)) Environment.Exit(1);
        NokoApplication.Run(nokoHwdOptions);
        return retVal;
    }
}