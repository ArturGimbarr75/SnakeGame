using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DataBase
{
    class PlayerTable : TableBase
    {
        public List<PlayerGameInfo> GameInfo;

        public void AddNewRow (string player, string gameType, int score)
        {
            string sqlQuery = String.Format(
                "INSERT INTO Player (date, playerType, gameType, score)"
                + "VALUES(datetime('now', 'localtime'), '{0}', '{1}', {2});",
                player, gameType, score);

            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteReader();
            dbcmd.Dispose();
            dbconn.Close();
        }

        public void UpdatePlayerInfo()
        {
            GameInfo = new List<PlayerGameInfo>();
            string sqlQuery = "SELECT id, date, playerType, gameType, score FROM Player";

            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {
                var row = new PlayerGameInfo();
                row.ID = reader.GetInt32(0);
                row.Date = reader.GetDateTime(1);
                row.PlayerType = reader.GetString(2);
                row.GameType = reader.GetString(3);
                row.Score = reader.GetInt32(4);
                GameInfo.Add(row);
            }

            reader.Close();
            dbcmd.Dispose();
            dbconn.Close();
        }
    }

    struct PlayerGameInfo
    {
        public int ID;
        public DateTime Date;
        public string PlayerType;
        public string GameType;
        public int Score;
    }
}
