using Assets.Scripts.GameLogics;
using Assets.Scripts.Menu.Attributes;
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
    class AfterGameStatisticsManager : MonoBehaviour
    {
        public GameObject RowPrefab;
        public GameObject Content;
        private List<GameObject> Rows;

        private void Start()
        {
            AssemblySnakeFactory snakeFactory = new AssemblySnakeFactory();
            Rows = new List<GameObject>();
            RowPrefab.SetActive(false);
            UpdateTable();
        }

        private void UpdateTable()
        {
            foreach (var row in Rows)
                Destroy(row);

            for (int i = 0; i < GameInits.SnakeStatistics.Count; i++)
            {
                var tempRow = Instantiate(RowPrefab);
                tempRow.SetActive(true);
                tempRow.transform.parent = Content.transform;
                tempRow.transform.name = name;
                tempRow.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = (i+1).ToString();
                tempRow.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = GameInits.SnakeStatistics[i].Name;
                tempRow.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = GameInits.SnakeStatistics[i].Steps.ToString();
                tempRow.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = "Unknown info";
                tempRow.transform.GetChild(4).GetComponent<UnityEngine.UI.Text>().text = GameInits.SnakeStatistics[i].MaxSize.ToString();
                tempRow.transform.GetChild(5).GetComponent<UnityEngine.UI.Text>().text = GameInits.SnakeStatistics[i].EatenFood.ToString();

                Rows.Add(tempRow);
            }

        }

        public void Back()
        {
            SceneManager.LoadScene(0);
        }
    }
}
