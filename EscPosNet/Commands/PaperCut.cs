using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

public class PaperCut : IPaperCut
{
    public byte[] Full()
    {
        return [0x1d, 'V'.ToByte(), 'A'.ToByte(), 0];
    }

    public byte[] Partial()
    {
        return [0x1d, 'V'.ToByte(), 'A'.ToByte(), 1];
    }
}
