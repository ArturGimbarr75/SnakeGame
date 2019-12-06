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
        /// </summary>
        /// <param name="snakeLength">Изначальная длинна змейки</param>
        public SnakeStatistics(int snakeLength)
        {
            Steps = 0;
            EatenFood = 0;
            MaxSize = 0;
            _length = 0;
            Length = snakeLength;

        }
        /// <summary>
        /// Общее количество шагов
        /// </summary>
        public int Steps { get; set; }
        /// <summary>
        /// Общее количество съеденной еды
        /// </summary>
        public int EatenFood { get; set; }
        /// <summary>
        /// Текущая длинна змейки
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
        /// </summary>
        private int _length;
        /// <summary>
        /// Максимальная длинна
        /// </summary>
        public int MaxSize { get; private set; }

        public override string ToString()
        {
            return string.Format("Steps {0}, Eaten food {1}, Max size {2}, Size {3}", Steps, EatenFood, MaxSize, Length);
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
