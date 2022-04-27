using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Multiplayer Game State Manager
 * Handles current data of Multiplayer Game
 * Sets player one data
 * Sets player two data
 * Handles game data of active players
 */


public class MP_GameStateManager
{

    #region Variable declaration

    public MultiPlayerData playerData;

    public List<string> playerNames = new List<string>();
    public string playerOne, playerTwo;
    public bool isSinglePlayer;
    public string minigameId;

    private YipliPlayerInfo tempPlayer;

    public PlayerDetails playerOneDetails = new PlayerDetails();
    public PlayerDetails playerTwoDetails = new PlayerDetails();

    #endregion

    public void SetPlayerData(MultiPlayerData multiPlayerData)
    {
        playerData = multiPlayerData;
    }

    private YipliPlayerInfo GetPlayerInfoFromPlayerName(string playerName)
    {
        if (MultiPlayerSelection.instance.currentYipliConfig.allPlayersInfo.Count > 0)
        {
            foreach (YipliPlayerInfo player in MultiPlayerSelection.instance.currentYipliConfig.allPlayersInfo)
            {
                Debug.Log("Found player : " + player.playerName);
                if (player.playerName == playerName)
                {
                    Debug.Log("Found player : " + player.playerName);
                    return player;
                }
            }
        }
        else
        {
            Debug.Log("No Players found.");
        }
        return null;
    }


    public YipliPlayerInfo GetPlayerInfoFromPlayerId(string playerId)
    {
        Debug.Log("Checking for player : " + playerId);
        if (MultiPlayerSelection.instance.currentYipliConfig.allPlayersInfo.Count > 0)
        {
            foreach (YipliPlayerInfo player in MultiPlayerSelection.instance.currentYipliConfig.allPlayersInfo)
            {
                Debug.Log("Checking player : " + player.playerId);
                if (player.playerId == playerId)
                {
                    Debug.Log("Found player : " + player.playerId);
                    return player;
                }
            }
        }
        else
        {
            Debug.Log("No Players found.");
        }
        return null;
    }

    public void SetPlayerOne(string name)
    {
        playerOne = name;
        playerData.PlayerOneName = playerOne;

        tempPlayer = GetPlayerInfoFromPlayerName(playerOne);

        playerOneDetails.userId = PlayerSession.Instance.currentYipliConfig.userId;
        //playerOneDetails.matId = PlayerSession.Instance.currentYipliConfig.matInfo.matId;
        //playerOneDetails.matMacAddress = PlayerSession.Instance.currentYipliConfig.matInfo.macAddress;
        playerOneDetails.playerId = tempPlayer.playerId;
        playerOneDetails.gameId = PlayerSession.Instance.currentYipliConfig.gameId;
        playerOneDetails.playerAge = tempPlayer.playerAge;
        playerOneDetails.playerHeight = tempPlayer.playerHeight;
        playerOneDetails.playerWeight = tempPlayer.playerWeight;
        playerOneDetails.playerActionCounts = new Dictionary<YipliUtils.PlayerActions, int>();
        playerOneDetails.playerGameData = new Dictionary<string, string>();

        playerData.PlayerOneDetails = playerOneDetails;
    }

    public void SetPlayerTwo(string name)
    {
        if (name == null)
        {
            playerTwo = null;
            playerData.PlayerTwoName = null;
            playerData.PlayerTwoDetails = null;
            return;
        }
        playerTwo = name;
        playerData.PlayerTwoName = playerTwo;
        playerData.IsSinglePlayer = false;

        tempPlayer = GetPlayerInfoFromPlayerName(playerTwo);

        playerTwoDetails.userId = PlayerSession.Instance.currentYipliConfig.userId;
        //playerTwoDetails.matId = PlayerSession.Instance.currentYipliConfig.matInfo.matId;
        //playerTwoDetails.matMacAddress = PlayerSession.Instance.currentYipliConfig.matInfo.macAddress;
        playerTwoDetails.gameId = PlayerSession.Instance.currentYipliConfig.gameId;
        playerTwoDetails.playerId = tempPlayer.playerId;
        playerTwoDetails.playerAge = tempPlayer.playerAge;
        playerTwoDetails.playerHeight = tempPlayer.playerHeight;
        playerTwoDetails.playerWeight = tempPlayer.playerWeight;
        playerTwoDetails.playerActionCounts = new Dictionary<YipliUtils.PlayerActions, int>();
        playerTwoDetails.playerGameData = new Dictionary<string, string>();

        playerData.PlayerTwoDetails = playerTwoDetails;

        isSinglePlayer = false;
    }

}
