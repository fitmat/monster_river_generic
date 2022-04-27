using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllQuestions : MonoBehaviour
{
    // required variables
    List<ProblemModel> gameQuestions;
    List<ProblemModel> matQuestions;

    void Start()
    {
        CreateLists();
    }

    #region Game Questions

    readonly ProblemModel game_question_1 = new ProblemModel
    (
        1,
        "Is game/ app crashing ?",
        new string[] { "Check OS settings" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_2 = new ProblemModel
    (
        2,
        "Is OS Updated to the latest ?",
        new string[] { "Device is not compatible with yipli environment. Check specification on Yipli website.", "Update OS" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_3 = new ProblemModel
    (
        3,
        "Are you stuck on Player fetching screen ?",
        new string[] { },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_4 = new ProblemModel
    (
        4,
        "Are you stuck to no mat connection page ?",
        new string[] { },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_5 = new ProblemModel
    (
        5,
        "Is Internet available ?",
        new string[] { "Turn on internet", "Backend Curruption" },
        new string[] { "Yes", "No", "Not Sure" }
    );
    readonly ProblemModel game_question_6 = new ProblemModel
    (
        6,
        "Is mat connected to USB ?",
        new string[] { "Connect Mat via USB" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_7 = new ProblemModel
    (
        7,
        "Is Phone BLE on ?",
        new string[] { "Turn on Bluetooth" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_8 = new ProblemModel
    (
        8,
        "Have you added the Mat to your account that  you are using ?",
        new string[] { "Add Mat in Yipli app", "Please make the current mat active in yipli app to connect" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_9 = new ProblemModel
    (
        9,
        "Are there any apps or games running in the background during gameplay ?",
        new string[] { "Kill all the yipli games and restart the game" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_10 = new ProblemModel
    (
        10,
        "Is your yipli app and games on latest version?",
        new string[] { "Update all applications" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_11 = new ProblemModel
    (
        11,
        "Is the behaviour same in all  the games ?",
        new string[] { "Please try with another games to be sure" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_12 = new ProblemModel
    (
        12,
        "Is the behaviour same on all the platforms ?",
        new string[] { "Please trouble shoot this problem on another platform" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel game_question_13 = new ProblemModel
    (
        13,
        "Is the behavoir persistent or random ?",
        new string[] { },
        new string[] { "Persistent", "Random", "Not Sure" }
    );

    #endregion

    #region Mat Questions

    readonly ProblemModel mat_question_1 = new ProblemModel
    (
        1,
        "Is Mat On ?",
        new string[] { "Turn on mat" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel mat_question_2 = new ProblemModel
    (
        2,
        "What is the color of  LED ?",
        new string[] { "Charge Mat" },
        new string[] { "Green", "Red", "Other" }
    );

    readonly ProblemModel mat_question_3 = new ProblemModel
    (
        3,
        "Is Charging  light visible ?",
        new string[] { "Battery Issue", "Restart Test" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel mat_question_4 = new ProblemModel
    (
        4,
        "Check if ble shows yipli as device ?",
        new string[] { "Looks Like Bluetooth in Mat is not working properly, please troubleshoot on Windows machine if possible." },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel mat_question_5 = new ProblemModel
    (
        5,
        "Is Silicon Driver Installed ?",
        new string[] { "Install silicon driver" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel mat_question_6 = new ProblemModel
    (
        6,
        "Is silicon port  available in Device Manager ?",
        new string[] { "Device Hardware Issue" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel mat_question_7 = new ProblemModel
    (
        7,
        "Are there any other device connected to Mat ?",
        new string[] { "Disconnect all the devices and try again" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    readonly ProblemModel mat_question_8 = new ProblemModel
    (
        8,
        "Is this the  same mat that is added in  backend ?",
        new string[] { "Add Mat in yipli app" },
        new string[] { "Yes", "No", "Not Sure" }
    );

    #endregion
    public List<ProblemModel> GameQuestions { get => gameQuestions; set => gameQuestions = value; }
    public List<ProblemModel> MatQuestions { get => matQuestions; set => matQuestions = value; }

    #region Question list List Management

    private void CreateLists()
    {
        GameQuestions = new List<ProblemModel>();
        MatQuestions = new List<ProblemModel>();

        // add game questions to list
        GameQuestions.Add(game_question_1);
        GameQuestions.Add(game_question_2);
        GameQuestions.Add(game_question_3);
        GameQuestions.Add(game_question_4);
        GameQuestions.Add(game_question_5);
        GameQuestions.Add(game_question_6);
        GameQuestions.Add(game_question_7);
        GameQuestions.Add(game_question_8);
        GameQuestions.Add(game_question_9);
        GameQuestions.Add(game_question_10);
        GameQuestions.Add(game_question_11);
        GameQuestions.Add(game_question_12);
        GameQuestions.Add(game_question_13);

        // add mat questions to list
        MatQuestions.Add(mat_question_1);
        MatQuestions.Add(mat_question_2);
        MatQuestions.Add(mat_question_3);
        MatQuestions.Add(mat_question_4);
        MatQuestions.Add(mat_question_5);
        MatQuestions.Add(mat_question_6);
        MatQuestions.Add(mat_question_7);
        MatQuestions.Add(mat_question_8);
    }

    #endregion
}
