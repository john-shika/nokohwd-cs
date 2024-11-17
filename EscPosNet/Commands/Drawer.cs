using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

public class Drawer : IDrawer
{
    public byte[] Open()
    {
        return [0x1b, 'p'.ToByte(), 0, '<'.ToByte(), 'x'.ToByte()];
    }
}
