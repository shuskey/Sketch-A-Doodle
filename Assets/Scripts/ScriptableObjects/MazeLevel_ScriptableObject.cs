using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Maze", menuName = "Maze")]
public class MazeLevel_ScriptableObject : ScriptableObject
{
    [Header("Title for this maze..")]
    public string title;
    [Header("Who created this maze?")]
    public string creator;
    [Header("The texture for this maze.")]
    public Texture2D mazeTexture;
    [Header("If you have a maze with a white background, this should be true.")]
    public bool invertToUseBlackLines;
    [Header("Range 0.0 to 1.0 for each coordinate.")]
    public Vector2 startPositionRatio;  // range from 0.0 to 1.0 for each x and y
    [Header("Range 0.0 to 1.0 for each coordinate.")]
    public Vector2 endPositionRatio;  // range from 0.0 to 1.0 for each x and y
}
