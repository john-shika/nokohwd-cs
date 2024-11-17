using System.Drawing;

namespace EscPosNet.Interfaces.Commands;

internal interface IImage
{
    byte[] Print(Bitmap image);
}
