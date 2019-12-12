using Assets.Scripts.DataBase;
using Assets.Scripts.GameLogics;
using Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu
{
    class SnakeStatisticsManager : MonoBehaviour
    {
        private SnakesTable table = new SnakesTable();
        private List<string> SnakeNames;
        private List<GameObject> Rows;
        public GameObject RowPrefab;
        public GameObject Content;

        private void Start()
        {
            AssemblySnakeFactory snakeFactory = new AssemblySnakeFactory();
            SnakeNames = snakeFactory.GetAllSnakeTypes();
            Rows = new List<GameObject>();
            RowPrefab.SetActive(false);
            UpdateTable();
        }

        private void UpdateTable()
        {
            foreach (var row in Rows)
                Destroy(row);

            foreach (var name in SnakeNames)
            {
                if (table == null)
                    table = new SnakesTable();

                if (!table.IsExistInTable(name))
                    continue;

                var tempStatistics = table.GetStatisticsBySnake(name);
                var tempRow = Instantiate(RowPrefab);
                tempRow.SetActive(true);
                tempRow.transform.parent = Content.transform;
                tempRow.transform.name = name;
                tempRow.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = tempStatistics.Name;
                tempRow.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = tempStatistics.Genes;
                tempRow.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = tempStatistics.StepsCount.ToString();
                tempRow.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = tempStatistics.DeathCount.ToString();
                tempRow.transform.GetChild(4).GetComponent<UnityEngine.UI.Text>().text = tempStatistics.StepsPerFood.ToString();
                tempRow.transform.GetChild(5).GetComponent<UnityEngine.UI.Text>().text = tempStatistics.MaxSize.ToString();
                tempRow.transform.GetChild(6).GetComponent<UnityEngine.UI.Text>().text = tempStatistics.EatenFood.ToString();
                tempRow.transform.GetChild(7).GetComponent<UnityEngine.UI.Text>().text = tempStatistics.PlayedGames.ToString();
                tempRow.transform.GetChild(8).GetComponent<UnityEngine.UI.Text>().text = tempStatistics.Mark.ToString();
                Rows.Add(tempRow);
            }
            
        }

        public void Back()
        {
            SceneManager.LoadScene(0);
        }

        public void ClearData()
        {
            if (table == null)
                table = new SnakesTable();

            table.ClearTable();
            UpdateTable();
        }
    }
}
