using Assets.Scripts.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu
{
    class PlayerStatisticsManager : MonoBehaviour
    {
        private PlayerTable table = new PlayerTable();
        private List<string> SnakeNames;
        private List<GameObject> Rows;
        public GameObject RowPrefab;
        public GameObject Content;

        private void Start()
        {
            Rows = new List<GameObject>();
            RowPrefab.SetActive(false);
            UpdateTable();
        }

        private void UpdateTable()
        {
            foreach (var row in Rows)
                Destroy(row);

            if (table == null)
                table = new PlayerTable();
            table.UpdatePlayerInfo();
            var statistics = table.GameInfo;
            foreach (var element in statistics)
            {
                var tempRow = Instantiate(RowPrefab);
                tempRow.SetActive(true);
                tempRow.transform.parent = Content.transform;
                tempRow.transform.name = name;
                tempRow.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = element.ID.ToString();
                tempRow.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = element.Date.ToString();
                tempRow.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = element.PlayerType;
                tempRow.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = element.GameType;
                tempRow.transform.GetChild(4).GetComponent<UnityEngine.UI.Text>().text = element.Score.ToString();
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
                table = new PlayerTable();

            table.ClearTable();
            UpdateTable();
        }
    }
}
