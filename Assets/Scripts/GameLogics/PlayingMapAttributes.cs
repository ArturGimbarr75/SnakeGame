using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Snake;

namespace Map
{
    public class PlayingMapAttributes
    {
        /// <summary>
        /// Класс хранящий информацию о еде
        /// </summary>
        public struct Food
        {
            public int MaxCount;
            public List<SnakeAttribute.Cordinates> FoodCordinates;
        }

        /// <summary>
        /// Змейки для отображения на карте
        /// </summary>
        public struct Snake
        {
            public Snake (string name, List<SnakeAttribute.Cordinates> cordinates, bool isAlive)
            {
                Name = name;
                Cordinates = cordinates;
                this.isAlive = isAlive;
            }

            public string Name;
            public List<SnakeAttribute.Cordinates> Cordinates;
            public bool isAlive;
        }
    }
}
