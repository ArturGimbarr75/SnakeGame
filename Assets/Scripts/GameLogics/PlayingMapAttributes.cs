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

            /// <summary>
            /// Проверка равенства змеек
            /// </summary>
            /// <param name="snake1">Змейка</param>
            /// <param name="snake2">Змейка</param>
            /// <returns>True если равны</returns>
            public static bool operator == (Snake snake1, Snake snake2)
            {
                if (snake1.Name == snake2.Name && snake1.isAlive == snake2.isAlive
                    && snake1.Cordinates.Count == snake2.Cordinates.Count)
                {
                    for (int i = 0; i < snake1.Cordinates.Count; i++)
                        if (snake1.Cordinates[i] != snake2.Cordinates[i])
                            return false;
                }
                else
                    return false;
                return true;
            }

            /// <summary>
            /// Проверка неравенства змеек
            /// </summary>
            /// <param name="snake1">Змейка</param>
            /// <param name="snake2">Змейка</param>
            /// <returns>True если не равны</returns>
            public static bool operator != (Snake snake1, Snake snake2)
            {
                return !(snake1 == snake2);
            }
        }

        enum PlayingMapCell
        {
            Food,
            Barrier,
            Empty,
            SnakeHead,
            SnakeBody,
            SnakeTail
        }
    }
}
