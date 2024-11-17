namespace EscPosNet.Interfaces.Commands;

internal interface IAlignment
{
    byte[] Left();
    byte[] Right();
    byte[] Center();
}
