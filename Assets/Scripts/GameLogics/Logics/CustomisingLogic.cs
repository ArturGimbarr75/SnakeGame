using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Map;
using Snake;
using Situations;

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
            int mapSideSize, int foodCount, bool leftDeadSnakeBody)
            : base(gameoverPredicates, snakeFactory, mapSideSize, foodCount, snakeNames, leftDeadSnakeBody)
        {
            CollisionWithBarrier = new Dead();
            CollisionWithFood = new IncreaseBody();
            CollisionWithSnake = new Destroy();
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
            var tempMap = new PlayingMap(Map);
            Map.Snake.Clear();
            // Считываем следующие направления
            //Reads next directions
            foreach (var snake in SnakesForLogic.Snakes)
            {
                // Если змейка мертва у нее ничего не просим
                //In case of snake's dead dont ask for anything
                if (!snake.isAlive)
                {
                    if (LeftDeadSnakeBody)
                        Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake, snake.Statistics));
                    continue;
                }

                SnakeAttribute.SnakePathway snakePathway = snake.GetNextPathway(tempMap);
                snake.Statistics.Steps++;
                SnakeAttribute.Cordinates snakeHead = new SnakeAttribute.Cordinates(snake.Head);
                // Если змейка после шага погибает, мы ее не передвигаем
                // In case a snake dies after making a step, we don't move it
                switch (snakePathway)
                {
                    case SnakeAttribute.SnakePathway.Up:
                        snakeHead.Y = (snakeHead.Y - 1 != -1) ? --snakeHead.Y : Map.sideSize - 1;
                        break;

                    case SnakeAttribute.SnakePathway.Right:
                        snakeHead.X = (snakeHead.X + 1 != Map.sideSize) ? ++snakeHead.X : 0;
                        break;

                    case SnakeAttribute.SnakePathway.Down:
                        snakeHead.Y = (snakeHead.Y + 1 != Map.sideSize) ? ++snakeHead.Y : 0;
                        break;

                    case SnakeAttribute.SnakePathway.Left:
                        snakeHead.X = (snakeHead.X - 1 != -1) ? --snakeHead.X : Map.sideSize - 1;
                        break;

                    default:
                        throw new ArgumentException(nameof(snakePathway), "Unknown pathway");
                }
                snake.SnakeBody.Insert(0, snakeHead);
                Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake, snake.Statistics));
            }
            var afterStepsMapDublicate = new PlayingMap(Map);

            // Если нет коллизии с едой, удаляем последнюю часть змейки
            // If has collision whith food remove last snakes element
            for (int i = 0; i < Map.Snake.Count; i++)
                if (!Map.Snake[i].IsAlive)
                    continue;
                else if (!CollisionWithFood.OnCollision(Map.Snake[i], afterStepsMapDublicate))
                {
                    Map.Snake[i].Cordinates.RemoveAt(Map.Snake[i].Cordinates.Count - 1);
                }
                else
                    for (int j = 0; j < Map.Food.MaxCount; j++)
                        if (Map.Food.FoodCordinates[j] == Map.Snake[i].Cordinates[0])
                        {
                            Map.Food.FoodCordinates.RemoveAt(j);
                            break;
                        }
            for (int i = 0; i < Map.Snake.Count; i++)
            {
                if (!Map.Snake[i].IsAlive)
                    continue;
                CollisionWithBarrier.OnColision(Map.Snake[i], afterStepsMapDublicate);
                CollisionWithSnake.OnColision(Map.Snake[i], afterStepsMapDublicate, Map);
                if (!Map.Snake[i].IsAlive)
                if (AchievedLength != null && Map.Snake[i].IsAlive)
                    AchievedLength.OnAchievedLength(Map.Snake[i], afterStepsMapDublicate);
                if (DidStepsWithoutFood != null && Map.Snake[i].IsAlive)
                    DidStepsWithoutFood.OnStepsDid(Map.Snake[i], afterStepsMapDublicate);
            }

            InsertFood(Map);
            UpdateLengthStatistics();

            return Map;
        }
    }
}
