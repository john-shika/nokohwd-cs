using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

public class Alignment : IAlignment

{
    public byte[] Left()
    {
        return [0x1b, 'a'.ToByte(), 0];
    }

    public byte[] Right()
    {
        return [0x1b, 'a'.ToByte(), 2];
    }

    public byte[] Center()
    {
        return [0x1b, 'a'.ToByte(), 1];
    }
}
