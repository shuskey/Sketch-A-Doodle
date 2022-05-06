using Assets.Scripts.DataProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YourNewScoreScript : MonoBehaviour
{    
    [SerializeField] private Text yourNewScoreText;
    [SerializeField] private Button okayButton;

    // Start is called before the first frame update
    void Start()
    {

        okayButton.onClick.AddListener(OnOkayClick);
    }
    public void OnOkayClick()
    {
        //Time.timeScale = 1; // resume game
        SceneManager.LoadScene("Scenes/ChoosePlayMode");
    }

    public void GoalAchieved()
    {
        var displayText = $"{MazePlayMode.currentPlayer}, your time was {TimeToString(MazePlayMode.currentPlayerNewScore.timeInOneHundredsOfSeconds)} for {PlayModeToString()}.  ";
        // Check for personal best
        var listOfHighScores = new ListOfHighScoresFromDataBase();
        listOfHighScores.GetListOfHighScoresFromDataBase(MazePlayMode.currentMazeLevel.mazeId, MazePlayMode.mazePlayMode, MazePlayMode.currentPlayer);
        if (listOfHighScores.highScores.Count > 1)
            Debug.Log($"Warning: Player '{MazePlayMode.currentPlayer}' has {listOfHighScores.highScores.Count} highscore records for MazeId: {MazePlayMode.currentMazeLevel.mazeId} and {MazePlayMode.mazePlayMode}.");
        if (listOfHighScores.highScores.Count == 0)
        {
            displayText += "Your first score on the boards!  ";
            listOfHighScores.AddHighScore(MazePlayMode.currentPlayerNewScore);
            AddOverAllPlacement();
        }
        else if (listOfHighScores.highScores[0].timeInOneHundredsOfSeconds > MazePlayMode.currentPlayerNewScore.timeInOneHundredsOfSeconds)
        {
            displayText += "New Personal Best!  ";
            listOfHighScores.UpdateHighScore(listOfHighScores.highScores[0].id, MazePlayMode.currentPlayerNewScore);
            AddOverAllPlacement();
        } 
        else
        {
            displayText += $"Your previous best was {TimeToString(listOfHighScores.highScores[0].timeInOneHundredsOfSeconds)}.";
        }

        yourNewScoreText.text = displayText;

        string TimeToString(int timeToConvert)
        {
            return (string)TimeSpan.FromSeconds((float)timeToConvert/100.0f).ToString("mm':'ss'.'ff");
        }

        string PlayModeToString()
        {
            string playModeString = "";
            switch (MazePlayMode.mazePlayMode)
            {
                case Assets.Scripts.Enums.EnumMazePlayMode.None:
                    break;
                case Assets.Scripts.Enums.EnumMazePlayMode.PlayMode2D:
                    playModeString = "2D Play Mode";
                    break;
                case Assets.Scripts.Enums.EnumMazePlayMode.PlayMode3D:
                    playModeString = "3D Play Mode";
                    break;
                default:
                    break;
            }
            return playModeString;
        }

        void AddOverAllPlacement()
        {
            // get all scores for all players - see what place you are now in
            listOfHighScores.GetListOfHighScoresFromDataBase(MazePlayMode.currentMazeLevel.mazeId, MazePlayMode.mazePlayMode);
            int placeRanking = 1;
            foreach (var highScore in listOfHighScores.highScores)
            {
                if (highScore.playerName == MazePlayMode.currentPlayer)
                    break;
                placeRanking++;
            }
            string placeStringModifier = "th";
            switch (placeRanking)
            {
                case 1: placeStringModifier = "st"; break;
                case 2: placeStringModifier = "nd"; break;
                case 3: placeStringModifier = "rd"; break;
                default: placeStringModifier = "th"; break;
            }
            displayText += $"You are now in {placeRanking}{placeStringModifier} place! ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
