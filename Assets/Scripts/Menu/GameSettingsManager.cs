using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Assets.Scripts.GameLogics;
using Assets.Scripts.Menu.Attributes;
using Logic;
using Map;

namespace Assets.Scripts.Menu
{
    public class GameSettingsManager : MonoBehaviour
    {
        public List<Dropdown> SnakeTypesDropdowns;
        public Toggle LeftDeadBody;
        public Dropdown GameMode;
        public InputField FoodCount;
        public InputField MapSize;
        public List<Toggle> EndGameCheckboxes;

        private List<string> SnakeNames;
        private const string NoneStr = "None";
        private List<string> names = new List<string>(GameInits.SnakeNames);
        private HashSet<GameLogicsAttributes.GameoverPredicates> Predicates =
            new HashSet<GameLogicsAttributes.GameoverPredicates>(GameInits.GameoverPredicates);

        private void Start()
        {
            SetUpSnakeTypesDropdowns();
            SetUpElementsValue();
        }


        #region SetUps

        /// <summary>
        /// Установка предыдущих значений
        /// </summary>
        private void SetUpElementsValue()
        {
            // установка значений дропдаун менюшек
            for (int i = 0; i < names.Count; i++)
            {
                for (int j = 0; j < SnakeNames.Count; j++)
                    if (SnakeNames[j] == names[i])
                    {
                        SnakeTypesDropdowns[i].value = j;
                        break;
                    }
            }

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

        /// <summary>
        /// Устанавливаем значения дропдаунов
        /// </summary>
        private void SetUpSnakeTypesDropdowns()
        {
            AssemblySnakeFactory factory = new AssemblySnakeFactory();
            SnakeNames = factory.GetAllSnakeTypes();
            SnakeNames.Insert(0, NoneStr);
            foreach (var dropdown in SnakeTypesDropdowns)
            {
                dropdown.ClearOptions();
                dropdown.AddOptions(SnakeNames);
                dropdown.onValueChanged.AddListener(delegate {
                    OnDropdownsValueChanged();});
            }
            // Устанавливаем значение игрока
            for (int i = 0; i < SnakeNames.Count; i++)
                if (SnakeNames[i] == nameof(PlayerArrows))
                {
                    SnakeTypesDropdowns[0].value = i;
                    break;
                }
        }

        #endregion

        /// <summary>
        /// Добавляет имена змеек для игры
        /// </summary>
        public void OnDropdownsValueChanged()
        {
            GameInits.SnakeNames.Clear();
            foreach (var dropdown in SnakeTypesDropdowns)
                if (SnakeNames[dropdown.value] != NoneStr)
                    GameInits.SnakeNames.Add (SnakeNames[dropdown.value]);
        }

        /// <summary>
        /// Установка предикатов окончания игры
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
        /// </summary>
        /// <param name="numStr">Значение</param> 
        public void OnMapSizeChanged()
        {
            int num = int.Parse(MapSize.text);

            if (num < PlayingMap.MinSize)
                num = PlayingMap.MinSize;
            if (num > PlayingMap.MaxSize)
                num = PlayingMap.MaxSize;

            MapSize.text = num.ToString();
            GameInits.MapSize = num;
        }

        /// <summary>
        /// Установка корректного значения количества еды
        /// </summary>
        /// <param name="numStr">Значение</param>
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
        /// </summary>
        /// <param name="predicate">Название предиката</param>
        /// <param name="isOn">Значение</param>
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
