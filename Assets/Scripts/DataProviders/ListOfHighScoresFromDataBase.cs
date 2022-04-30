using Assets.Scripts.DataObjects;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using Assets.Scripts.Enums;
using System;

namespace Assets.Scripts.DataProviders
{
    class ListOfHighScoresFromDataBase : DataProviderBase
    {
        public List<MazeHighScore> highScores;

        public ListOfHighScoresFromDataBase()           
        {            
            highScores = new List<MazeHighScore>();
        }

        // TODO add clearHighScore(MazeId)
        // TODO removeHighScore(id)

        public void CreateTableForListOfHighScoresIfNotExists()
        {
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                sqlconn.ConnectionString = getSqlitePathToDataBaseFile();
                sqlconn.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS highScores " +
                        "(id INTEGER PRIMARY KEY ON CONFLICT ABORT AUTOINCREMENT," +
                        "mazeLevelId                INTEGER, " +
                        "mazePlayMode               INTEGER, " +
                        "playerName                 TEXT, " +
                        "dateAwarded                DATETIME, " +
                        "scoreAwarded               INTEGER, " +
                        "timeInOneHundredsOfSeconds INTEGER);";

                    IDataReader reader = command.ExecuteReader();
                    reader.Close();
                }
            }
        }

        public void GetListOfHighScoresFromDataBase(int mazeId, EnumMazePlayMode mazePlayMode)
        {
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                sqlconn.ConnectionString = getSqlitePathToDataBaseFile();
                sqlconn.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText =
                        "SELECT id, mazeLevelId, mazePlayMode, playerName, dateAwarded, scoreAwarded, timeInOneHundredsOfSeconds \n" +
                        "FROM highScores \n" +
                        $"WHERE mazeLevelId = {mazeId} AND mazePlayMode = {mazePlayMode}" +
                        "ORDER BY awardedScore DESC, timeInOneHundredsOfSeconds ASC;";

                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var highScore = new MazeHighScore(
                            id: reader.GetInt32(0),
                            mazeId: reader.GetInt32(1),
                            mazePlayMode: (EnumMazePlayMode)reader.GetInt32(2),
                            playerName: reader.GetString(3),
                            dateAwarded: reader.GetDateTime(4),
                            scoreAwarded: reader.GetInt32(5),
                            timeInOneHundredsOfSeconds: reader.GetInt32(6));
                        highScores.Add(highScore);
                    }

                    reader.Close();
                }
            }
        }

        public void AddHighScore(MazeHighScore newHighScore)
        {
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                sqlconn.ConnectionString = getSqlitePathToDataBaseFile();
                sqlconn.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText =
                        "INSERT INTO highScores (mazeId, mazePlayMode, playerName, " +
                        "dateAwarded, scoreAwarded, timeInOneHundredsOfSeconds) VALUES (?,?,?,?,?,?)";
                    command.Parameters.Add(newHighScore.mazeId);
                    command.Parameters.Add(newHighScore.mazePlayMode);
                    command.Parameters.Add(newHighScore.playerName);
                    command.Parameters.Add(newHighScore.dateAwarded);
                    command.Parameters.Add(newHighScore.scoreAwarded);
                    command.Parameters.Add(newHighScore.timeInOneHundredsOfSeconds);
                    command.ExecuteNonQuery();             
                }
            }
        }
    }        
}
