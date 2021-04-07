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
        /// Snake class, used in a game logic
        /// </summary> 
        public sealed class SnakesForLogic
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

        public enum Barriers
        {
            None,
            Solid,
            Random,
            LinesHorizontal,
            LinesVertical,
            Angles
        }
    }
}
