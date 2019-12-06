using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Snake;

namespace Logic
{
    public class GameLogicsAttributes
    {
        // TODO: Наверное этот класс не нужен или его надо добавить в другое место
        /// <summary>
        /// Класс змейки для использования в логике игры
        /// </summary> 
        public sealed class SnakesForLogic // TODO: запись предыдущего хода, длина...
        {
            public List<SnakeBase> Snakes = new List<SnakeBase>(); 
        }

        public delegate bool GameoverPredicate();

        public enum GameoverPredicates
        {
            DeadAllPlayers,
            DeadAllSnakes,
            LeftOneAliveSnake,
            Achieved30Cels
        }
    }
}
