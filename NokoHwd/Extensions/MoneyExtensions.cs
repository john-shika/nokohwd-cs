using System.Globalization;

namespace NokoHwd.Extensions;

public static class MoneyExtensions
{
    public static string ToMoneyString(this decimal value)
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        return value.ToString("#,##0", cultureInfo);
    }
    
    public static string ToMoneyString(this decimal value, CultureInfo cultureInfo)
    {
        return value.ToString("#,##0", cultureInfo);
    }
    
    public static string ToMoneyString(this string value)
    {
        return decimal.Parse(value).ToMoneyString();
    }
}
