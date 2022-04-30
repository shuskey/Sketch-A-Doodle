using Assets.Scripts.Enums;

namespace Assets.Scripts.DataObjects
{
    public class MazeHighScore
    {
        [UnityEngine.Header("SQLite MazeHighScore Id index.")]
        public int id;
        [UnityEngine.Header("SQLite MazeLevel Id index.")]
        public int mazeId;
        [UnityEngine.Header("How was this Maze Played? 2D or 3D.")]
        public EnumMazePlayMode mazePlayMode;
        [UnityEngine.Header("Player Name")] 
        public string playerName;
        [UnityEngine.Header("Date Awarded")] 
        public System.DateTime dateAwarded;
        // perhaps Awarded Score calculated like 1000 - time in seconds + treasure_value X quatitly - enemy_damage X quantity - jump_cost X quantity
        [UnityEngine.Header("Awarded Score")] 
        public int scoreAwarded;
        [UnityEngine.Header("Time in seconds")] 
        public int timeInOneHundredsOfSeconds;

        public MazeHighScore(int id, int mazeId, EnumMazePlayMode mazePlayMode, string playerName, System.DateTime dateAwarded, int scoreAwarded, int timeInOneHundredsOfSeconds)
        {
            this.id = id;
            this.mazeId = mazeId;
            this.mazePlayMode = mazePlayMode;
            this.playerName = playerName;
            this.dateAwarded = dateAwarded;
            this.scoreAwarded = scoreAwarded;
            this.timeInOneHundredsOfSeconds = timeInOneHundredsOfSeconds;
        }
    }
}
