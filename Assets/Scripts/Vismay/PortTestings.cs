#if UNITY_STANDALONE_WIN
using System.IO.Ports;

public static class PortTestings
{
    private static string[] allComPorts = null;

    public static int CheckAvailableComPorts()
    {
        allComPorts = SerialPort.GetPortNames();

        if (allComPorts == null)
        {
            return 0;
        }
        else
        {
            return allComPorts.Length;
        }
    }
}
#endif