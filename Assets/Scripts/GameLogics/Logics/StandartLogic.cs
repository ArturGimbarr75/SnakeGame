using System.Collections;
using System.Collections.Generic;
using System;

using Snake;
using Assets.Scripts.GameLogics;
using Map;


namespace Logic
{

    /// <summary>
    /// Стандартная логика игры
    /// </summary>
    public class StandartLogic : GameLogicBase
    {
        private ISnakeFactory snakeFactory;

        #region Constructors

        /// <summary>
        /// Конструктор принимающий змеек
        /// </summary>
        /// <param name="snakeNames">Змейки учавствующие в игре</param>
        public StandartLogic(GameLogicsAttributes.SnakesForLogic snakeNames) // TODO: Следует отказаться от этого конструктора
        {
            this.SnakesForLogic = snakeNames;
        }

        /// <summary>
        /// Конструктор принимающий имена змеек участвующих в игре
        /// </summary>
        /// <param name="snakeNames">Имена змеек</param>
        public StandartLogic(List<string> snakeNames, ISnakeFactory snakeFactory, int mapSideSize, int foodCount)
        {
            this.snakeFactory = snakeFactory;
            this.SnakesForLogic = new GameLogicsAttributes.SnakesForLogic();
            map = new PlayingMap(mapSideSize, foodCount);
            var snakesCordinates = GetInitialSnakesCordinates(mapSideSize, snakeNames.Count);
            for (int i = 0; i < snakeNames.Count; i++)
                SnakesForLogic.Snakes.Add (snakeFactory.GetSnakeByName(snakeNames[i], snakesCordinates[i]));
        }

        /// <summary>
        /// Используется только для теста.
        /// Только чтобы Андрей поигрался с UI
        /// </summary>
        public StandartLogic() // TODO: Удалить этот метот, он нужен только чтобы Андрей поигрался с UI
        {
            AssemblySnakeFactory assemblySnakeFactory = new AssemblySnakeFactory();
            const int sideSize = 50;
            const int foodCount = 15;
            map = new PlayingMap(sideSize, foodCount);

            var snakesCordinates = GetInitialSnakesCordinates(sideSize, 6);

            this.SnakesForLogic = new GameLogicsAttributes.SnakesForLogic();
            SnakesForLogic.Snakes.Add (assemblySnakeFactory.GetSnakeByName ("PlayerArrows",     snakesCordinates[0]));
            SnakesForLogic.Snakes.Add (assemblySnakeFactory.GetSnakeByName ("PlayerWASD",       snakesCordinates[1]));
            SnakesForLogic.Snakes.Add (assemblySnakeFactory.GetSnakeByName ("RandPathwaySnake", snakesCordinates[2]));
            SnakesForLogic.Snakes.Add (assemblySnakeFactory.GetSnakeByName ("RandPathwaySnake", snakesCordinates[3]));
            SnakesForLogic.Snakes.Add (assemblySnakeFactory.GetSnakeByName ("RandPathwaySnake", snakesCordinates[4]));
            SnakesForLogic.Snakes.Add (assemblySnakeFactory.GetSnakeByName ("RandPathwaySnake", snakesCordinates[5]));
        }

        #endregion

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

            int marginRow = sideSize / (column + 1);
            int marginColumn = sideSize / (row + 1);

            for (int i = 0; i < column; i++)
                for (int j = 0; j < row; j++)
                {
                    List<SnakeAttribute.Cordinates> temp = new List<SnakeAttribute.Cordinates>();
                    for (int y = 0; y < bodySize; y++)
                    {
                        temp.Add(new SnakeAttribute.Cordinates(marginColumn * i, marginRow * j + y));
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
        private void InsertFood()
        {
            Random random = new Random();

            while (map.Food.FoodCordinates.Count < map.Food.MaxCount)
            {
                SnakeAttribute.Cordinates foodCordinates = new SnakeAttribute.Cordinates
                    (random.Next(0, map.sideSize), random.Next(0, map.sideSize));

                if (!CollisionWithSnakes(foodCordinates) && !CollisionWithFood(foodCordinates)
                    && !CollisionWithBarriers(foodCordinates))
                    map.Food.FoodCordinates.Add(foodCordinates);
            }
        }

        /// <summary>
        /// Метот считывает все следующие шаги змеек.
        /// Производит логические операции 
        /// </summary>
        /// <returns>Возвращает карту с новым положением объектов</returns>
        public override PlayingMap GetNextPlayingMap()
        {
            PlayingMap tempMap = map;
            map.Snake.Clear();
            // Считываем следующие направления
            foreach (var snake in SnakesForLogic.Snakes)
            {
                // Если змейка мертва у нее ничего не просим
                if (!snake.isAlive)
                {
                    map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive));
                    continue;
                }

                SnakeAttribute.SnakePathway snakePathway = snake.GetNextPathway(tempMap);
                SnakeAttribute.Cordinates snakeHead = snake.Head;
                switch (snakePathway)
                {
                    case SnakeAttribute.SnakePathway.Up:
                        
                        break;

                    case SnakeAttribute.SnakePathway.Right:
                        break;

                    case SnakeAttribute.SnakePathway.Down:
                        break;

                    case SnakeAttribute.SnakePathway.Left:
                        break;

                    default:
                        throw new ArgumentException(nameof(snakePathway), "Unknown pathway");
                }

            }
            // stop there
            return null;
        }
    }
}
