using EscPosNet.Enums;
using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

internal class EscPos : IPrintCommand
{
    public IFontMode FontMode { get; set; }
    public IFontWidth FontWidth { get; set; }
    public IAlignment Alignment { get; set; }
    public IPaperCut PaperCut { get; set; }
    public IDrawer Drawer { get; set; }
    public IQrCode QrCode { get; set; }
    public IBarCode BarCode { get; set; }
    public ICustomMod CustomMod { get; set; }
    public IInitializePrint InitializePrint { get; set; }
    public IImage Image { get; set; }
    public ILineHeight  LineHeight { get; set; }
    public int ColsNormal => 48;
    public int ColsCondensed => 64 - 22; // 58 mm
    public int ColsExpanded => 24;        

    public EscPos()
    {
        FontMode = new FontMode();
        FontWidth = new FontWidth();
        Alignment = new Alignment();
        PaperCut = new PaperCut();
        Drawer = new Drawer();
        QrCode = new QrCode();
        BarCode = new BarCode();
        Image = new Image();
        LineHeight = new LineHeight();
        CustomMod = new CustomMod();
        InitializePrint = new InitializePrint();
    }

    public byte[] Separator(char c = '-')
    {
        return FontMode.Condensed(PrinterModeState.On)
            .AddBytes(new string(c, ColsCondensed))
            .AddBytes(FontMode.Condensed(PrinterModeState.Off))
            .AddCrLf();
    }

    public byte[] AutoTest()
    {
        return [0x1d, '('.ToByte(), 'A'.ToByte(), 2, 0, 0, 2];
    }

}
