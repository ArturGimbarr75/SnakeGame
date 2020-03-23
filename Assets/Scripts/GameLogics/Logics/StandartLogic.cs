using System.Collections;
using System.Collections.Generic;
using System;

using Snake;
using Assets.Scripts.GameLogics;
using Map;
using UnityEngine;

namespace Logic
{
    /// <summary>
    /// Стандартная логика игры
    /// Standart logic of the game
    /// </summary>
    public class StandartLogic : GameLogicBase
    {
        #region Constructors

        public StandartLogic(HashSet<GameLogicsAttributes.GameoverPredicates> gameoverPredicates,
            List<string> snakeNames, ISnakeFactory snakeFactory,
            int mapSideSize, int foodCount, bool leftDeadSnakeBody)
            : base (gameoverPredicates, snakeFactory, mapSideSize, foodCount, snakeNames, leftDeadSnakeBody)
        {    

        }

        #endregion

        /// <summary>
        /// Метот считывает все следующие шаги змеек.
        /// Производит логические операции 
        /// Method reads all next steps of snakes and make logic operations
        /// </summary>
        /// <returns>Возвращает карту с новым положением объектов</returns>
        /// Returns map with new location of objects
        public override PlayingMap GetNextPlayingMap()
        {
            PlayingMap tempMap = new PlayingMap (Map);
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
                        Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive, snake.Statistics));
                    continue;
                }

                SnakeAttribute.SnakePathway snakePathway = snake.GetNextPathway(tempMap);
                snake.Statistics.Steps++;
                SnakeAttribute.Cordinates snakeHead = snake.Head;
                // Если змейка после шага погибает, мы ее не передвигаем
                //In case a snake dies after making a step, we don't move it
                switch (snakePathway)
                {
                    case SnakeAttribute.SnakePathway.Up:
                        snakeHead.Y = (snakeHead.Y - 1 != -1) ? --snakeHead.Y : Map.sideSize - 1;
                        ReactionToMapsObjectsOnNewPosition (snakeHead, snake, tempMap);
                        break;

                    case SnakeAttribute.SnakePathway.Right:
                        snakeHead.X = (snakeHead.X + 1 != Map.sideSize) ? ++snakeHead.X : 0;
                        ReactionToMapsObjectsOnNewPosition (snakeHead, snake, tempMap);
                        break;

                    case SnakeAttribute.SnakePathway.Down:
                        snakeHead.Y = (snakeHead.Y + 1 != Map.sideSize) ? ++snakeHead.Y : 0;
                        ReactionToMapsObjectsOnNewPosition (snakeHead, snake, tempMap);
                        break;

                    case SnakeAttribute.SnakePathway.Left:
                        snakeHead.X = (snakeHead.X - 1 != -1) ? --snakeHead.X : Map.sideSize - 1;
                        ReactionToMapsObjectsOnNewPosition (snakeHead, snake, tempMap);
                        break;

                    default:
                        throw new ArgumentException(nameof(snakePathway), "Unknown pathway");
                }

                // Проверяем жива ли змейка после хода
                //Checking whether snake is alinve
                if (!snake.isAlive)
                {
                    if (LeftDeadSnakeBody)
                        Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive, snake.Statistics));
                }
                else
                {
                    Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive, snake.Statistics));
                }
            }

            // Окончательная проверка того живы ли змейки
            //Last check of snake's alive state
            foreach (var snake in SnakesForLogic.Snakes)
            {
                PlayingMap tempMapForColisionChecking = new PlayingMap(Map);
                var snakeForMap = new PlayingMapAttributes.Snake
                    (snake.SnakeName, new List<SnakeAttribute.Cordinates>(snake.SnakeBody), snake.isAlive, snake.Statistics);

                // Удаляем голову змейки из карты, чтобы у нее не было коллизии с собой 
                //Delete snake's head from the map, so that it doesn't collide with itself
                tempMapForColisionChecking.Snake.RemoveAll(s => snakeForMap == s);
                var head = snakeForMap.Cordinates[0];
                snakeForMap.Cordinates.RemoveAt(0);
                tempMapForColisionChecking.Snake.Add(snakeForMap);

                // Если обнаруживается коллизия укорачиваем змейку с головы
                //If snake collides shortening it starting from head
                if (HasCollisionAfterStep(head, tempMapForColisionChecking, snakeForMap))
                {
                    snakeForMap = new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive, snake.Statistics);
                    Map.Snake.RemoveAll(s => snakeForMap == s);
                    snakeForMap.Cordinates.RemoveAt(0);
                    Map.Snake.Add(snakeForMap);
                }
            }

            Map.Food = tempMap.Food;
            InsertFood(Map);
            UpdateLengthStatistics();

            return Map;
        }

        /// <summary>
        /// Реакция змейки на объекты на новой кординате
        /// Snake's reaction to the objects with new coordinates
        /// </summary>
        /// <param name="cordinate">Новая кордината/New cooirdinate</param>
        /// <param name="snake">Змейка/Snake</param>
        /// <param name="map">Карта с объектами/Map with objects</param>
        private void ReactionToMapsObjectsOnNewPosition (SnakeAttribute.Cordinates cordinate, SnakeBase snake, PlayingMap map)
        {
            if (CollisionWithBarriers(cordinate, map) || CollisionWithSnakesBody(cordinate, map)
                || CollisionWithSnakesHead(cordinate, map) || CollisionWithDeadSnakes(cordinate, map))
            {
                snake.isAlive = false;
                return;
            }

            if (CollisionWithFood (cordinate, map))
            {
                snake.Statistics.EatenFood++;
                map.Food.FoodCordinates.RemoveAll(c => cordinate == c);
                snake.SnakeBody.Insert(0, cordinate);
                return;
            }

            // Если нет колизии с объектами на карте, передвигаем змейку  
            //If there is no collision on map, move snake
            for (int i = snake.SnakeBody.Count - 1; i > 0; i--)
                snake.SnakeBody[i] = snake.SnakeBody[i - 1];
            snake.SnakeBody[0] = cordinate;
        }

        /// <summary>
        /// Метод используется для окончательной проверки коллизий
        /// после сделанного змейкой шага
        /// Method is used to final determination of collision after snake takes a step
        /// </summary>
        /// <param name="head">Голова змейки/Snake's head</param>
        /// <param name="map">Игровая карта/Game map</param>
        /// <param name="snake">Змейка/Snake</param>
        /// <returns>True если есть коллизия со змейкой или барьером/Returns true is there is collision with snake or obstacle</returns>
        private bool HasCollisionAfterStep (SnakeAttribute.Cordinates head, PlayingMap map, PlayingMapAttributes.Snake snake)
        {
            if (CollisionWithFood(head, map))
            {
                Debug.Log("Error: Unexpected food element");
                Map.Food.FoodCordinates.RemoveAll(c => c == head);
            }

            if (!snake.isAlive)
                return false;

            if (CollisionWithSnakes(head, map) || CollisionWithBarriers(head, map))
                return true;

            return false;
        }

        /// <summary>
        /// Обновляем статистику длины
        /// Updating length statistics
        /// </summary>
        private void UpdateLengthStatistics()
        {
            foreach (var snake in SnakesForLogic.Snakes)
                snake.Statistics.Length = snake.SnakeLength;
        }
    }
}
