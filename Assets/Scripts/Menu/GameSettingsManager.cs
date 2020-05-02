using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Tilemaps;

using Assets.Scripts.GameLogics;
using Assets.Scripts.Menu.Attributes;
using Logic;
using Map;

namespace Assets.Scripts.Menu
{
    public class GameSettingsManager : MonoBehaviour
    {
        public Toggle LeftDeadBody;
        public Dropdown GameMode;
        public InputField FoodCount;
        public InputField MapSize;
        public List<Toggle> EndGameCheckboxes;
        public GameObject SnakesScrollView;
        public GameObject SnakeButtonRowPrefab;
        public GameObject AddedSnakesScrollView;
        public GameObject AddedSnakeRowPrefab;

        private Dictionary<string, Sprite> SnakeHeadSprites;
        private HashSet<GameLogicsAttributes.GameoverPredicates> Predicates =
            new HashSet<GameLogicsAttributes.GameoverPredicates>(GameInits.GameoverPredicates);

        private void Start()
        {
            SetUpElementsValue();
            InitialiseSnakeHeadSprites();
            SetUpSnakeButtons(); 
        }


        #region SetUps

        private void InitialiseSnakeHeadSprites()
        {
            SnakeHeadSprites = new Dictionary<string, Sprite>();
            AssemblySnakeFactory factory = new AssemblySnakeFactory();
            foreach (string name in factory.GetAllSnakeTypes())
            {
                string path = String.Format("Assets\\IMG\\SnakeSprites\\Simple\\{0}\\{0}_1.asset", name);
                TileBase tile = (TileBase)AssetDatabase.LoadAssetAtPath(path, typeof(TileBase));
                TileData data = new TileData();
                tile.GetTileData(new Vector3Int(), null, ref data);
                SnakeHeadSprites.Add(name, data.sprite);
            }
        }

        private void SetUpSnakeButtons()
        {
            AssemblySnakeFactory factory = new AssemblySnakeFactory();
            SnakeButtonRowPrefab.SetActive(false);

            foreach (string name in factory.GetAllSnakeTypes())
            {
                var tempRow = Instantiate(SnakeButtonRowPrefab);
                tempRow.SetActive(true);
                Button tempButton = tempRow.transform.GetChild(0).gameObject.GetComponent<Button>();
                tempButton.image.sprite = SnakeHeadSprites[name];
                tempButton.onClick.AddListener(() => OnButtonSnakePressed(name));
                tempButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = name;
                tempRow.transform.parent = SnakesScrollView.transform;
            }
        }

        /// <summary>
        /// Установка предыдущих значений
        /// </summary>
        private void SetUpElementsValue()
        {
            // установка значения закрепления трупов змеек
            LeftDeadBody.isOn = GameInits.LeftDeadSnakeBody;

            // установка значений предикатов окончания игры
            GameInits.GameoverPredicates.Clear();
            foreach (var checkbox in EndGameCheckboxes)
                checkbox.isOn = false;

            foreach (var predicate in Predicates)
                    switch (predicate)
                    {
                        case Logic.GameLogicsAttributes.GameoverPredicates.Achieved30Cels:
                            SetPredicate ("Toggle: Achieved 30 cels");
                            break;

                        case Logic.GameLogicsAttributes.GameoverPredicates.DeadAllPlayers:
                            SetPredicate("Toggle: Dead all players");
                            break;

                        case Logic.GameLogicsAttributes.GameoverPredicates.DeadAllSnakes:
                            SetPredicate("Toggle: Dead all snakes");
                            break;

                        case Logic.GameLogicsAttributes.GameoverPredicates.LeftOneAliveSnake:
                            SetPredicate("Toggle: Left one alive snake");
                            break;
                    }

            // Установка размера карты
            MapSize.text = GameInits.MapSize.ToString();

            // Установка количества еды
            FoodCount.text = GameInits.FoodCount.ToString();
        }

        #endregion

