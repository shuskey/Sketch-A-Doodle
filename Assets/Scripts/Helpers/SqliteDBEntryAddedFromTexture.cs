using Assets.Scripts.DataObjects;
using Assets.Scripts.DataProviders;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SqliteDBEntryAddedFromTexture
{
    static string getSqlitePathToDataBaseFile() =>
   $"URI=file:{Application.persistentDataPath}/maze.db";

    [MenuItem("Maze/DataBase Add From Black Lined Texture")]
    static void CreateDBEntryBlackLines()
    {
        Texture2D mazeTexture = Selection.activeObject as Texture2D;
        CreateDBEntry(mazeTexture, invertToUseBlackLines: true);
    }

    // Found At https://answers.unity.com/questions/1349349/heightmap-from-texture-script-converter.html
    [MenuItem("Maze/DataBase Add From White Lined Texture")]
    static void CreateDBEntryWhiteLines()
    {
        Texture2D mazeTexture = Selection.activeObject as Texture2D;
        CreateDBEntry(mazeTexture);
    }

    static void CreateDBEntry(Texture2D mazeTexture, bool invertToUseBlackLines = false)
    {
        if (mazeTexture == null)
        {
            EditorUtility.DisplayDialog("No texture selected", "Please select a Maze Image Texture.", "Cancel");
            return;
        }

        var newMazeLevel = new MazeLevel(AssetDatabase.GetAssetPath(mazeTexture), invertToUseBlackLines);
        newMazeLevel.createdDate = System.DateTime.Now.Date;
        newMazeLevel.title = mazeTexture.name;

        AddMazeToDB(newMazeLevel);
    }

    static void AddMazeToDB(MazeLevel newMazeLevel)
    {
        using (SqliteConnection sqlconn = new SqliteConnection())
        {
            sqlconn.ConnectionString = getSqlitePathToDataBaseFile();
            sqlconn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = sqlconn;
                command.CommandText =
                    "INSERT INTO mazeLevels (title, creator, mazeTextureFileName, invertToUseBlackLines, " +
                    "startPositionRatioX, startPositionRatioY, endPositionRatioX, endPositionRatioY, " +
                    "createdDate, numberOfPlayThroughs) VALUES " +
                    "(:title, :creator, :mazeTextureFileName, :invertToUseBlackLines," +
                    ":startPositionRatioX, :startPositionRatioY, :endPositionRatioX, :endPositionRatioY, " +
                    ":createdDate, :numberOfPlayThroughs)";
                command.Parameters.Add("title", DbType.String).Value = newMazeLevel.title;
                command.Parameters.Add("creator", DbType.String).Value = newMazeLevel.creator;
                command.Parameters.Add("mazeTextureFileName", DbType.String).Value = newMazeLevel.mazeTextureFileName;
                command.Parameters.Add("invertToUseBlackLines", DbType.Int32).Value = newMazeLevel.invertToUseBlackLines;
                command.Parameters.Add("startPositionRatioX", DbType.Double).Value = newMazeLevel.startPositionRatio.x;
                command.Parameters.Add("startPositionRatioY", DbType.Double).Value = newMazeLevel.startPositionRatio.y;
                command.Parameters.Add("endPositionRatioX", DbType.Double).Value = newMazeLevel.endPositionRatio.x;
                command.Parameters.Add("endPositionRatioY", DbType.Double).Value = newMazeLevel.endPositionRatio.y;
                command.Parameters.Add("createdDate", DbType.DateTime).Value = newMazeLevel.createdDate;
                command.Parameters.Add("numberOfPlayThroughs", DbType.Int32).Value = newMazeLevel.numberOfPlayThroughs;

                command.ExecuteNonQuery();
            }
        }

    }
}
