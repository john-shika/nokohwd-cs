using EscPosNet.Enums;
using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

public class BarCode : IBarCode
{
    public byte[] Code128(string code, Positions pos = Positions.NotPrint)
    {
        if (code.Length > 14)
            throw new ArgumentException("code length must be less than 14");
        
        var bytes = BitConverter.GetBytes(Convert.ToInt16(code.Length + 2));
        
        return new byte[] { 0x1d, 'w'.ToByte(), 2 } // Width
            .AddBytes([0x1d, 'h'.ToByte(), '2'.ToByte()]) // Height
            .AddBytes([0x1d, 'f'.ToByte(), 1])
            .AddBytes([0x1d, 'H'.ToByte(), pos.ToByte()])
            .AddBytes([0x1d, 'k'.ToByte(), 'I'.ToByte()])
            
            // must be SINGLE-BYTE
            .AddBytes([bytes[0]])
            
            .AddBytes(['{'.ToByte(), 'C'.ToByte()])
            .AddBytes(code)
            .AddLf();
    }

    public byte[] Code39(string code, Positions pos = Positions.NotPrint)
    {
        if (code.Length > 12)
            throw new ArgumentException("code length must be less than 12");
        
        return new byte[] { 0x1d, 'w'.ToByte(), 2 } // Width
            .AddBytes([0x1d, 'h'.ToByte(), '2'.ToByte()]) // Height
            .AddBytes([0x1d, 'f'.ToByte(), 0])
            .AddBytes([0x1d, 'H'.ToByte(), pos.ToByte()])
            .AddBytes([0x1d, 'k'.ToByte(), 4])
            .AddBytes(code)
            .AddBytes([0])
            .AddLf();
    }

    // public byte[] Ean13(string code, Positions pos = Positions.NotPrint)
    // {
    //     if (code.Trim().Length != 13)
    //         throw new ArgumentException("code length must be equal to 13");
    //
    //     return new byte[] { 0x1d, 'w'.ToByte(), 2 } // Width
    //         .AddBytes([0x1d, 'h'.ToByte(), '2'.ToByte()]) // Height
    //         .AddBytes([0x1d, 'H'.ToByte(), pos.ToByte()])
    //         .AddBytes([0x1d, 'k'.ToByte(), 'C'.ToByte(), 12])
    //         .AddBytes(code[..12])
    //         .AddLf();
    // }
}
