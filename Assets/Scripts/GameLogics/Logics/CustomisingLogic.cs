using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Map;
using Snake;
using Situations;
using Assets.Scripts.Menu.Attributes;

namespace Logic
{
    public class CustomisingLogic : GameLogicBase
    {
        #region Params

        protected IAchievedLength AchievedLength;
        protected ICollisionWithBarrier CollisionWithBarrier;
        protected ICollisionWithFood CollisionWithFood;
        protected ICollisionWithSnake CollisionWithSnake;
        protected IDidStepsWithoutFood DidStepsWithoutFood;

        #endregion

        #region Constructors

        public CustomisingLogic(HashSet<GameLogicsAttributes.GameoverPredicates> gameoverPredicates,
            List<string> snakeNames, ISnakeFactory snakeFactory,
            int mapSideSize, int foodCount, bool leftDeadSnakeBody, GameLogicsAttributes.Barriers barriers = GameLogicsAttributes.Barriers.None)
            : base(gameoverPredicates, snakeFactory, mapSideSize, foodCount, snakeNames, leftDeadSnakeBody, barriers)
        {
            var situations = SituationsInit.Instance.GetSituationsObjects();
            AchievedLength = situations.AchievedLength;
            CollisionWithBarrier = situations.CollisionWithBarrier;
            CollisionWithFood = situations.CollisionWithFood;
            CollisionWithSnake = situations.CollisionWithSnake;
            DidStepsWithoutFood = situations.DidStepsWithoutFood;
        }

        #endregion

        /// <summary>
        /// Метот считывает все следующие шаги змеек.
        /// Производит логические операции 
        /// Method reads all next steps of snakes and make logic operations
        /// </summary>
        /// <returns>Возвращает карту с новым положением объектов
        /// Returns map with new location of objects</returns>
        public override PlayingMap GetNextPlayingMap()
        {
            var previousMap = new PlayingMap(Map);
            Map.Snake.Clear();

            ReadNextSnakesPathwayAndMove(previousMap);
            CheckCollisionWithFoodAndRemoveFood(previousMap);

            foreach (var snake in Map.Snake)
                if (snake.FoundFoodAfterStep)
                    CollisionWithFood.OnCollision(snake, Map, previousMap);

            foreach (var snake in Map.Snake)
            {
                if (snake.IsAlive && CollisionWithBarrier(snake, Map))
                    CollisionWithBarrier.OnCollision(snake, Map, previousMap);
            }

            while (SnakesHaveCollisionsWithOtherSnakes(Map))
                foreach (var snake in Map.Snake)
                {
                    if (CollisionWithSnakes(snake, Map))
                        CollisionWithSnake.OnCollision(snake, Map, previousMap);
                }

            foreach (var snake in Map.Snake)
            {
                if (snake.IsAlive)
                    DidStepsWithoutFood.OnStepDid(snake, Map, previousMap, this);
            }

            foreach (var snake in Map.Snake)
            {
                if (snake.Cordinates.Count >= AchievedLength.Length)
                    AchievedLength.OnAchievedLength(snake, Map, previousMap, this);
            }

            AchievedLength.AddSnakes(Map);

            InsertFood(Map);

            return Map;
        }

        private void CheckCollisionWithFoodAndRemoveFood(PlayingMap previousMap)
        {
            HashSet<SnakeAttribute.Cordinates> foodCoordsToRemove = new HashSet<SnakeAttribute.Cordinates>();
            foreach (var snake in Map.Snake)
                if (snake.IsAlive)
                    if (CollisionWithFood(snake.SnakeB.Head, previousMap))
                    {
                        snake.SnakeB.Statistics.EatenFood++;
                        snake.SnakeB.Statistics.Length = snake.Cordinates.Count;
                        snake.FoundFoodAfterStep = true;
                        foodCoordsToRemove.Add(snake.SnakeB.Head);
                    }

            Map.Food.FoodCordinates = Map.Food.FoodCordinates.Where(x => !foodCoordsToRemove.Contains(x)).ToList();
        }

        private void ReadNextSnakesPathwayAndMove(PlayingMap previousMap)
        {
            foreach (var snake in SnakesForLogic.Snakes)
            {
                if (!snake.IsAlive)
                {
                    if (LeftDeadSnakeBody)
                        Map.Snake.Add(new PlayingMapAttributes.Snake(snake));
                    continue;
                }

                var pathway = snake.GetNextPathway(previousMap);
                snake.Statistics.Steps++;
                var headCoord = snake.Head;

                switch (pathway)
                {
                    case SnakeAttribute.SnakePathway.Up:
                        headCoord.Y = (headCoord.Y - 1 != -1) ? --headCoord.Y : Map.sideSize - 1;
                        break;

                    case SnakeAttribute.SnakePathway.Right:
                        headCoord.X = (headCoord.X + 1 != Map.sideSize) ? ++headCoord.X : 0;
                        break;

                    case SnakeAttribute.SnakePathway.Down:
                        headCoord.Y = (headCoord.Y + 1 != Map.sideSize) ? ++headCoord.Y : 0;
                        break;

                    case SnakeAttribute.SnakePathway.Left:
                        headCoord.X = (headCoord.X - 1 != -1) ? --headCoord.X : Map.sideSize - 1;
                        break;
                }
                snake.SnakeBody.Insert(0, headCoord);
                snake.SnakeBody.RemoveAt(snake.SnakeBody.Count - 1);
                Map.Snake.Add(new PlayingMapAttributes.Snake(snake));
            }
        }
    }
}
