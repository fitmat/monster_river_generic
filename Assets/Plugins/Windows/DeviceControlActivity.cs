using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace FMInterface_Windows
{
    public class DeviceControlActivity
    {
        public static Thread readThread;

        [DllImport("FMDriver_DLL.dll")]
        private static extern int getGameID();

        [DllImport("FMDriver_DLL.dll")]
        private static extern void setGameID(int gameId);

        [DllImport("FMDriver_DLL.dll")]
        private static extern bool initSerialFramework();

        [DllImport("FMDriver_DLL.dll")]
        private static extern void readSerialData();

        [DllImport("FMDriver_DLL.dll")]
        private static extern void getFMResponse(byte []buff);

        [DllImport("FMDriver_DLL.dll")]
        private static extern void setGameMode(int _gameMode);

        [DllImport("FMDriver_DLL.dll")]
        private static extern int getGameMode();

        [DllImport("FMDriver_DLL.dll")]
        private static extern void setGameID_Multiplayer(int _P1_gameID, int _P2_gameID);

        [DllImport("FMDriver_DLL.dll")]
        private static extern int getGameID_Multiplayer(int _playerID);

        [DllImport("FMDriver_DLL.dll")]
        private static extern void getDriverVersion(byte[] buff);

        [DllImport("FMDriver_DLL.dll")]
        private static extern int isDeviceConnected();

        [DllImport("FMDriver_DLL.dll")]
        private static extern void disconnect();

        public static void InitPCFramework(int gameID)
        {

            if (initSerialFramework())
            {
                _setGameID(gameID);
                readThread = new Thread(() =>
                {
                    readSerialData();
                });
                readThread.Start();
            }
        }

        public static void _setGameID(int GameID)
        {
            setGameID(GameID);
        }

        public static void _setGameID(int P1_gameID, int P2_gameID)
        {
            setGameID_Multiplayer(P1_gameID, P2_gameID);
        }

        public static int _getGameID(int playerID)
        {
            return getGameID_Multiplayer(playerID);
        }

        public static int _getGameID()
        {
            return getGameID();
        }


        public static void _setGameMode(int GameMode)
        {
            setGameMode(GameMode);
        }

        public static int _getGameMode()
        {
            return getGameMode();
        }

        public static String _getFMResponse()
        {
            byte[] buf = new byte[1000];
            getFMResponse(buf);

            String response = System.Text.Encoding.UTF8.GetString(buf); 
            return response;
        }

        public static String _getDriverVersion()
        {
            byte[] buf = new byte[1000];
            getDriverVersion(buf);

            String version = System.Text.Encoding.UTF8.GetString(buf); 
            return version;
        }

        public static int _IsDeviceConnected()
        {
            return isDeviceConnected();
        }

        public static void _disconnect()
        {
            disconnect();
        }

        
    }
}
