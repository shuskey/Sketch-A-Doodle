using Assets.Scripts.DataProviders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OnClickClearScores : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {

        var listOfHighScoresFromDataBase = new ListOfHighScoresFromDataBase();
        listOfHighScoresFromDataBase.RemoveHighScoresForThisMaze(MazePlayMode.currentMazeLevel);

        SceneManager.LoadScene("Scenes/Intro");
    }
}
