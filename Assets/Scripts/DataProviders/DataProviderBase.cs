using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.DataProviders
{
    class DataProviderBase
    {

        public bool CreateDataBaseFileIfNotExists()
        {
#if UNITY_WEBGL
            return true;
#else
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                if (!File.Exists(getDirectPathToDataBaseFile()))
                {
                    SqliteConnection.CreateFile(getDirectPathToDataBaseFile());
                    return false; // file did not exist
                }
                return true; // file did indeed exist
            }
#endif
        }

        public string SafeGetString(IDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }

        public bool QuickDataBaseIntergetyCheck()
        {
            using (SqliteConnection sqlconn = new SqliteConnection())
            {
                sqlconn.ConnectionString = getSqlitePathToDataBaseFile();
                try
                {
                    sqlconn.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }                
            }            
        }

        protected int StringToNumberProtected(string stringToConvert, string description)
        {
            int intToReturn = 0;
            try
            {
                intToReturn = Int32.Parse(stringToConvert);
            }
            catch (Exception)
            {
                Debug.Log($"For {description}, attempted to convert {stringToConvert} to an integer, but failed.  Will return 0.");
            }
            return intToReturn;
        }

        protected string getDirectPathToDataBaseFile() =>
            $"{Application.persistentDataPath}/{MazeDataBase.fileName}";

        protected string getSqlitePathToDataBaseFile() =>
            $"URI=file:{Application.persistentDataPath}/{MazeDataBase.fileName}";
    }
}
