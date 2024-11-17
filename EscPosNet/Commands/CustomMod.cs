using EscPosNet.Interfaces.Commands;
using EscPosNet.Extensions;

namespace EscPosNet.Commands;

public class CustomMod : ICustomMod
{
    public byte[] Smooth(bool enabled = true)
    {
        return enabled
            ? [0x1d, 'b'.ToByte(), 1]
            : [0x1d, 'b'.ToByte(), 0];
    }

    public byte[] End()
    {
        return [0xfa];
    }
}