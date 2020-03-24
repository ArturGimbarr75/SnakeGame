using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Snake;
using Map;
using Assets.Scripts.GameLogics;
using Assets.Scripts.DataBase;

namespace Logic
{
    /// <summary>
    /// Абстрактный класс для создания разных логик
    /// Abstract class for creating a logic
    /// </summary>
    public abstract class GameLogicBase
    {
        /// <summary>
        /// Название типа игры
        /// Game's type
        /// </summary>
        public string GameType => this.GetType().Name;
        /// <summary>
        /// Список змеек учавствующих в игре
        /// List of snakes in the game
        /// </summary>
        protected GameLogicsAttributes.SnakesForLogic SnakesForLogic;
        /// <summary>
        /// Игровая карта
        /// Game map
        /// </summary>
        protected PlayingMap Map;
        /// <summary>
        /// Поле указывающее оставлять ли мертвое тело змейки на карте как барьер
        /// Field which says where dead snake should be left on the game map
        /// </summary>
        protected bool LeftDeadSnakeBody;
        /// <summary>
        /// Объект позволяющий получить любую инстанцию змейки по имени
        /// Objects which return any instance of a snake base on snake's name
        /// </summary>
        protected ISnakeFactory SnakeFactory;

        /// <summary>
        /// Базовый конструктор
        /// Base constructor
        /// </summary>
        /// <param name="snakeFactory">Производство змеек/Snakes factory</param>
        /// <param name="mapSideSize">Сторона карты/Map's edge</param>
        /// <param name="foodCount">Максимальное колличество еды/Max amount of food</param>
        public GameLogicBase (HashSet<GameLogicsAttributes.GameoverPredicates> gameoverPredicates,
            ISnakeFactory snakeFactory, int mapSideSize, int foodCount, List<string> snakeNames, bool leftDeadSnakeBody = false)
        {
            this.SnakeFactory = snakeFactory;
            this.SnakesForLogic = new GameLogicsAttributes.SnakesForLogic();
            Map = new PlayingMap(mapSideSize, foodCount);
            LeftDeadSnakeBody = leftDeadSnakeBody;
            InitialGameoverPredicate(gameoverPredicates);

            var snakesCordinates = GetInitialSnakesCordinates(mapSideSize, snakeNames.Count);
            for (int i = 0; i < snakeNames.Count; i++)
                SnakesForLogic.Snakes.Add(snakeFactory.GetSnakeByName(snakeNames[i], snakesCordinates[i]));

            foreach (var snake in SnakesForLogic.Snakes)
                Map.Snake.Add(new PlayingMapAttributes.Snake(snake.SnakeName, snake.SnakeBody, snake.isAlive, snake.Statistics));

            InsertFood(Map);
        }

        /// <summary>
        /// Метод добавляющий еду на карту в зависимости
        /// от максимально возможного ее количества.
        /// Следует вызывать только после того как все
        /// змейки будут перемещены
        /// Method which adds food to the map based on max amoung of it's instances.
        /// Should be called only when all snakes had been moved.
        /// </summary>
        public void InsertFood(PlayingMap map)
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
        /// Метод возвращающий кординаты для змеек
        /// равномерно распределленных
        /// Method which returns coordinates for equally destributed snakes
        /// </summary>
        /// <param name="sideSize">Размер стороны карты/Map's side size</param>
        /// <param name="snakeCount">Количество змеек/Snakes amount</param>
        /// <param name="bodySize">Изначальная длина змейки/Initial snake's length</param>
        /// <returns>Массив из массивов кординат/Array of coordinates array</returns>
        public List<List<SnakeAttribute.Cordinates>> GetInitialSnakesCordinates
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
        /// Метод получения игровой карты
        /// Method for obtaining game map
        /// </summary>
        /// <returns>текущая игровая карта/Current game map</returns>
        public PlayingMap GetCurrentPlayingMap() => Map;

        /// <summary>
        /// Метот считывает все следующие шаги змеек.
        /// Производит логические операции 
        /// Method reads all next snakes' steps and makes logic operations
        /// </summary>
        /// <returns>Возвращает карту с новым положением объектов/Returns a map with in a new way placed objects</returns>
        public abstract PlayingMap GetNextPlayingMap();

