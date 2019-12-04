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
        /// </summary>
        protected string dbPath;
        /// <summary>
        /// Поля для связи с базой данных
        /// </summary>
        protected IDbConnection dbconn;
        /// <summary>
        /// SQL команда
        /// </summary>
        protected IDbCommand dbcmd;
        /// <summary>
        /// Поле для чтения из базы данных
        /// </summary>
        protected IDataReader reader;

        /// <summary>
        /// Конструктор
        /// </summary>
        protected TableBase()
        {
            dbPath = "URI=file:" + Application.dataPath + "/DataBase/GameDB.s3db";
            dbconn = (IDbConnection)new SqliteConnection(dbPath);
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~TableBase()
        {
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
            if (dbcmd != null)
            {
                dbcmd.Dispose();
                dbcmd = null;
            }
            if (dbconn != null)
            {
                dbconn.Close();
                dbconn = null;
            }
        }
    }
}
