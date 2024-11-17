using System.Drawing;
using System.Text;
using EscPosNet.Commands;
using EscPosNet.Enums;
using EscPosNet.Helpers;
using EscPosNet.Interfaces;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet;

public class Printer : IPrinter
    {
        private byte[] _buffer;
        private readonly string _printerName;
        private readonly EscPos _command;
        private readonly string _codepage;
        
        public Printer(string printerName, string codepage= "IBM860")
        {
            _buffer = [];
            _printerName = printerName;
            _command = new EscPos();
            _codepage = codepage;
        }

        public int ColsNormal
        {
            get
            {
                return _command.ColsNormal;
            }
        }

        public int ColsCondensed
        {
            get
            {
                return _command.ColsCondensed;
            }
        }

        public int ColsExpanded
        {
            get
            {
                return _command.ColsExpanded;
            }
        }

        public void PrintDocument()
        {
            if (_buffer.Length == 0)
                return;
            if (!PrinterHelper.Print(_printerName, _buffer))
                throw new ArgumentException("Unable to access printer : " + _printerName);
        }

        public void Append(string value, bool newLine = true)
        {
            AppendString(value, newLine);
        }

        public void AppendSpaceBetweenLines(string left, string center, string right, bool newLine = true)
        {
            AlignCenter();
            var maxLines = ColsCondensed;
            CondensedMode(PrinterModeState.On);
            var result = new char[maxLines];
            for (var i = 0; i < maxLines; i++)
            {
                result[i] = ' ';
            }

            // Insert the left string
            for (var i = 0; i < left.Length && i < maxLines; i++)
            {
                result[i] = left[i];
            }

            // Calculate position for the center string
            var centerStart = (maxLines - center.Length) / 2;
            
            // Ensure centerStart is within bounds
            centerStart = Math.Max(0, centerStart);
            centerStart = Math.Min(maxLines - center.Length, centerStart);

            // Insert the center string
            for (var i = 0; i < center.Length && centerStart + i < maxLines; i++)
            {
                result[centerStart + i] = center[i];
            }

            // Insert the right string
            for (var i = 0; i < right.Length && maxLines - i - 1 >= 0; i++)
            {
                var j = right.Length - i - 1;
                result[maxLines - i - 1] = right[j];
            }

            // Convert the result array back to a string and append it
            AppendString(new string(result), newLine);
            CondensedMode(PrinterModeState.Off);
        }

        public void Append(byte[] value)
        {
            if (value.Length == 0)
                return;
            var list = new List<byte>();
            if (_buffer.Length > 0)
                list.AddRange(_buffer);
            list.AddRange(value);
            _buffer = list.ToArray();
        }

        private void AppendString(string value, bool newLine)
        {
            if (string.IsNullOrEmpty(value))
                return;
            if (newLine)
                value += "\r\n";
            var list = new List<byte>();
            if (_buffer.Length > 0)
                list.AddRange(_buffer);
            var bytes = Encoding.GetEncoding(_codepage).GetBytes(value);
            list.AddRange(bytes);
            _buffer = list.ToArray();
        }

        public void Feed(int count)
        {
            Append(_command.FontMode.Feed(count));
        }

        public void NewLine()
        {
            Append("\r\n");
        }

        public void NewLines(int lines)
        {
            for (int i = 1, loopTo = lines - 1; i <= loopTo; i++)
                NewLine();
        }

        public void Clear()
        {
            _buffer = [];
        }

        public void Separator(char c = '-')
        {
            Append(_command.Separator(c ));
        }

        public void AutoTest()
        {
            Append(_command.AutoTest());
        }

        public void TestPrinter()
        {
            Append("NORMAL - 48 COLUMNS");
            Append("1...5...10...15...20...25...30...35...40...45.48");
            Separator();
            Append("Text Normal");
            BoldMode("Bold Text");
            UnderlineMode("Underlined text");
            Separator();
            ExpandedMode(PrinterModeState.On);
            Append("Expanded - 23 COLUMNS");
            Append("1...5...10...15...20..23");
            ExpandedMode(PrinterModeState.Off);
            Separator();
            CondensedMode(PrinterModeState.On);
            Append("Condensed - 64 COLUMNS");
            Append("1...5...10...15...20...25...30...35...40...45...50...55...60..64");
            CondensedMode(PrinterModeState.Off);
            Separator();
            DoubleWidth2();
            Append("Font Width 2");
            DoubleWidth3();
            Append("Font Width 3");
            NormalWidth();
            Append("Normal width");
            Separator();
            AlignRight();
            Append("Right aligned text");
            AlignCenter();
            Append("Center-aligned text");
            AlignLeft();
            Append("Left aligned text");
            Separator();
            Font("Font A", Fonts.FontA);
            Font("Font B", Fonts.FontB);
            Font("Font C", Fonts.FontC);
            Font("Font D", Fonts.FontD);
            Font("Font E", Fonts.FontE);
            Font("Font Special A", Fonts.SpecialFontA);
            Font("Font Special B", Fonts.SpecialFontB);
            Separator();
            InitializePrint();
            Append("This is first line with line height of 30 dots");
            SetLineHeight(12);
            Append("This is third line with line height of 12 dots");
            SetLineHeight(24);
            Append("This is second line with line height of 24 dots");
            SetLineHeight(40);
            Append("This is third line with line height of 40 dots");
            NormalLineHeight();
            NewLines(3);
            Append("End of Test :)");
            Separator();
        }

        public void BoldMode(string value)
        {
            Append(_command.FontMode.Bold(value));
        }

        public void BoldMode(PrinterModeState state)
        {
            Append(_command.FontMode.Bold(state));
        }

        public void Font(string value, Fonts state)
        {
            Append(_command.FontMode.Font(value, state));
        }

        public void UnderlineMode(string value)
        {
            Append(_command.FontMode.Underline(value));
        }

        public void UnderlineMode(PrinterModeState state)
        {
            Append(_command.FontMode.Underline(state));
        }

        public void ExpandedMode(string value)
        {
            Append(_command.FontMode.Expanded(value));
        }

        public void ExpandedMode(PrinterModeState state)
        {
            Append(_command.FontMode.Expanded(state));
        }

        public void CondensedMode(string value)
        {
            Append(_command.FontMode.Condensed(value));
        }

        public void CondensedMode(PrinterModeState state)
        {
            Append(_command.FontMode.Condensed(state));
        }

        public void Smooth(bool enabled = true)
        {
            Append(_command.CustomMod.Smooth(enabled));
        }
        
        public void End()
        {
            Append(_command.CustomMod.End());
        }

        public void NormalWidth()
        {
            Append(_command.FontWidth.Normal());
        }

        public void DoubleWidth2()
        {
            Append(_command.FontWidth.DoubleWidth2());
        }

        public void DoubleWidth3()
        {
            Append(_command.FontWidth.DoubleWidth3());
        }

        public void AlignLeft()
        {
            Append(_command.Alignment.Left());
        }

        public void AlignRight()
        {
            Append(_command.Alignment.Right());
        }

        public void AlignCenter()
        {
            Append(_command.Alignment.Center());
        }

        public void FullPaperCut()
        {
            Append(_command.PaperCut.Full());
        }

        public void PartialPaperCut()
        {
            Append(_command.PaperCut.Partial());
        }

        public void OpenDrawer()
        {
            Append(_command.Drawer.Open());
        }

        public void QrCode(string code)
        {
            Append(_command.QrCode.Print(code));
        }

        public void QrCode(string code, QrCodeSize qrCodeSize )
        {
            Append(_command.QrCode.Print(code, qrCodeSize));
        }

        public void Code128(string code, Positions pos = Positions.NotPrint)
        {
            Append(_command.BarCode.Code128(code,  pos));
        }

        public void Code39(string code, Positions pos=Positions.NotPrint)
        {
            Append(_command.BarCode.Code39(code,  pos));
        }

        // public void Ean13(string code, Positions pos = Positions.NotPrint)
        // {
        //     Append(_command.BarCode.Ean13(code,  pos));
        // }

        public void InitializePrint()
        {
            PrinterHelper.Print(_printerName, _command.InitializePrint.Initialize());
        }

        public void Image(Bitmap image)
        {
            Append(_command.Image.Print(image));
        }
        public void NormalLineHeight()
        {
            Append(_command.LineHeight.Normal());
        }

        public void SetLineHeight(int height)
        {
            Append(_command.LineHeight.SetLineHeight(height));
        }
    }
