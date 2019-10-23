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
            MaxLength = 0;
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
                return Length;
            }

            set
            {
                if (value > MaxLength)
                    MaxLength = value;
                Length = value;
            }
        }
        /// <summary>
        /// Максимальная длинна
        /// </summary>
        public int MaxLength { get; private set; }

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
