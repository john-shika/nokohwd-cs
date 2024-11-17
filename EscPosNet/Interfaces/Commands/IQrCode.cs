using EscPosNet.Enums;

namespace EscPosNet.Interfaces.Commands;

internal interface IQrCode
{
    byte[] Print(string code);
    byte[] Print(string code, QrCodeSize qrCodeSize);
}
