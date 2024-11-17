using System.Drawing;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using EscPosNet;
using EscPosNet.Enums;
using NokoHwd.Common;
using NokoHwd.Extensions;
using NokoHwd.Globals;

namespace NokoHwd.Commands;

public class Cart
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public int Qty { get; set; } = 0;
    public decimal Price { get; set; } = 0;
    public decimal Total { get; set; } = 0;
}

public class Transaction
{
    public required string Code { get; set; } = "";
    public required string Cashier { get; set; } = "";
    public decimal Total { get; set; } = 0;
    public int TaxRate { get; set; } = 0;
    public decimal Tax { get; set; } = 0;
    public decimal Pay { get; set; } = 0;
    public decimal Money { get; set; } = 0;
    public decimal Change { get; set; } = 0;
    public string QrCode { get; set; } = "https://github.com/john-shika";
    public IEnumerable<Cart> Carts { get; set; } = new List<Cart>();
}

public sealed class SessionPrinterOptions
{
    public required string Name { get; set; }
    public string CodePage { get; set; } = "IBM860";
}

public partial class SessionPrinter
{
    [GeneratedRegex(@"\r\n|\r|\n")]
    private static partial Regex NewLineRegex();
    
    private Printer Printer { get; init; }

    public SessionPrinter(SessionPrinterOptions options)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Printer = new Printer(options.Name, codepage: options.CodePage);
    }

    [SupportedOSPlatform("windows")]
    public void Print(Transaction transaction)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Images", "profile.png");
        
        using var image = Image.FromFile(path);
        using var bitmap = new Bitmap(image);
        
        var dateTimeNow = DateTime.Now;
        
        Printer.Smooth();
        Printer.Image(bitmap);
        Printer.Feed(1);

        #region greetings

        {
            var greetings = Localization.GetString("Greetings");
        
            NokoCommonMod.ForEachStringSplit(greetings, NewLineRegex(), (key, value) =>
            {
                Printer.AlignCenter();
                Printer.Append(value);
                Printer.Feed(1); 
            });
        
            Printer.Separator();
        }

        #endregion

        #region header

        {
            var cashierName = Localization.GetString("Cashier");
        
            Printer.AppendSpaceBetweenLines(cashierName, "", transaction.Cashier);
            Printer.Separator();
        
            var codeName = Localization.GetString("Code");
            var dateName = Localization.GetString("Date");
            var timeName = Localization.GetString("Time");
        
            Printer.AppendSpaceBetweenLines(codeName, dateName, timeName);
            Printer.Separator();
        
            var dateNow = dateTimeNow.ToString("yyyy-MM-dd");
            var timeNow = dateTimeNow.ToString("HH:mm:ss");
        
            Printer.AppendSpaceBetweenLines(transaction.Code, dateNow, timeNow);
            Printer.Separator();
        }

        #endregion

        #region carts

        {
            foreach (var cart in transaction.Carts)
            {
                Printer.Feed(1);
            
                Printer.AlignLeft();
                Printer.Append(cart.Name);
                Printer.Feed(1);
            
                var qty = $"{cart.Price.ToMoneyString(Localization.CultureInfo)} (x{cart.Qty})".PadLeft(14);
                var price = cart.Total.ToMoneyString(Localization.CultureInfo);
            
                Printer.AppendSpaceBetweenLines(cart.Type, qty, price);
                Printer.Feed(1);
            }
        
            Printer.Separator();
        }

        #endregion

        #region transactions

        {
            var total = Localization.GetString("Total");
            var totalValue = transaction.Total.ToMoneyString(Localization.CultureInfo);
        
            Printer.AppendSpaceBetweenLines(total, "", totalValue);
            Printer.Separator();
        
            var tax = $"{Localization.GetString("Tax")} {transaction.TaxRate}%";
            var taxValue = transaction.Tax.ToMoneyString(Localization.CultureInfo);
        
            Printer.AppendSpaceBetweenLines(tax, "", taxValue);
            Printer.Separator();
        
            var pay = Localization.GetString("Pay");
            var payValue = transaction.Pay.ToMoneyString(Localization.CultureInfo);
        
            Printer.AppendSpaceBetweenLines(pay, "", payValue);
            Printer.Separator();
        
            var money = Localization.GetString("Money");
            var moneyValue = transaction.Money.ToMoneyString(Localization.CultureInfo);
        
            Printer.AppendSpaceBetweenLines(money, "", moneyValue);
            Printer.Separator();
        
            var change = Localization.GetString("Change");
            var changeValue = transaction.Change.ToMoneyString(Localization.CultureInfo);
        
            Printer.AppendSpaceBetweenLines(change, "", changeValue);
            Printer.Separator();
        
            Printer.Feed(1);
        }

        #endregion

        #region consents

        {
            var consents = Localization.GetString("Consents");
        
            NokoCommonMod.ForEachStringSplit(consents, NewLineRegex(), (key, value) =>
            {
                // Printer.AlignLeft();
                Printer.AlignCenter();
                Printer.CondensedMode(PrinterModeState.On);
            
                // var prefix = key == 0 ? "*" : " ";
                // Printer.Append($"{prefix}{value.ToLower()}");
                Printer.Append(value);
            
                Printer.CondensedMode(PrinterModeState.Off);
                Printer.Feed(1);    
            });

            Printer.Feed(1);
        }

        #endregion

        #region footer

        {
            var messages = Localization.GetString("Messages");
        
            NokoCommonMod.ForEachStringSplit(messages, NewLineRegex(), (key, value) =>
            {
                Printer.AlignCenter();
                Printer.Append(value);
                Printer.Feed(1);
            });

            Printer.Feed(1);
            Printer.QrCode(transaction.QrCode, QrCodeSize.Size2);
            Printer.Feed(4);
            Printer.Separator();
        }

        #endregion

        Printer.Feed(8);
        Printer.FullPaperCut();
        Printer.End();
        
        Printer.PrintDocument();
    }

    public static Transaction GetTransactionTest()
    {
        var carts = new List<Cart>
        {
            // for (var i = 0; i < 5; i++)
            // {
            //     carts.Add(new Cart {
            //         Name = "Paracetamol",
            //         Type = "Tablets",
            //         Price = 10000,
            //         Qty = 12,
            //         Total = 120000,
            //     });
            // }
            new Cart {
                Name = "Paracetamol",
                Type = "Tablets",
                Price = 2000,
                Qty = 6,
                Total = 12000,
            },
            new Cart {
                Name = "Amoxicillin",
                Type = "Strips",
                Price = 8000,
                Qty = 4,
                Total = 32000,
            },
            new Cart {
                Name = "Omeprazole",
                Type = "Tablets",
                Price = 2000,
                Qty = 3,
                Total = 6000,
            }
        };

        return new Transaction
        {
            Code = "087C8D9A",
            Cashier = "Angelia, Emilia John",
            Total = 50000,
            TaxRate = 12,
            Tax = 6000,
            Pay = 56000,
            Money = 100000,
            Change = 44000,
            QrCode = "https://www.alodev.id/",
            Carts = carts,
        };
    }
}
