using Assets.Scripts.DataObjects;
using Assets.Scripts.DataProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestTimeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var listOfHighScoresFromDataBase = new ListOfHighScoresFromDataBase();
        listOfHighScoresFromDataBase.CreateTableForListOfHighScoresIfNotExists();
        listOfHighScoresFromDataBase.GetListOfHighScoresFromDataBase(
            MazePlayMode.currentMazeLevel.mazeId,
            MazePlayMode.mazePlayMode,
            MazePlayMode.currentPlayer);
        if (listOfHighScoresFromDataBase.highScores.Count != 0)
        {
            MazePlayMode.currentPlayerHighScore = listOfHighScoresFromDataBase.highScores[0];


            var bestTime = (float)MazePlayMode.currentPlayerHighScore.timeInOneHundredsOfSeconds / 100.0f;

            gameObject.GetComponent<Text>().text = "Best: " + TimeSpan.FromSeconds(bestTime).ToString("mm':'ss'.'ff");
        }
        else
        {
            gameObject.GetComponent<Text>().text = "Best: --:--.--";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
