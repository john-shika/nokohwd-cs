using System.Text;
using EscPosNet.Enums;
using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

public class QrCode : IQrCode
{
    private static byte[] Size(QrCodeSize size)
    {
        return [0x1d, '('.ToByte(), 'k'.ToByte(), 3, 0, '1'.ToByte(), 'C'.ToByte(), (size + 3).ToByte()];
    }

    private static IEnumerable<byte> ModelQr()
    {
        return [0x1d, '('.ToByte(), 'k'.ToByte(), 4, 0, '1'.ToByte(), 'A'.ToByte(), '2'.ToByte(), 0];
    }

    private static IEnumerable<byte> ErrorQr()
    {
        return [0x1d, '('.ToByte(), 'k'.ToByte(), 3, 0, '1'.ToByte(), 'E'.ToByte(), '0'.ToByte()];
    }

    private static IEnumerable<byte> StoreQr(string qrData)
    {
        var length = qrData.Length + 3;
        
        var b = (byte)(length % 256);
        var b2 = (byte)(length / 256);

        return [0x1d, '('.ToByte(), 'k'.ToByte(), b, b2, '1'.ToByte(), 'P'.ToByte(), '0'.ToByte()];
    }

    private static IEnumerable<byte> PrintQr()
    {
        return [0x1d, '('.ToByte(), 'k'.ToByte(), 3, 0, '1'.ToByte(), 'Q'.ToByte(), '0'.ToByte()];
    }

    public byte[] Print(string code)
    {
        return Print(code, QrCodeSize.Size0);
    }

    public byte[] Print(string code, QrCodeSize qrCodeSize)
    {
        var list = new List<byte>();
        list.AddRange(ModelQr());
        list.AddRange(Size(qrCodeSize));
        list.AddRange(ErrorQr());
        list.AddRange(StoreQr(code));
        list.AddRange(Encoding.UTF8.GetBytes(code));
        list.AddRange(PrintQr());
        return list.ToArray();
    }
}
