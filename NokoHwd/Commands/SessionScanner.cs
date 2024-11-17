using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using NokoHwd.Common;

namespace NokoHwd.Commands;

public sealed class SessionScannerOptions
{
    public required string Serial { get; set; }
    public int BaudRate { get; set; } = 9600;
    public Parity Parity { get; set; } = Parity.None;
    public int DataBits { get; set; } = 8;
    public StopBits StopBits { get; set; } = StopBits.One;
    public Handshake Handshake { get; set; } = Handshake.None;
    public int ReadTimeout { get; set; } = 500;
    public int WriteTimeout { get; set; } = 500;
}

public partial class SessionScanner
{
    [GeneratedRegex(@"\r\n|\r|\n")]
    private static partial Regex NewLineRegex();
    
    private static void SerialDataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e)
    {
        if (sender is not SerialPort sp) return;
        var data = sp.ReadExisting();
        var code = NewLineRegex().Replace(data, string.Empty);
        
        // var buff = Encoding.UTF8.GetBytes(data);
        // var hex = string.Join(" ", buff.Select(b => b.ToString("X2")).ToList());
        
        Console.WriteLine($"Data: {code}");
        // Console.WriteLine($"Hex: {hex}");
    }
    
    public SessionScanner(SessionScannerOptions options)
    {
        var portNames = SerialPort.GetPortNames();

        var found = portNames.FirstOrDefault(x => x.Equals(options.Serial, StringComparison.OrdinalIgnoreCase));
        if (found is null) NokoCommonMod.FatalError("No serial port found.");
        
        var serialPort = new SerialPort(options.Serial)
        {
            BaudRate = options.BaudRate,
            Parity = options.Parity,
            DataBits = options.DataBits,
            StopBits = options.StopBits,
            Handshake = options.Handshake,
            ReadTimeout = options.ReadTimeout,
            WriteTimeout = options.WriteTimeout,
        };
        
        serialPort.DataReceived += SerialDataReceivedEventHandler;
        
        try
        {
            serialPort.Open();
            NokoCommonMod.ConsoleReadLineByPrefix("EXIT");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}