using System.Collections;
using System.Drawing;
using System.Runtime.Versioning;
using EscPosNet.Extensions;
using EscPosNet.Interfaces.Commands;

namespace EscPosNet.Commands;

public class Image : IImage
{
    [SupportedOSPlatform("windows")]
    private static BitmapData GetBitmapData(Bitmap bmp)
    {

            const int threshold = 127;
            const double multiplier = 576; // this depends on your printer model.
            
            var index = 0;
            
            var scale = multiplier / bmp.Width;
            var xHeight = (int)(bmp.Height * scale);
            var xWidth = (int)(bmp.Width * scale);
            var dimensions = xWidth * xHeight;
            var dots = new BitArray(dimensions);

            for (var y = 0; y < xHeight; y++)
            {
                for (var x = 0; x < xWidth; x++)
                {
                    var x1 = (int)(x / scale);
                    var y1 = (int)(y / scale);
                    var color = bmp.GetPixel(x1, y1);
                    var luminance = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    dots[index] = (luminance < threshold);
                    index++;
                }
            }

            return new BitmapData()
            {
                Dots = dots,
                Height = (int)(bmp.Height * scale),
                Width = (int)(bmp.Width * scale)
            };
   
    }

    [SupportedOSPlatform("windows")]
    byte[] IImage.Print(Bitmap image)
    {
        var data = GetBitmapData(image);
        
        var dots = data.Dots;
        var width = BitConverter.GetBytes(data.Width);

        var offset = 0;
        var stream = new MemoryStream();
        var bw = new BinaryWriter(stream);

        bw.Write((byte)0x1b);
        bw.Write('@'.ToByte());

        bw.Write((byte)0x1b);
        bw.Write('3'.ToByte());
        bw.Write((byte)24);

        while (offset < data.Height)
        {
            bw.Write((byte)0x1b);
            bw.Write('*'.ToByte());
            bw.Write('!'.ToByte());
            
            // LE-SHORT
            bw.Write(width[0]);
            bw.Write(width[1]);

            for (var x = 0; x < data.Width; ++x)
            {
                for (var k = 0; k < 3; ++k)
                {
                    byte slice = 0;
                    for (var b = 0; b < 8; ++b)
                    {
                        var y = (((offset / 8) + k) * 8) + b;
                        

                        var i = (y * data.Width) + x;

                        var v = false;
                        if (i < dots.Length)
                        {
                            v = dots[i];
                        }
                        
                        slice |= (byte)((v ? 1 : 0) << (7 - b));
                    }

                    bw.Write(slice);
                }
            }
            
            offset += 24;
            
            // CRLF
            bw.Write('\r'.ToByte());
            bw.Write('\n'.ToByte());
        }
        
        // Restore the line spacing to the default of 30 dots.
        bw.Write((byte)0x1b);
        bw.Write('3'.ToByte());
        bw.Write((byte)30);

        bw.Flush();
        var bytes = stream.ToArray();
        bw.Dispose();
        return bytes;
    }
}

public class BitmapData
{
    public required BitArray Dots { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
}
