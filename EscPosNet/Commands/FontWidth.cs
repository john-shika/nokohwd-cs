using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

public class FontWidth : IFontWidth
{
    public byte[] Normal()
    {
        return [0x1b, '!'.ToByte(), 0];
    }

    public byte[] DoubleWidth2()
    {
        return [0x1d, '!'.ToByte(), 16];
    }

    public byte[] DoubleWidth3()
    {
        return [0x1d, '!'.ToByte(), 32];
    }
}
