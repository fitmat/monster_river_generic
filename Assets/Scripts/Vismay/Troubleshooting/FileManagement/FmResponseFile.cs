using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FmResponseFile
{
    private static string fileLogName;
    private static string fileLogPath;
    private static string fileFlowName;
    private static string fileFlowPath;

    private static string storagePath = string.Empty;

    public static string FileLogName { get => fileLogName; set => fileLogName = value; }
    public static string FileLogPath { get => fileLogPath; set => fileLogPath = value; }
    public static string FileFlowName { get => fileFlowName; set => fileFlowName = value; }
    public static string FileFlowPath { get => fileFlowPath; set => fileFlowPath = value; }
    public static string StoragePath { get => storagePath; set => storagePath = value; }

    #region LogFile
    private static void CreateLogFile()
    {
        // File name and File path
        //FileName = "FmResponseLogs_" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".txt";
        FileLogName = "FmResponseLogs.txt";
        FileLogPath = Application.dataPath + "/TroubleshootingModule/" + FileLogName;

        // Create File if doesn't exit
        if (!File.Exists(FileLogPath))
        {
            File.WriteAllText(FileLogPath, "Troubleshoot logs : " + DateTime.Now + " \n\n");
        }
        else
        {
            File.WriteAllText(FileLogPath, "Troubleshoot logs : " + DateTime.Now + " \n\n");
        }
    }

    private static void WriteResponseToFile(List<string> fmResponseList)
    {
        CreateLogFile();

        foreach (string s in fmResponseList)
        {
            File.AppendAllText(FileLogPath, s);
            File.AppendAllText(FileLogPath, "\n");
        }
    }
    #endregion

    // upload function
    private static async System.Threading.Tasks.Task UploadLogsAsync(string userID)
    {
        List<string> filePaths = new List<string>();
        List<string> fileNames = new List<string>();

        filePaths.Add(FileLogPath);
        filePaths.Add(FileFlowPath);

        fileNames.Add(FileLogName);
        fileNames.Add(FileFlowName);

        if (File.Exists(FileLogPath))
        {
            StoragePath = await FirebaseDBHandler.UploadLogsFileToDB(userID, fileNames, filePaths);
        }

        Debug.Log("File Upload Finished");
    }

    #region flow file

    private static void CreateFlowFile()
    {
        // File name and File path
        FileFlowName = "FmResponseFlowDetails.json";
        FileFlowPath = Application.dataPath + "/TroubleshootingModule/" + FileFlowName;

        // Create File if doesn't exit
        if (!File.Exists(FileFlowPath))
        {
            File.WriteAllText(FileFlowPath, "");
        }
        else
        {
            File.WriteAllText(FileFlowPath, "");
        }
    }

    private static void WriteFlowsToFile(string jsonData)
    {
        CreateFlowFile();

        File.AppendAllText(FileFlowPath, jsonData);
    }

    #endregion

    #region management Stuff

    public static async void GenerateFilesAndUpload(List<string> fmResponseList, string flowInfo, int troubleShootAlgoId, YipliConfig currentYipliConfig, string decription, string questionsAnswers)
    {
        FlowDetails fd = new FlowDetails();

        fd.title = "Troubleshoot flow";
        fd.date = DateTime.Now.ToString();
        fd.algorithmID = troubleShootAlgoId.ToString();
        fd.flowStructure = flowInfo;
        fd.scriptableValues = questionsAnswers;

        if (fmResponseList != null)
        {
            WriteResponseToFile(fmResponseList);
        }

        WriteFlowsToFile(fd.GetJson());

        // now upload files
        await UploadLogsAsync(currentYipliConfig.userId);

        Dictionary<string, object> currentTicket = new Dictionary<string, object>();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (!currentYipliConfig.thisUserTicketInfo.bleTest.Equals("done", StringComparison.OrdinalIgnoreCase))
            {
                currentTicket.Add("ble-test", "done");
            }
            else
            {
                currentTicket.Add("ble-test", "notDone");
            }
        }

        currentTicket.Add("description", decription);
        currentTicket.Add("file-storage-location", StoragePath);
        currentTicket.Add("time-created", DateTime.UtcNow.ToString());
        currentTicket.Add("user-email", await FirebaseDBHandler.GetEmailFromUserID(currentYipliConfig.userId));

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (!currentYipliConfig.thisUserTicketInfo.usbTest.Equals("done", StringComparison.OrdinalIgnoreCase))
            {
                currentTicket.Add("usb-test", "done");
            }
            else
            {
                currentTicket.Add("usb-test", "notDone");
            }
        }

        FreshDeskManager.SetTicketDataAndGenerate(currentTicket);
    }

    #endregion

    #region required classes

    public class FlowDetails
    {
        public string title = string.Empty;
        public string date = string.Empty;
        public string algorithmID = string.Empty;
        public string flowStructure = string.Empty;
        public string scriptableValues = string.Empty;

        public string GetJson()
        {
            return JsonUtility.ToJson(this);
        }
    }

    #endregion
}
