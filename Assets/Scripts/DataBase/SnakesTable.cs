using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Snake;

namespace Assets.Scripts.DataBase
{
    class SnakesTable : TableBase
    {
        public void AddNewSnake (SnakeBase snake)
        {
            string genes = (snake as SmartSnakeBase == null)
                ? "'null'"
                : "'" + (snake as SmartSnakeBase).GenesString + "'";

            string sqlQuery = String.Format(
                "INSERT INTO Snakes" +
                    "(name, genes, stepsCount, deathCount, stepsPerFood, maxSize, eatenFood, playedGames, mark)" +
                "VALUES" +
                    "('{0}', {1}, 0, 0, 0, 0, 0, 0, 0);",
                snake.SnakeName, genes);

            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteReader();
            dbcmd.Dispose();
            dbconn.Close();
        }

        public void UpdateStatistics (SnakeBase snake)
        {
            if (!IsExistInTable (snake.SnakeName))
                AddNewSnake (snake);

            var curSt = GetStatisticsBySnake (snake.SnakeName);
            var newStatistics = new SnakeStatisticsForTable();

            newStatistics.Name = curSt.Name;
            newStatistics.Genes = (snake is SmartSnakeBase)
                ? (snake as SmartSnakeBase).GenesString 
                : "null";
            newStatistics.StepsCount    = curSt.StepsCount + snake.Statistics.Steps;
            newStatistics.DeathCount    = curSt.DeathCount + (snake.IsAlive? 0 : 1);
            newStatistics.EatenFood     = curSt.EatenFood + snake.Statistics.EatenFood;
            newStatistics.StepsPerFood  = (int)Math.Round (
                (double)(newStatistics.StepsCount / ((newStatistics.EatenFood != 0)? newStatistics.EatenFood : 1)));
            newStatistics.MaxSize       = (curSt.MaxSize > snake.Statistics.MaxSize)? curSt.MaxSize : snake.Statistics.MaxSize;
            newStatistics.PlayedGames   = curSt.PlayedGames + 1;
            newStatistics.Mark          = (int)(Math.Round((decimal)(newStatistics.StepsPerFood / 10)));


            string sqlQuery = String.Format("UPDATE Snakes SET " +
                "name = '{0}'," +
                "genes = '{1}'," +
                "stepsCount = {2}," +
                "deathCount = {3}," +
                "stepsPerFood = {4}," +
                "maxSize = {5}," +
                "eatenFood = {6}," +
                "playedGames = {7}," +
                "mark = {8} " +
                "WHERE name = '{0}'",
                newStatistics.Name,
                newStatistics.Genes,
                newStatistics.StepsCount,
                newStatistics.DeathCount,
                newStatistics.StepsPerFood,
                newStatistics.MaxSize,
                newStatistics.EatenFood,
                newStatistics.PlayedGames,
                newStatistics.Mark);

            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteReader();
            dbcmd.Dispose();
            dbconn.Close();
        }

        public SnakeStatisticsForTable GetStatisticsBySnake (string name)
        {
            if (!IsExistInTable(name))
                return new SnakeStatisticsForTable(); // TODO: Вернуть NULL; Should return Null
            


            SnakeStatisticsForTable statistics = new SnakeStatisticsForTable();

            string sqlQuery = "SELECT * FROM Snakes WHERE name = '" + name + "'";

            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            reader.Read();

            statistics.Name         = reader.GetString(0);
            //statistics.Genes        = reader.GetString(1);
            statistics.StepsCount   = reader.GetInt32(2);
            statistics.DeathCount   = reader.GetInt32(3);
            statistics.StepsPerFood = reader.GetInt32(4);
            statistics.MaxSize      = reader.GetInt32(5);
            statistics.EatenFood    = reader.GetInt32(6);
            statistics.PlayedGames  = reader.GetInt32(7);
            //statistics.Mark         = reader.GetInt32(8);

            reader.Close();
            dbcmd.Dispose();
            dbconn.Close();

            return statistics;
        }

        /// <summary>
        /// Метод очищает таблицу
        /// Method that clears the table
        /// </summary>
        public void ClearTable()
        {
            string sqlQuery = "DELETE FROM Snakes";

            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteReader();
            dbcmd.Dispose();
            dbconn.Close();
        }

        public bool IsExistInTable (string name)
        {
            string sqlQuery = "SELECT * FROM Snakes WHERE name = '" + name + "'";

            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();

            int count = 0;
            while (reader.Read())
                count++;

            bool result = count >= 1;

            reader.Close();
            dbcmd.Dispose();
            dbconn.Close();

            return result;
        }

    }
}
