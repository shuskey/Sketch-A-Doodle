using Assets.Scripts.DataProviders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OnClickDeleteMaze : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        var listOfMazesFromDataBase = new ListOfMazesFromDataBase();
        listOfMazesFromDataBase.RemoveMaze(MazePlayMode.currentMazeLevel);

        var listOfHighScoresFromDataBase = new ListOfHighScoresFromDataBase();
        listOfHighScoresFromDataBase.RemoveHighScoresForThisMaze(MazePlayMode.currentMazeLevel);

        SceneManager.LoadScene("Scenes/Intro");
    }
}
