using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Multiplayer/GameDetails")]
public class GamesData : ScriptableObject
{
    [System.Serializable]
    public class GameDetails
    {
        public string gameName;
        public int difficultyValue;
        public Sprite gameDifficulty;
        public string sceneName;
        public GameObject gameInstructionPanel;
        public GameObject gameLoadingScreen;
        public Sprite gameScreenshot;
    }

    [SerializeField] private List<GameDetails> games;

    public List<GameDetails> Games { get => games; set => games = value; }
}
