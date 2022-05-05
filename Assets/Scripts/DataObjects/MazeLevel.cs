using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DataObjects
{
    public class MazeLevel
    {
        [UnityEngine.Header("Database index for this maze")]
        public int mazeId;
        [UnityEngine.Header("Title for this maze..")] 
        public string title;
        [UnityEngine.Header("Who created this maze?")] 
        public string creator;
        [UnityEngine.Header("The File Name for the texture for this maze.")] 
        public string mazeTextureFileName;
        [UnityEngine.Header("True if you have a maze with a white background.")] 
        public bool invertToUseBlackLines;
        [UnityEngine.Header("Range 0.0 to 1.0 for each coordinate.")] public UnityEngine.Vector2 startPositionRatio;  // range from 0.0 to 1.0 for each x and y
        [UnityEngine.Header("Range 0.0 to 1.0 for each coordinate.")] public UnityEngine.Vector2 endPositionRatio;
        [UnityEngine.Header("Date Created")] public System.DateTime createdDate;
        [UnityEngine.Header("Populatity based on number of completed plays")] public int numberOfPlayThroughs;


        public MazeLevel(string mazeTextureFileName, bool invertToUseBlackLines) 
        {
            this.mazeId = 0;
            this.title = "";
            this.creator = MazePlayMode.currentPlayer;
            this.mazeTextureFileName = mazeTextureFileName;
            this.invertToUseBlackLines = invertToUseBlackLines;
            this.startPositionRatio = new Vector2(0, 0);
            this.endPositionRatio = new Vector2(1, 1);
            this.createdDate = DateTime.Now.Date;
            this.numberOfPlayThroughs = 0;
        }

        public MazeLevel(int mazeId, string title, string creator, string mazeTextureFileName,
            bool invertToUseBlackLines, UnityEngine.Vector2 startPositionRatio, UnityEngine.Vector2 endPositionRatio,
            System.DateTime createdDate, int numberOfPlayThroughs)
        {
            this.mazeId = mazeId; 
            this.title = title;
            this.creator = creator;
            this.mazeTextureFileName = mazeTextureFileName;
            this.invertToUseBlackLines = invertToUseBlackLines;
            this.startPositionRatio = startPositionRatio;
            this.endPositionRatio = endPositionRatio;           
            this.createdDate = createdDate;
            this.numberOfPlayThroughs = numberOfPlayThroughs;
        }
    }
}
