using Firebase.Database;
using System;
using UnityEngine;

public class YipliPlayerInfo
{
    public string playerName;
    public string playerId;
    public string playerDob;
    public string playerAge; //Current age of the player
    public string playerHeight; //Current height of the player
    public string playerWeight; //Current height of the player
    //public string playerExpertyLevel;//The Experty level of the player at time of playing the game.
    //public string gender;
    public string profilePicUrl;
    public Sprite playerProfilePicIMG;
    public int isMatTutDone;
    //public string difficultyLevel; // to be decided by the game.

    const string matAdvertisingName = "YIPLI";

    public YipliPlayerInfo() { }

    public YipliPlayerInfo(string playerId, string playerName, string playerDob, string playerHeight, string playerWeight, string profilePicUrl = null, int pTutDone = 0)
    {
        this.playerId = playerId;
        this.playerName = char.ToUpper(playerName[0]) + playerName.Substring(1);
        this.playerDob = playerDob;
        if (playerDob == null || playerDob == "")
        {
            playerAge = "";
        }
        else
        {
            Debug.Log("Calculation player age for DOB : " + playerDob);
            playerAge = CalculateAge(playerDob);
        }
        this.playerHeight = playerHeight;
        this.playerWeight = playerWeight;
        this.profilePicUrl = profilePicUrl;
        this.isMatTutDone = pTutDone;
    }

    public YipliPlayerInfo(DataSnapshot snapshot, string key)
    {
        try
        {
            if (snapshot != null)
            {
                Debug.Log("filling the YipliPlayerInfo from Snapshot.");
                playerId = key.ToString();
                playerName = snapshot.Child("name").Value?.ToString() ?? "";
                playerName = char.ToUpper(playerName[0]) + playerName.Substring(1);
                playerWeight = snapshot.Child("weight").Value?.ToString() ?? "";
                playerHeight = snapshot.Child("height").Value?.ToString() ?? "";
                playerDob = snapshot.Child("dob").Value?.ToString() ?? "";
                isMatTutDone = snapshot.Child("mat-tut-done").Value == null ? 0 : YipliHelper.StringToIntConvert(snapshot.Child("mat-tut-done").Value.ToString());

                //DOB is stored in the format "mm-dd-yyyy" in the backend
                Debug.Log("DOB recieved from backend : " + playerDob);
                if (playerDob == "")
                {
                    Debug.Log("Player age is null.");
                    playerAge = "";
                }
                else
                {
                    playerAge = CalculateAge(playerDob);
                    Debug.Log("Got Player age : " + playerAge);
                }

                profilePicUrl = snapshot.Child("profile-pic-url").Value?.ToString() ?? "";

                //If playername is not found, set PlayerId to null
                if (playerName== "")
                {
                    playerId = null;
                }

                SetProfilePicForPlayer(profilePicUrl);

                Debug.Log("Player Found with details :" + playerAge + " " + playerHeight + " " + playerId + " " + playerWeight + " " + playerName + " ProfilePicUrl:" + profilePicUrl);
            }
            else
            {
                Debug.Log("DataSnapshot is null. Can't create YipliPlayerInfo instance.");
                playerId = null;
            }
        }
        catch (Exception exp)
        {
            Debug.Log("Exception in creating YipliPlayerInfo object from DataSnapshot : " + exp.Message);
            playerId = null;
        }
    }

    private async void SetProfilePicForPlayer(string profilePicUrlNew) {
        string onDeviceProfilePicPath = Application.persistentDataPath + "/" + profilePicUrlNew;
        playerProfilePicIMG = await FirebaseDBHandler.GetImageAsync(profilePicUrlNew, onDeviceProfilePicPath);
    }

    private string CalculateAge(string strDob /* 'mm-dd-yyyy' format */)
    {
        DateTime now = DateTime.Now;
        string[] tokens = strDob.Split('-');
        int month = int.Parse(tokens[0]);
        int year = int.Parse(tokens[2]);
        int day = int.Parse(tokens[1]);
        var years = now.Year - year;
        if (now.Month < month)
        {
            years--;
        }
        if (now.Month == month)
        {
            if (now.Day < day)
            {
                years--;
            }
        }
        return years.ToString();
    }
}

public class YipliMatInfo
{
    public string matName;
    public string matId;
    public string macAddress;
    public string matAdvertisingName;

    public YipliMatInfo() { }

    public YipliMatInfo(string matId, string macAddress)
    {
        this.matId = matId;
        this.macAddress = macAddress;
    }

    public YipliMatInfo(string matId, string macAddress, string matAdvertisingName)
    {
        this.matId = matId;
        this.macAddress = macAddress;
        this.matAdvertisingName = matAdvertisingName;
    }

