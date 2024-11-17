namespace EscPosNet.Interfaces.Commands;

public interface ICustomMod
{
    byte[] Smooth(bool enabled = true);
    byte[] End();
}