        /// <summary>
        /// Метод проверяет окончена ли игра 
        /// Method checks whethe game is over
        /// </summary>
        /// <returns>True если игра окончена/Returns true if game is over</returns>
        public bool IsGameEnded()
        {
            var predecates = GameoverPredicate.GetInvocationList();
            bool result = false;

            foreach (var p in predecates)
                if (((GameLogicsAttributes.GameoverPredicate)p)())
                    result = true;

            if (result)
            {
                SnakesTable snakesTable = new SnakesTable();
                PlayerTable playerTable = new PlayerTable();
                foreach (var s in SnakesForLogic.Snakes)
                {
                    snakesTable.UpdateStatistics(s);
                    if (s.SnakeName.Contains("Player"))
                        playerTable.AddNewRow(s.SnakeName, GameType, s.Statistics.EatenFood);
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает статистику игравших змеек
        /// Returns statistics of snakes, which were in the game
        /// </summary>
        /// <returns>Статистика змеек/Snake statistics</returns>
        public List<SnakeStatistics> GetSnakeStatistics()
        {
            List<SnakeStatistics> statistics = new List<SnakeStatistics>();
            foreach (var snake in SnakesForLogic.Snakes)
                statistics.Add(new SnakeStatistics(snake.Statistics));

            return statistics;
        }

        #region CheckCollisions

        /// <summary>
        /// Метод проверяет колизию со змейками
        /// </summary>
        /// <param name="cordinate">Кординаты для проверки</param>
        /// <param name="map">Карта с объектами</param>
        /// <returns>True если есть колизия со змейкой</returns>
        protected bool CollisionWithSnakes (SnakeAttribute.Cordinates cordinate, PlayingMap map)
        {
            foreach (var snake in map.Snake)
            {
                foreach (var snakesCordinates in snake.Cordinates)
                    if (snakesCordinates == cordinate)
                    {
                        return true;
                    }
            }
            return false;
        }

        /// <summary>
        /// Метод проверяет колизию с хвостами змеек
        /// </summary>
        /// <param name="cordinate">Кординаты для проверки</param>
        /// <param name="map">Карта с объектами</param>
        /// <returns>True если есть колизия с хвостами змееек</returns>
        protected bool CollisionWithSnakesTail(SnakeAttribute.Cordinates cordinate, PlayingMap map)
        {
            foreach (var snake in map.Snake)
            {
                if (snake.Cordinates[snake.Cordinates.Count - 1] == cordinate)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Метод проверяет колизию с телами змеек (без учета головы и хвоста)
        /// </summary>
        /// <param name="cordinate">Кординаты для проверки</param>
        /// <param name="map">Карта с объектами</param>
        /// <returns>True если есть колизия с телами змееек</returns>
        protected bool CollisionWithSnakesBody(SnakeAttribute.Cordinates cordinate, PlayingMap map)
        {
            foreach (var snake in map.Snake)
            {
                foreach (var snakesCordinates in snake.Cordinates)
                    if (snakesCordinates == cordinate && snake.Cordinates[0] != cordinate
                        && snake.Cordinates[snake.Cordinates.Count - 1] != cordinate)
                    {
                        return true;
                    }
            }
            return false;
        }

        /// <summary>
        /// Метод проверяет колизию с головами змеек
        /// </summary>
        /// <param name="cordinate">Кординаты для проверки</param>
        /// <param name="map">Карта с объектами</param>
        /// <returns>True если есть колизия с головами змеек</returns>
        protected bool CollisionWithSnakesHead(SnakeAttribute.Cordinates cordinate, PlayingMap map)
        {
            foreach (var snake in map.Snake)
            {
                if (snake.Cordinates[0] == cordinate)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Метод проверяет колизию с хвостами мертвых змеек
        /// </summary>
        /// <param name="cordinate">Кординаты для проверки</param>
        /// <param name="map">Карта с объектами</param>
        /// <returns>True если есть колизия с головами змеек</returns>
        protected bool CollisionWithDeadSnakes(SnakeAttribute.Cordinates cordinate, PlayingMap map)
        {
            foreach (var snake in map.Snake)
            {
                foreach(var snakesCordinates in snake.Cordinates)
                    if (snakesCordinates == cordinate && !snake.isAlive)
                    {
                        return true;
                    }
            }
            return false;
        }

        /// <summary>
        /// Метод проверяет колизию с барьерами
        /// </summary>
        /// <param name="cordinate">Кординаты для проверки</param>
        /// <param name="map">Карта с объектами</param>
        /// <returns>True если есть колизия с барьером</returns>
        protected bool CollisionWithBarriers(SnakeAttribute.Cordinates cordinate, PlayingMap map)
        {
            foreach (var barrier in map.Barriers)
                if (barrier == cordinate)
                {
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Метод проверяет колизию с едой
        /// </summary>
        /// <param name="cordinate">Кординаты для проверки</param>
        /// <param name="map">Карта с объектами</param>
        /// <returns>True если есть колизия с едой</returns>
        protected bool CollisionWithFood(SnakeAttribute.Cordinates cordinate, PlayingMap map)
        {
            foreach (var food in map.Food.FoodCordinates)
                if (food == cordinate)
                {
                    return true;
                }
            return false;
        }
        #endregion

        #region GameOverPredicates

        private GameLogicsAttributes.GameoverPredicate GameoverPredicate;

        /// <summary>
        /// Инициализация предикатов для игры
        /// </summary>
        /// <param name="gameoverPredicates">Предикаты</param>
        private void InitialGameoverPredicate (HashSet<GameLogicsAttributes.GameoverPredicates> gameoverPredicates)
        {
            GameoverPredicate = new GameLogicsAttributes.GameoverPredicate(DeadAllSnakes);
            GameoverPredicate -= DeadAllSnakes; // TODO: Придумать нормальную инициализацию

            if (gameoverPredicates == null && gameoverPredicates.Count == 0)
            {
                GameoverPredicate += DeadAllSnakes;
                return;
            }

            foreach (var pr in gameoverPredicates)
            {
                switch (pr)
                {
                    case GameLogicsAttributes.GameoverPredicates.Achieved30Cels:
                        GameoverPredicate += Achieved30Cels;
                        break;

                    case GameLogicsAttributes.GameoverPredicates.DeadAllPlayers:
                        GameoverPredicate += DeadAllPlayers;
                        break;

                    case GameLogicsAttributes.GameoverPredicates.DeadAllSnakes:
                        GameoverPredicate += DeadAllSnakes;
                        break;

                    case GameLogicsAttributes.GameoverPredicates.LeftOneAliveSnake:
                        GameoverPredicate += LeftOneAliveSnake;
                        break;
                }
            }
        }

        /// <summary>
        /// Предекат проверки мертвы ли игроки
        /// </summary>
        /// <returns>True если все мертвы</returns>
        private bool DeadAllPlayers()
        {
            var players = (from snake in Map.Snake
                           where snake.Name.Contains("Player")
                           select snake).ToList();

            if (players.Count == 0)
            {
                return DeadAllSnakes();
            }

            return players.Count (s => s.isAlive) > 0;
        }

        /// <summary>
        /// Предекат проверки мертвы ли змейки
        /// </summary>
        /// <returns>True если все мертвы</returns>
        private bool DeadAllSnakes()
        {
            var snakes = (from snake in Map.Snake
                          select snake).ToList();

            return snakes.Count (s => s.isAlive) == 0;
        }

        /// <summary>
        /// Предекат проверки жива ли одна змейка
        /// </summary>
        /// <returns>True если все мертвы кроме одной</returns>
        private bool LeftOneAliveSnake()
        {
            var snakes = (from snake in Map.Snake
                          select snake).ToList();

            return snakes.Count (s => s.isAlive) == 1;
        }

        /// <summary>
        /// Проверяет достигли ли змейки длины в 30 клеток
        /// </summary>
        /// <returns>True если хоть одна достигла или если все погибли</returns>
        private bool Achieved30Cels()
        {
            var snakes = (from snake in Map.Snake
                          select snake).ToList();

            return snakes.Count(s => s.Cordinates.Count >= 30) > 0 || DeadAllSnakes();
        }


        #endregion

    }
}