#if UNITY_STANDALONE_WIN
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace yipli.Windows
{
    public static class FileReadWrite
    {
        static string myDocLoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        static readonly string yipliFolder = "Yipli";
        static readonly string yipliFile = "userinfo.txt";
        
        //static readonly string yipliAppDownloadUrl = "https://www.playyipli.com/download.html";
        static string yipliAppDownloadUrl = "";

        static bool logOutFlag = false;
        static string userIdInFile = null;
        static string skippedVersion = null;
        static bool driverInstalledFinished = false;
        static string currentPlayerID = string.Empty;

        static string[] otherPorcessList = { 
            "Metro Rush",
            "Trapped",
            "Joyful Jumps"
        };

        static RegistryKey rk = Registry.CurrentUser;

        public static string YipliAppDownloadUrl { get => yipliAppDownloadUrl; set => yipliAppDownloadUrl = value; }
        public static bool LogOutFlag { get => logOutFlag; set => logOutFlag = value; }
        public static string UserIdInFile { get => userIdInFile; set => userIdInFile = value; }
        public static string SkippedVersion { get => skippedVersion; set => skippedVersion = value; }
        public static bool DriverInstalledFinished { get => driverInstalledFinished; set => driverInstalledFinished = value; }
        public static string[] OtherPorcessList { get => otherPorcessList; set => otherPorcessList = value; }
        public static string CurrentPlayerID { get => currentPlayerID; set => currentPlayerID = value; }

        public static string ReadFromFile()
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                if (File.Exists(myDocLoc + "/Yipli/userinfo.txt"))
                {
                    string[] allLines = File.ReadAllLines(myDocLoc + "/Yipli/userinfo.txt");

                    for (int i = 0; i < allLines.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(allLines[i]))
                        {
                            switch (i)
                            {
                                case 0:
                                    UserIdInFile = allLines[i].Substring(startIndex: 17);
                                    break;
                                /*
                                case 1:
                                    if (allLines[i].Substring(18) != null || allLines[i].Substring(18) != "")
                                    {
                                        if (int.Parse(allLines[i].Substring(18)) == 0) {
                                            LogOutFlag = false;
                                        } else {
                                            LogOutFlag = true;
                                        }
                                    }
                                    else
                                    {
                                        LogOutFlag = true;
                                    }
                                    break;

                                case 2:
                                    SkippedVersion = allLines[i].Substring(startIndex: 18);
                                    break;

                                case 3:
                                    if (allLines[i].Substring(26) != null || allLines[i].Substring(startIndex: 26) != "")
                                    {
                                        DriverInstalledFinished = Boolean.Parse(allLines[i].Substring(startIndex: 26));
                                    }
                                    else
                                    {
                                        DriverInstalledFinished = false;
                                    }
                                    break;

                                case 4:
                                    break;
                                */

                                case 5:
                                    if (allLines[i].Substring(19) != null || allLines[i].Substring(19) != "")
                                    {
                                        CurrentPlayerID = allLines[i].Substring(19);
                                    }
                                    else
                                    {
                                        CurrentPlayerID = string.Empty;
                                    }
                                    break;

                                /*
                                default:
                                    UserIdInFile = null;
                                    LogOutFlag = true;
                                    SkippedVersion = null;
                                    DriverInstalledFinished = false;
                                    CurrentPlayerID = string.Empty;
                                    break;
                                    */
                            }
                        }
                    }

                    return UserIdInFile;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                UnityEngine.Debug.LogError("Reading Failed : " + e.Message);
                return null;
            }
        }

        public static void WriteToFile(bool isUserLoggedOut = false)
        {
            if (!Directory.Exists(myDocLoc + "/" + yipliFolder))
            {
                Directory.CreateDirectory(myDocLoc + "/" + yipliFolder);
                var yipliFileToCreate = File.Create(myDocLoc + "/" + yipliFolder + "/" + yipliFile);

                yipliFileToCreate.Close();
            }

            string writeLine = "Current UserID : " + UserIdInFile + "\nisUserLoggedOut : " + isUserLoggedOut + "\nskipped version : " + SkippedVersion + "\nDriverInstalledFinished : " + DriverInstalledFinished;
            //UnityEngine.Debug.LogError("writeline is : " + writeLine); // 26

            StreamWriter sw = new StreamWriter(myDocLoc + "/Yipli/userinfo.txt");
            //sw.WriteLine(TripleDES.Encrypt(writeLine));
            sw.WriteLine(writeLine);
            sw.Close();

            //ReadFromFile();
        }

        public static void WriteToFileForDriverSetup(string gameID, bool isUserLoggedOut = false)
        {
            if (!Directory.Exists(myDocLoc + "/" + yipliFolder))
            {
                Directory.CreateDirectory(myDocLoc + "/" + yipliFolder);
                var yipliFileToCreate = File.Create(myDocLoc + "/" + yipliFolder + "/" + yipliFile);

                yipliFileToCreate.Close();
            }

            string writeLine = "Current UserID : " + UserIdInFile + "\nisUserLoggedOut : " + isUserLoggedOut + "\nskipped version : " + SkippedVersion + "\nDriverInstalledFinished : " + false + "\nGameID : " + gameID;
            //UnityEngine.Debug.LogError("writeline is : " + writeLine); // 26

            StreamWriter sw = new StreamWriter(myDocLoc + "/Yipli/userinfo.txt");
            //sw.WriteLine(TripleDES.Encrypt(writeLine));
            sw.WriteLine(writeLine);
            sw.Close();

            //ReadFromFile();
        }

        public static void OpenYipliApp()
        {
            string yipliAppExeLoc = GetApplictionInstallPath("yipliapp") + "\\" + "Yipli.exe";
            
            if (ValidateFile(yipliAppExeLoc))
            {
                Process.Start(yipliAppExeLoc);
            }
            else
            {
                Process.Start(YipliAppDownloadUrl);
            }
            
            //UnityEngine.Debug.LogError("Application is switched");
            UnityEngine.Application.Quit();
        }
        
        public static string GetApplictionInstallPath(string gameName)
        {
            string installPath = null;
            
            RegistryKey subKey = rk.OpenSubKey(gameName);
            
            try
            {
                installPath = subKey.GetValue("InstallPath").ToString();
                UnityEngine.Debug.LogError("sub key : " + subKey.GetValue("InstallPath"));
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("sub key not found. Error : " + e.Message);
            }
            
            return installPath;
        }
        
        public static bool ValidateFile(string fileLocation)
        {
            return File.Exists(fileLocation);
        }

        public static bool IsYipliPcIsInstalled()
        {
            string yipliAppExeLoc = GetApplictionInstallPath("yipliapp") + "\\" + "YipliApp.exe";

            return ValidateFile(yipliAppExeLoc);
        }

        public static void CheckIfMatDriverIsInstalled(string gameID)
        {
            Process.Start(GetMatDriverExePath(gameID));
        }

        public static string GetMatDriverExePath(string gameID)
        {
            string exePath = null;

            RegistryKey subKey = rk.OpenSubKey(gameID);

            try
            {
                exePath = subKey.GetValue("MatDriverCheck").ToString();
                UnityEngine.Debug.LogError("Exepath : " + exePath);
            }
            catch (Exception e)
            {
               UnityEngine.Debug.LogError("Exepath sub key not found. Error : " + e.Message);
            }

            return exePath;
        }

        public static bool CheckIfOtherProcessesAreRunning()
        {
            int totalYipliProcess = 0;

            for (int i = 0; i < OtherPorcessList.Length; i++)
            {
                Process[] allProcess = Process.GetProcessesByName(OtherPorcessList[i]);

                if (allProcess.Length > 0)
                {
                    totalYipliProcess++;
                }
            }

            if (totalYipliProcess > 0)
            {
                return true;
            }
            else
            { 
                return false;
            }

        }
    }
}
#endif