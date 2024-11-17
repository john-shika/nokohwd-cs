using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

public class InitializePrint : IInitializePrint
{
    public byte[] Initialize()
    {
        return [ 0x1b, '@'.ToByte(), 0x1b, 't'.ToByte(), 40 ];
    }
}
