using EscPosNet.Enums;
using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

public class FontMode : IFontMode
{

    public byte[] Bold(string value)
    {
        return Bold(PrinterModeState.On)
            .AddBytes(value)
            .AddBytes(Bold(PrinterModeState.Off))
            .AddLf();
    }

    public byte[] Bold(PrinterModeState state)
    {
        return state == PrinterModeState.On
            ? [0x1b, 'E'.ToByte(), 1]
            : [0x1b, 'E'.ToByte(), 0];
    }

    public byte[] Underline(string value)
    {
        return Underline(PrinterModeState.On)
            .AddBytes(value)
            .AddBytes(Underline(PrinterModeState.Off))
            .AddLf();
    }

    public byte[] Underline(PrinterModeState state)
    {
        return state == PrinterModeState.On
            ? [0x1b, '-'.ToByte(), 1]
            : [0x1b, '-'.ToByte(), 0];
    }

    public byte[] Expanded(string value)
    {
        return Expanded(PrinterModeState.On)
            .AddBytes(value)
            .AddBytes(Expanded(PrinterModeState.Off))
            .AddLf();
    }

    public byte[] Expanded(PrinterModeState state)
    {
        return state == PrinterModeState.On
            ? [0x1d, '!'.ToByte(), 16]
            : [0x1d, '!'.ToByte(), 0];
    }

    public byte[] Condensed(string value)
    {
        return Condensed(PrinterModeState.On)
            .AddBytes(value)
            .AddBytes(Condensed(PrinterModeState.Off))
            .AddLf();
    }

    public byte[] Condensed(PrinterModeState state)
    {
        return state == PrinterModeState.On
            ? [0x1b, '!'.ToByte(), 1]
            : [0x1b, '!'.ToByte(), 0];
    }

    public byte[] Font(string value, Fonts state)
    {
        return Font(state)
       .AddBytes(value)
       .AddBytes(Font(Fonts.FontA))
       .AddLf();
    }

    public byte[] Font(Fonts state)
    {
        byte fnt = state switch
        {
            Fonts.FontA => 0,
            Fonts.FontB => 1,
            Fonts.FontC => 2,
            Fonts.FontD => 3,
            Fonts.FontE => 4,
            Fonts.SpecialFontA => 5,
            Fonts.SpecialFontB => 6,
            _ => 0
        };
        return [0x1b, 'M'.ToByte(), fnt];
    }

    public byte[] Feed(int count)
    {
        // LE-SHORT
        var bytes = BitConverter.GetBytes(Convert.ToInt16(count));
        
        // take SINGLE-BYTE
        return [0x1b, 'd'.ToByte(), bytes[0]];
    }
}