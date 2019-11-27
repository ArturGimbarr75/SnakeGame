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
    /// </summary>
    public class StandartLogic : GameLogicBase
    {
        #region Constructors

        /// <summary>
        /// Конструктор принимающий имена змеек участвующих в игре
        /// </summary>
        /// <param name="snakeNames">Имена змеек</param>
        public StandartLogic(List<string> snakeNames, ISnakeFactory snakeFactory,
            int mapSideSize, int foodCount, bool leftDeadSnakeBody)
            : base (snakeFactory, mapSideSize, foodCount, leftDeadSnakeBody)
        {    
            var snakesCordinates = GetInitialSnakesCordinates(mapSideSize, snakeNames.Count);
            for (int i = 0; i < snakeNames.Count; i++)
                SnakesForLogic.Snakes.Add (snakeFactory.GetSnakeByName(snakeNames[i], snakesCordinates[i]));

            foreach (var snake in SnakesForLogic.Snakes)
                Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive));

            InsertFood(Map);
        }

        /// <summary>
        /// Используется только для теста.
        /// Только чтобы Андрей поигрался с UI
        /// </summary>
        public StandartLogic(int snakeCount = 0) // TODO: Удалить этот метот, он нужен только чтобы Андрей поигрался с UI
            : base (new AssemblySnakeFactory(), 50, 1, true)
        {
            var snakesCordinates = GetInitialSnakesCordinates(50, snakeCount);

            if (snakeCount > 0)
                SnakesForLogic.Snakes.Add (SnakeFactory.GetSnakeByName (nameof(PlayerArrows),     snakesCordinates[0]));
            if (snakeCount > 1)
                SnakesForLogic.Snakes.Add (SnakeFactory.GetSnakeByName (nameof(PlayerWASD),       snakesCordinates[1]));
            for (int i = 2; i < snakeCount; i++)
            {
                switch (i % ((SnakeFactory as AssemblySnakeFactory).GetAllSnakeTypes().Count - 2))
                {
                    case 1:
                        SnakesForLogic.Snakes.Add(SnakeFactory.GetSnakeByName(nameof(FollowFoodSnake),  snakesCordinates[i]));
                        break;

                    default:
                        SnakesForLogic.Snakes.Add(SnakeFactory.GetSnakeByName(nameof(RandPathwaySnake), snakesCordinates[i]));
                        break;
                }
            }
                

            foreach (var snake in SnakesForLogic.Snakes)
                Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive));

            InsertFood(Map);
        }

        #endregion

        /// <summary>
        /// Метот считывает все следующие шаги змеек.
        /// Производит логические операции 
        /// </summary>
        /// <returns>Возвращает карту с новым положением объектов</returns>
        public override PlayingMap GetNextPlayingMap()
        {
            PlayingMap tempMap = new PlayingMap (Map);
            Map.Snake.Clear();
            // Считываем следующие направления
            foreach (var snake in SnakesForLogic.Snakes)
            {
                // Если змейка мертва у нее ничего не просим
                if (!snake.isAlive)
                {
                    if (LeftDeadSnakeBody)
                        Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive));
                    continue;
                }

                SnakeAttribute.SnakePathway snakePathway = snake.GetNextPathway(tempMap);
                SnakeAttribute.Cordinates snakeHead = snake.Head;
                // Если змейка после шага погибает, мы ее не передвигаем
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
                if (!snake.isAlive)
                {
                    if (LeftDeadSnakeBody)
                        Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive));
                }
                else
                {
                    Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive));
                }
            }

            // Окончательная проверка того живы ли змейки
            foreach (var snake in SnakesForLogic.Snakes)
            {
                PlayingMap tempMapForColisionChecking = new PlayingMap(Map);
                var snakeForMap = new PlayingMapAttributes.Snake(snake.SnakeName, new List<SnakeAttribute.Cordinates>(snake.SnakeBody), snake.isAlive);

                // Удаляем голову змейки из карты, чтобы у нее не было коллизии с собой 
                tempMapForColisionChecking.Snake.RemoveAll(s => snakeForMap == s);
                var head = snakeForMap.Cordinates[0];
                snakeForMap.Cordinates.RemoveAt(0);
                tempMapForColisionChecking.Snake.Add(snakeForMap);

                // Если обнаруживается коллизия укорачиваем змейку с головы
                if (HasCollisionAfterStep(head, tempMapForColisionChecking, snakeForMap))
                {
                    snakeForMap = new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive);
                    Map.Snake.RemoveAll(s => snakeForMap == s);
                    snakeForMap.Cordinates.RemoveAt(0);
                    Map.Snake.Add(snakeForMap);
                }
            }

            Map.Food = tempMap.Food;
            InsertFood(Map);
            
            return Map;
        }

        /// <summary>
        /// Метод возвращающий кординаты для змеек
        /// равномерно распределленных
        /// </summary>
        /// <param name="sideSize">Размер стороны карты</param>
        /// <param name="snakeCount">Количество змеек</param>
        /// <param name="bodySize">Изначальная длина змейки</param>
        /// <returns>Массив из массивов кординат</returns>
        private List<List<SnakeAttribute.Cordinates>> GetInitialSnakesCordinates
            (int sideSize, int snakeCount, int bodySize = 3)
        {
            int row = (int)Math.Sqrt(snakeCount);
            int column = (int)Math.Round((double)(snakeCount / row));

            while (row * column < snakeCount)
                column++;

            List<List<SnakeAttribute.Cordinates>> cordinates
                = new List<List<SnakeAttribute.Cordinates>>();

            int marginRow = sideSize / (row + 1);
            int marginColumn = sideSize / (column + 1);

            for (int i = 1; i <= column; i++)
                for (int j = 1; j <= row; j++)
                {
                    List<SnakeAttribute.Cordinates> temp = new List<SnakeAttribute.Cordinates>();
                    for (int body = 0; body < bodySize; body++)
                    {
                        temp.Add(new SnakeAttribute.Cordinates(marginColumn * i, marginRow * j + body));
                    }
                    cordinates.Add(temp);
                }

            return cordinates;
        }

        /// <summary>
        /// Метод добавляющий еду на карту в зависимости
        /// от максимально возможного ее количества.
        /// Следует вызывать только после того как все
        /// змейки будут перемещены
        /// </summary>
        private void InsertFood(PlayingMap map)
        {
            System.Random random = new System.Random();

            while (map.Food.FoodCordinates.Count < map.Food.MaxCount)
            {
                SnakeAttribute.Cordinates foodCordinates = new SnakeAttribute.Cordinates
                    (random.Next(0, map.sideSize), random.Next(0, map.sideSize));

                if (!CollisionWithSnakes(foodCordinates, map) && !CollisionWithFood(foodCordinates, map)
                    && !CollisionWithBarriers(foodCordinates, map))
                    map.Food.FoodCordinates.Add(foodCordinates);
            }
        }

        /// <summary>
        /// Реакция змейки на объекты на новой кординате
        /// </summary>
        /// <param name="cordinate">Новая кордината</param>
        /// <param name="snake">Змейка</param>
        /// <param name="map">Карта с объектами</param>
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
                map.Food.FoodCordinates.RemoveAll(c => cordinate == c);
                snake.SnakeBody.Insert(0, cordinate);
                return;
            }

            // Если нет колизии с объектами на карте, передвигаем змейку           
            for (int i = snake.SnakeBody.Count - 1; i > 0; i--)
                snake.SnakeBody[i] = snake.SnakeBody[i - 1];
            snake.SnakeBody[0] = cordinate;
        }

        /// <summary>
        /// Метод используется для окончательной проверки коллизий
        /// после сделанного змейкой шага
        /// </summary>
        /// <param name="head">Голова змейки</param>
        /// <param name="map">Игровая карта</param>
        /// <param name="snake">Змейка</param>
        /// <returns>True если есть коллизия со змейкой или барьером</returns>
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
    }
}
