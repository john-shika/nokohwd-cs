using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;
using System.Text;

namespace EscPosNet.Extensions;

internal static class CustomExtensions
{
    public static byte ToByte(this char c)
    {
        return (byte)c;
    }

    public static byte ToByte(this Enum c)
    {
        // INT to INT16 to BYTES LE-SHORT
        var bytes = BitConverter.GetBytes(Convert.ToInt16(c));
        
        // take SINGLE-BYTE
        return bytes[0];
    }

    public static byte ToByte(this short c)
    {
        return (byte)c;
    }

    public static byte[] AddBytes(this byte[] bytes, byte[]? other)
    {
        if (other == null)
            return bytes;

        var list = new List<byte>();
        list.AddRange(bytes);
        list.AddRange(other);
        return list.ToArray();
    }

    public static byte[] AddBytes(this byte[] bytes, string value)
    {
        if (string.IsNullOrEmpty(value))
            return bytes;

        var list = new List<byte>();
        list.AddRange(bytes);
        list.AddRange(Encoding.GetEncoding(850).GetBytes(value));
        return list.ToArray();
    }

    public static byte[] AddLf(this byte[] bytes)
    {
        return bytes.AddBytes("\n");
    }

    public static byte[] AddCrLf(this byte[] bytes)
    {
        return bytes.AddBytes("\r\n");
    }

    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
}
