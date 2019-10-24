using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Snake;

namespace Map
{
    public sealed class PlayingMapAttributes
    {
        /// <summary>
        /// Класс хранящий информацию о еде
        /// </summary>
        public sealed class Food
        {
            /// <summary>
            /// Максимальное количество еды на поле
            /// </summary>
            public int MaxCount;
            /// <summary>
            /// Кординаты еды на поле
            /// </summary>
            public List<SnakeAttribute.Cordinates> FoodCordinates;
        }

        /// <summary>
        /// Змейки для отображения на карте
        /// </summary>
        public sealed class Snake
        {
            /// <summary>
            /// Стандартный конструктор
            /// </summary>
            /// <param name="name">Имя змейки</param>
            /// <param name="cordinates">Кординаты тела змейки</param>
            /// <param name="isAlive">Жива ли змейка</param>
            public Snake (string name, List<SnakeAttribute.Cordinates> cordinates, bool isAlive)
            {
                Name = name;
                Cordinates = cordinates;
                this.isAlive = isAlive;
            }

            /// <summary>
            /// Имя змейки
            /// </summary>
            public string Name;
            /// <summary>
            /// Кординаты змейки
            /// </summary>
            public List<SnakeAttribute.Cordinates> Cordinates;
            /// <summary>
            /// Жива ли змейка
            /// </summary>
            public bool isAlive;
        }
    }
}
