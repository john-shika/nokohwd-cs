using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace EscPosNet.Helpers;

public static class PrinterHelper
{
    private const string WinSpoolDrv = "winspool.Drv";

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private class DocInfoA
    {
        [MarshalAs(UnmanagedType.LPStr)] public string? pDocName;
        [MarshalAs(UnmanagedType.LPStr)] public string? pOutputFile;
        [MarshalAs(UnmanagedType.LPStr)] public string? pDataType;
    }

    private delegate bool OpenPrinterDelegate([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);
    private delegate bool ClosePrinterDelegate(IntPtr hPrinter);
    private delegate bool StartDocPrinterDelegate(IntPtr hPrinter, int level, [In] [MarshalAs(UnmanagedType.LPStruct)] DocInfoA di);
    private delegate bool EndDocPrinterDelegate(IntPtr hPrinter);
    private delegate bool StartPagePrinterDelegate(IntPtr hPrinter);
    private delegate bool EndPagePrinterDelegate(IntPtr hPrinter);
    private delegate bool WritePrinterDelegate(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

    private static bool Print(string printerName, IntPtr pBytes, int dwCount)
    {
        var winSpoolLibHandle = NativeLibrary.Load(WinSpoolDrv);

        var openPrinterPtr = NativeLibrary.GetExport(winSpoolLibHandle, "OpenPrinterA");
        var closePrinterPtr = NativeLibrary.GetExport(winSpoolLibHandle, "ClosePrinter");
        var startDocPrinterPtr = NativeLibrary.GetExport(winSpoolLibHandle, "StartDocPrinterA");
        var endDocPrinterPtr = NativeLibrary.GetExport(winSpoolLibHandle, "EndDocPrinter");
        var startPagePrinterPtr = NativeLibrary.GetExport(winSpoolLibHandle, "StartPagePrinter");
        var endPagePrinterPtr = NativeLibrary.GetExport(winSpoolLibHandle, "EndPagePrinter");
        var writePrinterPtr = NativeLibrary.GetExport(winSpoolLibHandle, "WritePrinter");

        var openPrinter = Marshal.GetDelegateForFunctionPointer<OpenPrinterDelegate>(openPrinterPtr);
        var closePrinter = Marshal.GetDelegateForFunctionPointer<ClosePrinterDelegate>(closePrinterPtr);
        var startDocPrinter = Marshal.GetDelegateForFunctionPointer<StartDocPrinterDelegate>(startDocPrinterPtr);
        var endDocPrinter = Marshal.GetDelegateForFunctionPointer<EndDocPrinterDelegate>(endDocPrinterPtr);
        var startPagePrinter = Marshal.GetDelegateForFunctionPointer<StartPagePrinterDelegate>(startPagePrinterPtr);
        var endPagePrinter = Marshal.GetDelegateForFunctionPointer<EndPagePrinterDelegate>(endPagePrinterPtr);
        var writePrinter = Marshal.GetDelegateForFunctionPointer<WritePrinterDelegate>(writePrinterPtr);
        
        var dwError = 0;
        var dwWritten = 0;
        var bSuccess = false;

        var di = new DocInfoA
        {
            pDocName = "NokoHwd Print Document",
            pDataType = "RAW"
        };

        if (openPrinter(printerName.Normalize(), out var hPrinter, IntPtr.Zero))
        {
            if (startDocPrinter(hPrinter, 1, di))
            {
                if (startPagePrinter(hPrinter))
                {
                    bSuccess = writePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                    endPagePrinter(hPrinter);
                }
                endDocPrinter(hPrinter);
            }
            closePrinter(hPrinter);
        }
        
        if (bSuccess == false) dwError = Marshal.GetLastWin32Error();
        NativeLibrary.Free(winSpoolLibHandle);
        
        if (dwError != 0) throw new Exception($"Win32 error: dwError = {dwError}, dwWritten = {dwWritten}, hPrinter = 0x{hPrinter.ToInt64():X}");
        return bSuccess;        
    }
    
    public static bool PrintFile(string printerName, string path)
    {
        var fs = new FileStream(path, FileMode.Open);
        var br = new BinaryReader(fs);

        var nLength = Convert.ToInt32(fs.Length);
        var bytes = br.ReadBytes(nLength);
        var pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
        Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
        
        var bSuccess = Print(printerName, pUnmanagedBytes, nLength);
        Marshal.FreeCoTaskMem(pUnmanagedBytes);
        
        return bSuccess;
    }

    public static bool Print(string printerName, byte[] data)
    {
        var pUnmanagedBytes = Marshal.AllocCoTaskMem(data.Length);
        Marshal.Copy(data, 0, pUnmanagedBytes, data.Length);
        var retVal = Print(printerName, pUnmanagedBytes, data.Length);
        Marshal.FreeCoTaskMem(pUnmanagedBytes);
        return retVal;
    }

    public static bool Print(string printerName, string data)
    {
        var dwCount = data.Length;
        var pBytes = Marshal.StringToCoTaskMemAnsi(data);
        var result = Print(printerName, pBytes, dwCount);
        Marshal.FreeCoTaskMem(pBytes);

        return result;
    }
}
