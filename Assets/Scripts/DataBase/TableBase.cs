using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine;

namespace Assets.Scripts.DataBase
{
    public abstract class TableBase
    {
        /// <summary>
        /// Путь до базы данных
        /// Path to DataBase
        /// </summary>
        protected string dbPath;
        /// <summary>
        /// Поля для связи с базой данных
        /// Field for making a link with DataBase
        /// </summary>
        protected IDbConnection dbconn;
        /// <summary>
        /// SQL команда
        /// SQL querry
        /// </summary>
        protected IDbCommand dbcmd;
        /// <summary>
        /// Поле для чтения из базы данных
        /// Field for reading from DataBase
        /// </summary>
        protected IDataReader reader;

        /// <summary>
        /// Конструктор
        /// Constructor
        /// </summary>
        protected TableBase()
        {
            dbPath = "URI=file:" + Application.dataPath + "/DataBase/GameDB.s3db";
            dbconn = (IDbConnection)new SqliteConnection(dbPath);
        }
    }
}
