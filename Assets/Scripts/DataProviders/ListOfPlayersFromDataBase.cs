using Assets.Scripts.DataObjects;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using Assets.Scripts.Enums;
using System;
using System.IO;

namespace Assets.Scripts.DataProviders
{
    class ListOfPlayersFromDataBase : DataProviderBase
    {
        public List<String> players;        

        public ListOfPlayersFromDataBase()           
        {            
            players = new List<String>();
        }

        //TODO Add RemovePlayer(name)

        public void CreateTableForListOfPlayersIfNotExists()
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
                        "CREATE TABLE IF NOT EXISTS players (name TEXT UNIQUE ON CONFLICT IGNORE)";

                    IDataReader reader = command.ExecuteReader();
                    reader.Close();
                }
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "INSERT INTO players (name) VALUES (@name)";
                    command.Parameters.Add("@name", DbType.String).Value = "Guest";
                    command.ExecuteNonQuery();                 
                }
            }  
#endif
        }

        public void GetListOfPlayersFromDataBase()
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
                        "SELECT name \n" +
                        "FROM players \n" +
                        "ORDER BY name ASC;";

                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        players.Add(SafeGetString(reader, 0));                            
                    }

                    reader.Close();
                }
            }
#endif
        }

        public void AddPlayer(string name)           
        {
#if UNITY_WEBGL
            players.Add(name);
            return;
#else
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                sqlconn.ConnectionString = getSqlitePathToDataBaseFile();

                sqlconn.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "INSERT INTO players (name) VALUES (:name) ;";                    
                    command.Parameters.Add("name", DbType.String).Value = name;
                    command.ExecuteNonQuery();
                }
            } 
#endif
        }

        public void RemovePlayer(string name)
        {
#if UNITY_WEBGL
            players.Remove(name);
            return;
#else
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                sqlconn.ConnectionString = getSqlitePathToDataBaseFile();

                sqlconn.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "DELETE FROM players WHERE name = :playerName ;";
                    command.Parameters.Add("playerName", DbType.String).Value = name;
                    command.ExecuteNonQuery();
                }
            }
#endif
        }
    }        
}
