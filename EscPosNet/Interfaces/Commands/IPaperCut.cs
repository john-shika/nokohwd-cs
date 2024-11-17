namespace EscPosNet.Interfaces.Commands;

internal interface IPaperCut
{
    byte[] Full();
    byte[] Partial();
}
