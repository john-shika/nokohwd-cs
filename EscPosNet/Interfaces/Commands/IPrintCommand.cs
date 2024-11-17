namespace EscPosNet.Interfaces.Commands;

internal interface IPrintCommand
{
    int ColsNormal { get; }
    int ColsCondensed { get; }
    int ColsExpanded { get; }
    IFontMode FontMode { get; set; }
    IFontWidth FontWidth { get; set; }
    IAlignment Alignment { get; set; }
    IPaperCut PaperCut { get; set; }
    IDrawer Drawer { get; set; }
    IQrCode QrCode { get; set; }
    IBarCode BarCode { get; set; }
    IImage Image { get; set; }
    ILineHeight LineHeight { get; set; }
    IInitializePrint InitializePrint { get; set; }
    byte[] Separator(char c = '-');
    byte[] AutoTest();
}