        /// <summary>
        /// Добавляет имена змеек для игры
        /// Adds snakes' names for a game
        /// </summary>
        private void OnButtonSnakePressed(string name)
        {
            GameInits.SnakeNames.Add(name);

            var tempRow = Instantiate(AddedSnakeRowPrefab);
            tempRow.SetActive(true);

            if (AddedSnakesScrollView.transform.childCount % 2 == 0)
            {
                var color = tempRow.GetComponent<Image>().color;
                color.a = 30.0f / 255.0f;
                tempRow.GetComponent<Image>().color = color;
            }
  
            tempRow.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = SnakeHeadSprites[name];

            tempRow.transform.GetChild(1).gameObject.GetComponent<Text>().text =
                new Func<string>(() =>
                {
                    string line = String.Empty;
                    foreach (var c in name)
                    {
                        if (Regex.Replace(c.ToString(), "[A-Z]", String.Empty) == String.Empty)
                            line += " " + c;
                        else
                            line += c;
                    }
                    return line;
                }).Invoke().Trim();

            var tempButton = tempRow.transform.GetChild(2).gameObject.GetComponent<Button>();
            tempButton.onClick.AddListener(() => OnButtonRemoveSnakePresed(tempRow, name));
            tempRow.transform.parent = AddedSnakesScrollView.transform;

        }

        private void OnButtonRemoveSnakePresed(GameObject o, string name)
        {
            GameObject.Destroy(o);
            GameInits.SnakeNames.Remove(name);
        }

        /// <summary>
        /// Установка предикатов окончания игры
        /// Setting game ending predicates
        /// </summary>
        public void OnEndGamePredicateChanged()
        {
            GameInits.GameoverPredicates.Clear();
            foreach (var checkbox in EndGameCheckboxes)
                if (checkbox.isOn)
                    switch (checkbox.name)
                    {
                        case "Toggle: Dead all players":
                            GameInits.GameoverPredicates.Add(Logic.GameLogicsAttributes.GameoverPredicates.DeadAllPlayers);
                            break;

                        case "Toggle: Dead all snakes":
                            GameInits.GameoverPredicates.Add(Logic.GameLogicsAttributes.GameoverPredicates.DeadAllSnakes);
                            break;

                        case "Toggle: Left one alive snake":
                            GameInits.GameoverPredicates.Add(Logic.GameLogicsAttributes.GameoverPredicates.LeftOneAliveSnake);
                            break;

                        case "Toggle: Achieved 30 cels":
                            GameInits.GameoverPredicates.Add(Logic.GameLogicsAttributes.GameoverPredicates.Achieved30Cels);
                            break;
                    }
        }

        /// <summary>
        /// Установка корректного значения размеров карты
        /// Setting correct map size
        /// </summary>
        /// <param name="numStr">Значение/Value</param> 
        public void OnMapSizeChanged()
        {
            int num = int.Parse(MapSize.text);

            if (num < PlayingMap.MinSize)
                num = PlayingMap.MinSize;
            if (num > PlayingMap.MaxSize)
                num = PlayingMap.MaxSize;
            if (num % 2 != 0)
                num++;

            MapSize.text = num.ToString();
            GameInits.MapSize = num;
        }

        /// <summary>
        /// Установка корректного значения количества еды
        /// Setting correct food amount
        /// </summary>
        /// <param name="numStr">Значение/Value</param>
        public void OnFoodCountChanged()
        {
            int num = int.Parse(FoodCount.text);

            int minCount = 1, maxCount = GameInits.MapSize * GameInits.MapSize / 4;

            if (num < minCount)
                num = minCount;
            if (num > maxCount)
                num = maxCount;

            FoodCount.text = num.ToString();
            GameInits.FoodCount = num;
        }

        /// <summary>
        /// Установка значения чекбокса
        /// Setting checkbox value
        /// </summary>
        /// <param name="predicate">Название предиката/Predicate's name</param>
        /// <param name="isOn">Значение/Value</param>
        private void SetPredicate(string predicate, bool isOn = true)
        {
            foreach (var checkbox in EndGameCheckboxes)
                if (predicate == checkbox.name)
                    checkbox.isOn = isOn;
        }

        #region Buttons

        /// <summary>
        /// Переход к игре
        /// </summary>
        public void Next()
        {
            SceneManager.LoadScene(2);
        }

        /// <summary>
        /// Переход к главному меню
        /// </summary>
        public void Back()
        {
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Меняем инициализационную переменную
        /// </summary>
        public void OnLeftDeadBodyValueChanged()
        {
            GameInits.LeftDeadSnakeBody = LeftDeadBody.isOn;
        }

        #endregion
    }
}
