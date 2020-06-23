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
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    class SnakeStatisticsManager : MonoBehaviour
    {
        private SnakesTable table;
        private List<string> SnakeNames;
        private List<SnakeStatisticsForTable> Rows;
        private Columns OrderBy;
        private bool IsOrderByDescending;
        public GameObject RowPrefab;
        public GameObject Content;

        private void Start()
        {
            IsOrderByDescending = false;
            table = new SnakesTable();
            AssemblySnakeFactory snakeFactory = new AssemblySnakeFactory();
            SnakeNames = snakeFactory.GetAllSnakeTypes();
            Rows = new List<SnakeStatisticsForTable>();
            RowPrefab.SetActive(false);
            OrderBy = Columns.StepsPerFood;
            Sort();
            UpdateTable();
        }

        private void UpdateTable()
        {

            foreach (var name in SnakeNames)
            {
                if (table == null)
                    table = new SnakesTable();

                if (!table.IsExistInTable(name))
                    continue;

                Rows.Add(table.GetStatisticsBySnake(name)); 
            }
            Sort();
            ShowStatistics();
        }

        private void ShowStatistics()
        {
            
            while (RowPrefab.transform.parent.childCount - 1 < Rows.Count)
            {
                var tempRow = Instantiate(RowPrefab);
                tempRow.SetActive(true);
                tempRow.transform.parent = Content.transform;
            }

            for (int i = 1; i <= Rows.Count; i++)
            {
                RowPrefab.transform.parent.GetChild(i).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text
                    = Rows[i-1].Name;
                RowPrefab.transform.parent.GetChild(i).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text
                    = Rows[i - 1].StepsCount.ToString();
                RowPrefab.transform.parent.GetChild(i).transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text
                    = Rows[i - 1].DeathCount.ToString();
                RowPrefab.transform.parent.GetChild(i).transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text
                    = Rows[i - 1].StepsPerFood.ToString();
                RowPrefab.transform.parent.GetChild(i).transform.GetChild(4).GetComponent<UnityEngine.UI.Text>().text
                    = Rows[i - 1].MaxSize.ToString();
                RowPrefab.transform.parent.GetChild(i).transform.GetChild(5).GetComponent<UnityEngine.UI.Text>().text
                    = Rows[i - 1].EatenFood.ToString();
                RowPrefab.transform.parent.GetChild(i).transform.GetChild(6).GetComponent<UnityEngine.UI.Text>().text
                    = Rows[i - 1].PlayedGames.ToString();
            }
        }

        private void Sort()
        {
            switch (OrderBy)
            {
                case Columns.Name:
                    if (IsOrderByDescending)
                        Rows = Rows.OrderByDescending(x => x.Name).ToList();
                    else
                        Rows = Rows.OrderBy(x => x.Name).ToList();
                    break;

                case Columns.StepsCount:
                    if (IsOrderByDescending)
                        Rows = Rows.OrderByDescending(x => x.StepsCount).ToList();
                    else
                        Rows = Rows.OrderBy(x => x.StepsCount).ToList();
                    break;

                case Columns.DeathCount:
                    if (IsOrderByDescending)
                        Rows = Rows.OrderByDescending(x => x.DeathCount).ToList();
                    else
                        Rows = Rows.OrderBy(x => x.DeathCount).ToList();
                    break;

                case Columns.StepsPerFood:
                    if (IsOrderByDescending)
                        Rows = Rows.OrderByDescending(x => x.StepsPerFood).ToList();
                    else
                        Rows = Rows.OrderBy(x => x.StepsPerFood).ToList();
                    break;

                case Columns.MaxSize:
                    if (IsOrderByDescending)
                        Rows = Rows.OrderByDescending(x => x.MaxSize).ToList();
                    else
                        Rows = Rows.OrderBy(x => x.MaxSize).ToList();
                    break;

                case Columns.EatenFood:
                    if (IsOrderByDescending)
                        Rows = Rows.OrderByDescending(x => x.EatenFood).ToList();
                    else
                        Rows = Rows.OrderBy(x => x.EatenFood).ToList();
                    break;

                case Columns.PlayedGames:
                    if (IsOrderByDescending)
                        Rows = Rows.OrderByDescending(x => x.PlayedGames).ToList();
                    else
                        Rows = Rows.OrderBy(x => x.PlayedGames).ToList();
                    break;


            }
            ShowStatistics();
        }

        public void SortByName()
        {
            if (OrderBy == Columns.Name)
                IsOrderByDescending = !IsOrderByDescending;
            else
            {
                IsOrderByDescending = false;
                OrderBy = Columns.Name;
            }

            Sort();
        }

        public void SortByStepsCount()
        {
            if (OrderBy == Columns.StepsCount)
                IsOrderByDescending = !IsOrderByDescending;
            else
            {
                IsOrderByDescending = false;
                OrderBy = Columns.StepsCount;
            }

            Sort();
        }

        public void SortByDeathCount()
        {
            if (OrderBy == Columns.DeathCount)
                IsOrderByDescending = !IsOrderByDescending;
            else
            {
                IsOrderByDescending = false;
                OrderBy = Columns.DeathCount;
            }

            Sort();
        }

        public void SortByStepsPerFood()
        {
            if (OrderBy == Columns.StepsPerFood)
                IsOrderByDescending = !IsOrderByDescending;
            else
            {
                IsOrderByDescending = false;
                OrderBy = Columns.StepsPerFood;
            }

            Sort();
        }

        public void SortByMaxSize()
        {
            if (OrderBy == Columns.MaxSize)
                IsOrderByDescending = !IsOrderByDescending;
            else
            {
                IsOrderByDescending = false;
                OrderBy = Columns.MaxSize;
            }

            Sort();
        }

        public void SortByEatenFood()
        {
            if (OrderBy == Columns.EatenFood)
                IsOrderByDescending = !IsOrderByDescending;
            else
            {
                IsOrderByDescending = false;
                OrderBy = Columns.EatenFood;
            }

            Sort();
        }

        public void SortByPlayedGames()
        {
            if (OrderBy == Columns.PlayedGames)
                IsOrderByDescending = !IsOrderByDescending;
            else
            {
                IsOrderByDescending = false;
                OrderBy = Columns.PlayedGames;
            }

            Sort();
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
            // Reload scene
            SceneManager.LoadScene(5);
        }

        private enum Columns
        {
            Name,
            StepsCount,
            DeathCount,
            StepsPerFood,
            MaxSize,
            EatenFood,
            PlayedGames
        }
    }
}
