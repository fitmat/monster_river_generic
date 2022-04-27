using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostPlayerSessions : MonoBehaviour
{
    public int SessionCount = 10;
    public YipliUtils.PlayerActions requiredAction;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("In Start() of PostPlayerSessions");
        StartCoroutine(generatedDummyPlayerSessionsForLoadTesting());
    }

    private IEnumerator generatedDummyPlayerSessionsForLoadTesting()
    {
        //PlayerSession.Instance.StartSPSession("eggcatcher");
        yield return new WaitForSecondsRealtime(1.0f);
        Debug.Log("Posting player sessions Started");
        try
        {
            for (int i = 0; i < SessionCount; i++)
            {
                Dictionary<string, dynamic> tempData = _T_GetDummyPlayerSessionDataJsonDic(requiredAction);
                FirebaseDBHandler._T_PostDummyPlayerSession(tempData);
            }
        }
        catch(Exception exp)
        {
            Debug.Log("Exception occured in Posting sessions : " + exp.Message);
        }
        yield return new WaitForSecondsRealtime(1.0f);
        Debug.Log("Posting player sessions Completed");
    }

    public static IDictionary<YipliUtils.PlayerActions, int> _T_getDummyPlayerActions(YipliUtils.PlayerActions inputAction)
    {
        IDictionary<YipliUtils.PlayerActions, int> playerActionCounts = new Dictionary<YipliUtils.PlayerActions, int>();

        playerActionCounts.Add(inputAction, 100);

        if (playerActionCounts.ContainsKey(YipliUtils.PlayerActions.RUNNING))
            playerActionCounts[YipliUtils.PlayerActions.RUNNING] = playerActionCounts[YipliUtils.PlayerActions.RUNNING] + 1000;
        else
            playerActionCounts.Add(YipliUtils.PlayerActions.RUNNING, 1000);

        return playerActionCounts;
    }

    public static Dictionary<string, dynamic> _T_GetDummyPlayerSessionDataJsonDic(YipliUtils.PlayerActions inputAction)
    {
        Debug.Log("In _T_GetDummyPlayerSessionDataJsonDic()");

        Dictionary<string, dynamic> dummyPlayerGameSessionData;
        string[] validPlayerIds = {"-M2iG0P2_UNsE2VRcU5P",
                                    "-M2w4KLzwrYAJazfUiVk",
                                    "-M3HbYi8E7R-8A5aQwIe",
                                    "-M76fDnU3flm2kJtZLej",
                                    "-M7DmXj_rRc7OaMYUOiT",
                                    "-M7MEaBdQsdURULQ8mBl",
                                    "-M7MW0ooyJJmyHJdXjcx",
                                    "-M7W_5gJ089KDR8qw66A",
                                    "-M9qjcsLm6PIlA4ZnPsc",
                                    "-M9w0uOCu4KQNQXtWndZ",
                                    "-MCqsjHKKbeXdst-zmIM",
                                    "-MFH8Rc5Bao_TCUcg4jP",
                                    "-MFotyPFL5xjSP_y-he0"};

        string[] validGameIds = {"eggcatcher",
            "trapped",
            "joyfuljumps",
            "yiplirunner",
            "skater",
            "rollingball"};

        dummyPlayerGameSessionData = new Dictionary<string, dynamic>();
        try
        {
            //dummyPlayerGameSessionData.Add("game-id", validGameIds[UnityEngine.Random.Range(0, validGameIds.Length - 1)]);
            dummyPlayerGameSessionData.Add("game-id", "joyfuljumps");
            dummyPlayerGameSessionData.Add("user-id", "F9zyHSRJUCb0Ctc15F9xkLFSH5f1");
            dummyPlayerGameSessionData.Add("player-id", validPlayerIds[UnityEngine.Random.Range(0, validPlayerIds.Length - 1)]);
            dummyPlayerGameSessionData.Add("age", UnityEngine.Random.Range(6, 60));
            dummyPlayerGameSessionData.Add("points", UnityEngine.Random.Range(0, 500));
            dummyPlayerGameSessionData.Add("player", UnityEngine.Random.Range(120, 200));
            dummyPlayerGameSessionData.Add("duration", UnityEngine.Random.Range(60, 500));
            dummyPlayerGameSessionData.Add("intensity", UnityEngine.Random.Range(1, 3));
            dummyPlayerGameSessionData.Add("player-actions", _T_getDummyPlayerActions(inputAction));
            dummyPlayerGameSessionData.Add("mac-address", "-MGDS-zCTSVHfd4JWB2H");
            dummyPlayerGameSessionData.Add("calories", UnityEngine.Random.Range(10, 100));
            dummyPlayerGameSessionData.Add("fitness-points", UnityEngine.Random.Range(500, 20000));
            double timeStamp = new DateTime(2020, UnityEngine.Random.Range(1, 12), UnityEngine.Random.Range(1, 25)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            Debug.Log("Currrent TimeStamp : " + timeStamp);
            Debug.Log("Sending TimeStamp : " + timeStamp);
            dummyPlayerGameSessionData.Add("timestamp", timeStamp);
        }
        catch (Exception exp)
        {
            Debug.Log("Exception in filling data : " + exp.Message);
        }

        return dummyPlayerGameSessionData;
    }
}
