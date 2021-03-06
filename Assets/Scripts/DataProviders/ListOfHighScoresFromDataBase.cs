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
#if UNITY_WEBGL
            return;
#else
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
#endif
        }

        public void GetListOfHighScoresFromDataBase(int mazeId, EnumMazePlayMode mazePlayMode, string optionalPlayerName = null)
        {
#if UNITY_WEBGL
            return;
#else
            highScores.Clear();
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
                        $"WHERE mazeLevelId = {mazeId} AND mazePlayMode = {(int)mazePlayMode}";
                    if (!String.IsNullOrEmpty(optionalPlayerName))
                    {
                        command.CommandText += $" AND playerName = '{optionalPlayerName}'";                     
                    }
                    command.CommandText += "\n ORDER BY scoreAwarded DESC, timeInOneHundredsOfSeconds ASC;";

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
#endif
        }

        public void AddHighScore(MazeHighScore newHighScore)
        {
#if UNITY_WEBGL
            return;
#else
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                sqlconn.ConnectionString = getSqlitePathToDataBaseFile();
                sqlconn.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText =
                        "INSERT INTO highScores (mazeLevelId, mazePlayMode, playerName, " +
                        "dateAwarded, scoreAwarded, timeInOneHundredsOfSeconds) " +
                        "VALUES (:mazeLevelId,:mazePlayMode,:playerName,:dateAwarded,:scoreAwarded,:timeInOneHundredsOfSeconds)";
                    command.Parameters.Add("mazeLevelId", DbType.Int32).Value = newHighScore.mazeId;
                    command.Parameters.Add("mazePlayMode", DbType.Int32).Value = (int)newHighScore.mazePlayMode;
                    command.Parameters.Add("playerName", DbType.String).Value = newHighScore.playerName;
                    command.Parameters.Add("dateAwarded", DbType.DateTime).Value = newHighScore.dateAwarded;
                    command.Parameters.Add("scoreAwarded", DbType.Int32).Value = newHighScore.scoreAwarded;
                    command.Parameters.Add("timeInOneHundredsOfSeconds", DbType.Int32).Value = newHighScore.timeInOneHundredsOfSeconds;

                    command.ExecuteNonQuery();             
                }
            }
#endif
        }

        public void RemoveHighScoresForThisMaze(MazeLevel mazeLevel)
        {
#if UNITY_WEBGL
            return;
#else
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                sqlconn.ConnectionString = getSqlitePathToDataBaseFile();
                sqlconn.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText =
                        "DELETE FROM highScores " +
                        "WHERE mazeLevelId = :mazeId ;";
                    command.Parameters.Add("mazeId", DbType.String).Value = mazeLevel.mazeId;

                    command.ExecuteNonQuery();
                }
            }
#endif
        }

        public void RemoveHighScoresForThisPlayer(string name)
        {
#if UNITY_WEBGL
            return;
#else
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                sqlconn.ConnectionString = getSqlitePathToDataBaseFile();
                sqlconn.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText =
                        "DELETE FROM highScores " +
                        "WHERE playerName = :playerName ;";
                    command.Parameters.Add("playerName", DbType.String).Value = name;

                    command.ExecuteNonQuery();
                }
            }
#endif
        }

        public void UpdateHighScore(int highScoreId, MazeHighScore newHighScore)
        {
#if UNITY_WEBGL
            return;
#else
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                sqlconn.ConnectionString = getSqlitePathToDataBaseFile();
                sqlconn.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText =
                        "UPDATE highScores " +
                        "set dateAwarded = :dateAwarded, " +
                        "scoreAwarded = :scoreAwarded, " +
                        "timeInOneHundredsOfSeconds = :timeInOneHundredsOfSeconds " +                    
                        "where id = :highScoreId";
                    command.Parameters.Add("dateAwarded", DbType.DateTime).Value = newHighScore.dateAwarded;
                    command.Parameters.Add("scoreAwarded", DbType.Int32).Value = newHighScore.scoreAwarded;
                    command.Parameters.Add("timeInOneHundredsOfSeconds", DbType.Int32).Value = newHighScore.timeInOneHundredsOfSeconds;
                    command.Parameters.Add("highScoreId", DbType.Int32).Value = highScoreId;

                    command.ExecuteNonQuery();
                }
            }
#endif
        }
    }
}        

