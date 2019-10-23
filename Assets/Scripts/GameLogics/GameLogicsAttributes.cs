using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Snake;

namespace Logic
{
    public class GameLogicsAttributes
    {
        /// <summary>
        /// Класс змейки для использования в логике игры
        /// </summary>
        public sealed class SnakesForLogic // TODO: запись предыдущего хода, длина...
        {
            public List<SnakeBase> Snakes = new List<SnakeBase>();
        }
    }
}
