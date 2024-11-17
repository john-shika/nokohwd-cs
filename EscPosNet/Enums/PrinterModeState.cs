namespace EscPosNet.Enums;

public enum PrinterModeState
{
    Off,
    On,
}

public static class PrinterModeStateExtensions
{
    public static bool GetBool(this PrinterModeState state)
    {
        return state switch
        {
            PrinterModeState.Off => false,
            PrinterModeState.On => true,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
    
    public static PrinterModeState ToPrinterModeState(this bool enabled)
    {
        return enabled switch
        {
            false => PrinterModeState.Off,
            true => PrinterModeState.On,
        };
    }
}
