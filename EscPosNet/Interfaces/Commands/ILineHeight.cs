namespace EscPosNet.Interfaces.Commands;

interface ILineHeight
{
    byte[] Normal();
    byte[] SetLineHeight(int height);
}
