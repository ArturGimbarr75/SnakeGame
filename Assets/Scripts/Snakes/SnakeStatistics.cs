using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    /// <summary>
    /// Статистика змеек
    /// </summary>
    public struct SnakeStatistics
    {

        /// <summary>
        /// Конструктор класса статистики
        /// Statistics constructor
        /// </summary>
        /// <param name="snakeLength">Изначальная длинна змейки/Initial snake length</param>
        public SnakeStatistics(string name)
        {
            Name = name;
            Steps = 0;
            EatenFood = 0;
            MaxSize = 0;
            _length = 0;
            Length = 0;
        }

        /// <summary>
        /// Копи конструктор
        /// Copy constructor
        /// </summary>
        /// <param name="statistics">Оригинал/Original</param>
        public SnakeStatistics (SnakeStatistics statistics)
        {
            Name = statistics.Name;
            Steps = statistics.Steps;
            EatenFood = statistics.EatenFood;
            MaxSize = statistics.MaxSize;
            _length = statistics.Length;
        }

        /// <summary>
        /// Имя змейки
        /// Snake's name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Общее количество шагов
        /// General amount of steps
        /// </summary>
        public int Steps { get; set; }
        /// <summary>
        /// Общее количество съеденной еды
        /// General amount of eaten food
        /// </summary>
        public int EatenFood { get; set; }
        /// <summary>
        /// Текущая длинна змейки
        /// Current snake's length
        /// </summary>
        public int Length
        {
            get
            {
                return _length;
            }

            set
            {
                _length = value;
                if (MaxSize < value)
                MaxSize = value;
            }
        }
        /// <summary>
        /// Текущая длинна змейки
        /// Current snake's length
        /// </summary>
        private int _length;
        /// <summary>
        /// Максимальная длинна
        /// Max snake's length
        /// </summary>
        public int MaxSize { get; private set; }

        public override string ToString()
        {
            return string.Format("{4} -> Steps {0}, Eaten food {1}, Max size {2}, Size {3}",
                Steps, EatenFood, MaxSize, Length, Name);
        }

    }
}

/*  TODO:
 * длинна X
 * количество шагов X
 * количество съеденной еды X
 * среднее количество шагов на одну еду ---------------------------
 * количество смертей ----------
 * максимальная длина X
 * среднее количество шагов до смерти --------------
 * общее количество шагов -----------------------
 */
