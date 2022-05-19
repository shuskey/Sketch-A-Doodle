using Assets.Scripts.DataObjects;
using System.Collections.Generic;
using System.Linq;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using Assets.Scripts.Enums;
using System;

namespace Assets.Scripts.DataProviders
{
    class ListOfMazesFromDataBase : DataProviderBase
    {

        public ListOfMazesFromDataBase()           
        {            
            MazeList.Mazes = new List<MazeLevel>();
        }

        // TODO: Create a RemoveMaze

        public void CreateTableForListOfMazesIfNotExists()
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
                        "CREATE TABLE IF NOT EXISTS mazeLevels " +
                        "(id INTEGER PRIMARY KEY ON CONFLICT ABORT AUTOINCREMENT," +
                        "title                      TEXT, " +
                        "creator                    TEXT, " +
                        "mazeTextureFileName        TEXT, " +
                        "invertToUseBlackLines      INTEGER, " +
                        "startPositionRatioX        REAL, " +
                        "startPositionRatioY        REAL, " +
                        "endPositionRatioX          REAL, " +
                        "endPositionRatioY          REAL, " +
                        "createdDate                DATETIME, " +
                        "numberOfPlayThroughs       INTEGER);";                    
                    IDataReader reader = command.ExecuteReader();
                    reader.Close();
                }
            }
#endif
        }

        public void GetListOfMazesFromDataBase()
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
                        "SELECT id, title, creator, mazeTextureFileName, invertToUseBlackLines, " +
                        "startPositionRatioX, startPositionRatioY, " +
                        "endPositionRatioX, endPositionRatioY, " +
                        "createdDate, numberOfPlayThroughs \n" +
                        "FROM mazeLevels \n" +
                        "ORDER BY createdDate DESC;";
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var nextMaze = new MazeLevel(
                            mazeId: reader.GetInt32(0),
                            title: SafeGetString(reader, 1),
                            creator: SafeGetString(reader, 2),
                            mazeTextureFileName: SafeGetString(reader, 3),
                            invertToUseBlackLines: reader.GetInt32(4) == 1,
                            startPositionRatio: new UnityEngine.Vector2((float)reader.GetDouble(5), (float)reader.GetDouble(6)),
                            endPositionRatio: new UnityEngine.Vector2((float)reader.GetDouble(7), (float)reader.GetDouble(8)),
                            createdDate: reader.GetDateTime(9),
                            numberOfPlayThroughs: reader.GetInt32(10)
                            );
                        MazeList.Mazes.Add(nextMaze);
                    }
                    reader.Close();
                }
            }
#endif
        }

        public void RemoveMaze(MazeLevel mazeLevel)
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
                        "DELETE FROM mazeLevels " +
                        "WHERE id = :mazeId;";                        
                    command.Parameters.Add("mazeId", DbType.String).Value = mazeLevel.mazeId;                    

                    command.ExecuteNonQuery();
                }
            }
#endif
        }


        public void AddMaze(MazeLevel newMazeLevel)
        {
#if UNITY_WEBGL
            MazeList.Mazes.Add(newMazeLevel);  // We are running without a database, so lets just add this to the running 'in memory' list
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
#endif
        }

        public void UpdateMaze(int mazeId, MazeLevel updateMazeLevel)
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
                        "update mazeLevels " +
                        "set title = :title, " +
                        "creator = :creator, " +
                        "invertToUseBlackLines = :invertToUseBlackLines, " +
                        "startPositionRatioX = :startPositionRatioX, " +
                        "startPositionRatioY = :startPositionRatioY, " +
                        "endPositionRatioX = :endPositionRatioX, " +
                        "endPositionRatioY = :endPositionRatioY " +
                        "where id=:mazeId";
                    command.Parameters.Add("title", DbType.String).Value = updateMazeLevel.title;
                    command.Parameters.Add("creator", DbType.String).Value = updateMazeLevel.creator;
                    command.Parameters.Add("invertToUseBlackLines", DbType.Int32).Value = updateMazeLevel.invertToUseBlackLines ? 1 : 0;
                    command.Parameters.Add("startPositionRatioX", DbType.Double).Value = updateMazeLevel.startPositionRatio.x;
                    command.Parameters.Add("startPositionRatioY", DbType.Double).Value = updateMazeLevel.startPositionRatio.y;
                    command.Parameters.Add("endPositionRatioX", DbType.Double).Value = updateMazeLevel.endPositionRatio.x;
                    command.Parameters.Add("endPositionRatioY", DbType.Double).Value = updateMazeLevel.endPositionRatio.y;
                    command.Parameters.Add("mazeId", DbType.Int32).Value = mazeId;

                    command.ExecuteNonQuery();
                }
            }
#endif
        }
    }
}
