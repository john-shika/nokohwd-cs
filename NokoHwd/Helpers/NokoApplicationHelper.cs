using NokoHwd.Common;
using NokoHwd.Schemas;

namespace NokoHwd.Helpers;

public static class NokoApplicationHelper
{
    public static bool Validate(NokoApplicationOptions options)
    {
        var serialWasEmpty = string.IsNullOrEmpty(options.Serial);
        var nameWasEmpty = string.IsNullOrEmpty(options.Name);
        var jsonWasEmpty = string.IsNullOrEmpty(options.Json);
        
        switch (options)
        {
            case { Scan: true } when serialWasEmpty:
                NokoCommonMod.Error("The '--scan' option requires a valid '--serial' to be specified.");
                return false;
            
            case { Scan: false } when !serialWasEmpty:
                NokoCommonMod.Error("The '--serial' option requires the '--scan' option to be specified.");
                return false;
            
            case { Print: true } when nameWasEmpty:
                NokoCommonMod.Error("The '--print' option requires a valid '--name' to be specified.");
                return false;
            
            case { Print: false } when !nameWasEmpty:
                NokoCommonMod.Error("The '--name' option requires the '--print' option to be specified.");
                return false;
            
            case { Test: true, Print: false }:
                NokoCommonMod.Error("The '--test' option is only available with the '--print' option.");
                return false;
            
            case { Print: false } when !jsonWasEmpty:
                NokoCommonMod.Error("The '--json' option is only available with the '--print' option.");
                return false;
            
            case { Test: true } when !jsonWasEmpty:
                NokoCommonMod.Error("The '--test' option can`t handle data input with JSON encoding.");
                return false;
            
            case { Print: false, Scan: false }:
                NokoCommonMod.Error("At least one other operation '--print' or '--scan' option is required.");
                return false;
            
            default:
                return true;
        }
    } 
}