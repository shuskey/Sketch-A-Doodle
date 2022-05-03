using Assets.Scripts.DataObjects;
using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayMode : MonoBehaviour
{
    public static EnumMazePlayMode mazePlayMode;
    public static MazeLevel currentMazeLevel;
    public static MazeHighScore currentPlayerHighScore;
    public static MazeHighScore currentPlayerNewScore;
    public static string currentPlayer;
}