    public YipliMatInfo(DataSnapshot snapshot, string key)
    {
        try
        {
            if (snapshot != null)
            {
                Debug.Log("filling the YipliMatInfo from Snapshot.");
                matId = key.ToString();
                matName = snapshot.Child("display-name").Value?.ToString() ?? "";
                macAddress = snapshot.Child("mac-address").Value?.ToString() ?? "";
                matAdvertisingName = snapshot.Child("mac-name").Value?.ToString() ?? matAdvertisingName;
                Debug.Log("Mat Found with details :" + matName + " " + macAddress + " " + matId + " " + matAdvertisingName);
            }
            else
            {
                Debug.Log("DataSnapshot is null. Can't create YipliMatInfo instance.");
                matId = null;
            }
        }
        catch (Exception exp)
        {
            Debug.Log("Exception in creating YipliMatInfo object from DataSnapshot : " + exp.Message);
            matId = null;
        }
    }
}


public class YipliInventoryGameInfo
{
    public string gameId;
    public string displayName;
    public string gamePackageId;
    public string gameVersion;
    //public int isGameUnderMaintenance = 0;
    public string androidMinVersion;
    public string androidTVMinVersion;
    public string iosMinVersion;
    public string winMinVersion;
    public string winCurrentVersion;
    public string iosCurrentVersion;
    public string versionUpdateMessage;
    public string maintenanceMessage;
    public string osListForMaintanence;
    public int maxDaysWithoutUpdate;
    public string onlyMatPlayMode = ",";

    YipliInventoryGameInfo(string gameId)
    {
        this.gameId = gameId;
    }

    public YipliInventoryGameInfo(DataSnapshot snapshot)
    {
        try
        {
            if (snapshot != null)
            {
                Debug.Log("onlyMatPlayMode : filling the GameInfo from Snapshot.");
                gameId = snapshot.Key;
                displayName = snapshot.Child("name").Value?.ToString() ?? "";
                gamePackageId = snapshot.Child("android-url").Value?.ToString() ?? "";
                gameVersion = snapshot.Child("current-version").Value?.ToString() ?? "";

                //isGameUnderMaintenance = YipliHelper.StringToIntConvert(snapshot.Child("is-game-under-maintenance").Value.ToString());
                osListForMaintanence = snapshot.Child("os-list-for-maintanence").Value.ToString();

                androidMinVersion = snapshot.Child("android-min-version").Value.ToString();
                androidTVMinVersion = snapshot.Child("android-tv-min-version").Value.ToString();
                iosMinVersion = snapshot.Child("ios-min-version").Value.ToString();
                winMinVersion = snapshot.Child("win-min-version").Value.ToString();

                winCurrentVersion = snapshot.Child("win-version").Value.ToString();
                iosCurrentVersion = snapshot.Child("ios-current-version").Value.ToString();

                maintenanceMessage = snapshot.Child("maintenance-message").Value.ToString();
                versionUpdateMessage = snapshot.Child("version-update-message").Value.ToString();

                if (snapshot.HasChild("max-days-without-update"))
                {
                    maxDaysWithoutUpdate = (int)snapshot.Child("max-days-without-update").Value;
                }
                else
                {
                    maxDaysWithoutUpdate = 6;
                }

                if (snapshot.HasChild("only-mat-play-mode"))
                {
                    onlyMatPlayMode = snapshot.Child("only-mat-play-mode").Value.ToString();
                }
                else
                {
                    onlyMatPlayMode = ",";
                }
            }
            else
            {
                Debug.Log("DataSnapshot is null. Can't create YipliGameInfo instance.");
                gameId = null;
            }
        }
        catch (Exception exp)
        {
            Debug.Log("Exception in creating YipliMatInfo object from DataSnapshot : " + exp.Message);
            gameId = null;
        }
    }

    public YipliInventoryGameInfo()
    {
    }
}

public class YipliThisUserTicketInfo
{
    public string bleTest;
    public string description;
    public string fileStorageLocation;
    public string timeCreated;
    public string usbTest;
    public int ticketStatus;

    public YipliThisUserTicketInfo(DataSnapshot snapshot)
    {
        try
        {
            if (snapshot != null)
            {
                Debug.Log("filling the Ticket info from Snapshot.");

                bleTest = snapshot.Child("ble-test").Value?.ToString() ?? "";
                description = snapshot.Child("description").Value?.ToString() ?? "";
                fileStorageLocation = snapshot.Child("file-storage-location").Value?.ToString() ?? "";
                timeCreated = snapshot.Child("time-created").Value.ToString();
                usbTest = snapshot.Child("usb-test").Value.ToString();
                ticketStatus = YipliHelper.StringToIntConvert(snapshot.Child("ticket-status").Value.ToString());
            }
            else
            {
                Debug.Log("DataSnapshot is null. Can't create UserTicketInfo instance.");
                bleTest = string.Empty;
                description = string.Empty;
                fileStorageLocation = string.Empty;
                timeCreated = string.Empty;
                usbTest = string.Empty;
                ticketStatus = 0;
            }
        }
        catch (Exception exp)
        {
            Debug.Log("Exception in creating UserTicketInfo object from DataSnapshot : " + exp.Message);
            bleTest = string.Empty;
            description = string.Empty;
            fileStorageLocation = string.Empty;
            timeCreated = string.Empty;
            usbTest = string.Empty;
            ticketStatus = 0;
        }
    }
}