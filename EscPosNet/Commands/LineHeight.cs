using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

public class LineHeight : ILineHeight
{
    public byte[] Normal()
    {
        return [0x1b, '3'.ToByte(), 30];
    }

    public byte[] SetLineHeight(int height)
    {
        var bytes = BitConverter.GetBytes(Convert.ToInt16(height));
        
        return [0x1b, '3'.ToByte(), bytes[0]];
    }
}
