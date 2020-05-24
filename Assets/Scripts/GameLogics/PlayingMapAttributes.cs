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
        /// Class containing food information
        /// </summary>
        public sealed class Food
        {
            /// <summary>
            /// Максимальное количество еды на поле
            /// Max amount of food in the map
            /// </summary>
            public int MaxCount;
            /// <summary>
            /// Кординаты еды на поле
            /// Food coordinates in the map
            /// </summary>
            public List<SnakeAttribute.Cordinates> FoodCordinates;
        }

        /// <summary>
        /// Змейки для отображения на карте
        /// Snakes for displaying in the map
        /// </summary>
        public sealed class Snake
        {
            /// <summary>
            /// Стандартный конструктор
            /// Standart constructor
            /// </summary>
            /// <param name="name">Имя змейки/Snake's name</param>
            /// <param name="cordinates">Кординаты тела змейки/Snake's body coordinates</param>
            /// <param name="isAlive">Жива ли змейка/Is snake alive</param>
            public Snake (string name, List<SnakeAttribute.Cordinates> cordinates, SnakeBase snake, SnakeStatistics snakeStatistics)
            {
                Name = name;
                Cordinates = cordinates;
                SnakeB = snake;
                SnakeStatistics = snakeStatistics; 
            }

            /// <summary>
            /// Имя змейки
            /// Snake's name
            /// </summary>
            public string Name;
            /// <summary>
            /// Кординаты змейки
            /// Snake's coordinates
            /// </summary>
            public List<SnakeAttribute.Cordinates> Cordinates;
            /// <summary>
            /// Жива ли змейка
            /// Is snake alive
            /// </summary>
            public bool IsAlive { get => SnakeB.IsAlive; set { SnakeB.IsAlive = value; } }
            /// <summary>
            /// Статистика змеек
            /// Snakes' statistics
            /// </summary>
            public SnakeStatistics SnakeStatistics;
            /// <summary>
            /// Ссылка на змейку
            /// Snake base reference
            /// </summary>
            public SnakeBase SnakeB { get; }

            /// <summary>
            /// Проверка равенства змеек
            /// Snakes' equality check
            /// </summary>
            /// <param name="snake1">Змейка/Snake</param>
            /// <param name="snake2">Змейка/Snake</param>
            /// <returns>True если равны/Returns true if 2 snakes are equal</returns>
            public static bool operator == (Snake snake1, Snake snake2)
            {
                if (snake1.Name == snake2.Name && snake1.IsAlive == snake2.IsAlive
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
            /// Inequality check
            /// </summary>
            /// <param name="snake1">Змейка/Snake</param>
            /// <param name="snake2">Змейка/Snake</param>
            /// <returns>True если не равны/Returns true if 2 snake are inequal</returns>
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
