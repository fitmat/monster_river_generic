using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDetails
{
    public string userId = ""; // to be recieved from Yipli
    public string gameId = ""; // to be assigned to every game.
    public string playerId = ""; // to be recieved from Yipli for each game
    public float points; // Game points / coins
    public string playerAge = ""; //Current age of the player
    public string playerHeight = ""; //Current height of the player
    public string playerWeight = ""; //Current height of the player
    //public string matId = "";
    //public string matMacAddress;
    public string minigameId = "";
    public float duration;
    public float calories;
    public float fitnesssPoints;
    public string intensityLevel = ""; // to be decided by the game.
    public IDictionary<YipliUtils.PlayerActions, int> playerActionCounts; // to be updated by the player movements
    public IDictionary<string, string> playerGameData; // to be used to store the player gameData like Highscore, last played level etc.

}


[CreateAssetMenu(menuName = "Multiplayer/MultiPlayerData")]
public class MultiPlayerData : ScriptableObject
{
    [SerializeField] private string playerOneName;
    [SerializeField] private string playerTwoName;
    [SerializeField] private Sprite playerOneImage;
    [SerializeField] private Sprite playerTwoImage;
    [SerializeField] private bool isSinglePlayer;
    [SerializeField] private PlayerDetails playerOneDetails;
    [SerializeField] private PlayerDetails playerTwoDetails;

    public string PlayerOneName { get => playerOneName; set => playerOneName = value; }
    public string PlayerTwoName { get => playerTwoName; set => playerTwoName = value; }
    public bool IsSinglePlayer { get => isSinglePlayer; set => isSinglePlayer = value; }
    public Sprite PlayerOneImage { get => playerOneImage; set => playerOneImage = value; }
    public Sprite PlayerTwoImage { get => playerTwoImage; set => playerTwoImage = value; }
    public PlayerDetails PlayerOneDetails { get => playerOneDetails; set => playerOneDetails = value; }
    public PlayerDetails PlayerTwoDetails { get => playerTwoDetails; set => playerTwoDetails = value; }
}
