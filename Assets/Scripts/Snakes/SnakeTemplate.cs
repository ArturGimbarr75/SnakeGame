using Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    /// <summary>
    /// Представляет собой шаблон для реализации и использования змеек в игре
    /// Template for snakes' realization and using
    /// </summary>
    public abstract class SnakeBase
    {
        #region Params
        public int ID;
        /// <summary>
        /// Имя змейки
        /// </summary>
        public string SnakeName => this.GetType().Name;
        /// <summary>
        /// Длина змейки
        /// </summary>
        public int SnakeLength => SnakeBody.Count;
        /// <summary>
        /// Текущее направление
        /// </summary>
        public SnakeAttribute.SnakePathway CurrentPathway { get; protected set; }
        /// <summary>
        /// Предыдущее направление
        /// </summary>
        public SnakeAttribute.SnakePathway LastPathway { get => GetLastPathway(); protected set { lastPathway = value; } }
        public SnakeAttribute.SnakePathway lastPathway { get; protected set; }
        /// <summary>
        /// Кординаты головы змейки
        /// </summary>
        public SnakeAttribute.Cordinates Head => SnakeBody[0];
        /// <summary>
        /// Кординаты хвоста змейки (последняя кордината тела)
        /// </summary>
        public SnakeAttribute.Cordinates Tail => SnakeBody[SnakeBody.Count - 1];
        /// <summary>
        /// Кординаты тела змейки
        /// </summary>
        public List<SnakeAttribute.Cordinates> SnakeBody { get; }
        /// <summary>
        /// Значиние указывающее на то, жива ли змейка
        /// </summary>
        public bool IsAlive = true;
        /// <summary>
        /// Статистика змеек
        /// </summary>
        public SnakeStatistics Statistics;

        #endregion

        #region Methods

        /// <summary>
        /// Конструктор
        /// </summary>
        protected SnakeBase()
        {
            Statistics = new SnakeStatistics(SnakeName);
            SnakeBody = new List<SnakeAttribute.Cordinates>();

        }

        /// <summary>
        /// Метод который возвращает следующее движение змейки
        /// </summary>
        /// <param name="map">Текущее игровое поле</param>
        /// <returns>Следующее направление змейки</returns>
        public abstract SnakeAttribute.SnakePathway GetNextPathway(PlayingMap map);

        public void SetLastPathway()
        {
            CurrentPathway = GetLastPathway();
        }

        private SnakeAttribute.SnakePathway GetLastPathway()
        {
            if ((SnakeBody[0].X == SnakeBody[1].X + 1 || SnakeBody[0].X < SnakeBody[1].X) && SnakeBody[0].X != SnakeBody[1].X - 1 && SnakeBody[0].Y == SnakeBody[1].Y)
                lastPathway = SnakeAttribute.SnakePathway.Right;
            if ((SnakeBody[0].X == SnakeBody[1].X - 1 || SnakeBody[0].X > SnakeBody[1].X) && SnakeBody[0].X != SnakeBody[1].X + 1 && SnakeBody[0].Y == SnakeBody[1].Y)
                lastPathway = SnakeAttribute.SnakePathway.Left;
            if ((SnakeBody[0].Y == SnakeBody[1].Y + 1 || SnakeBody[0].Y < SnakeBody[1].Y) && SnakeBody[0].Y != SnakeBody[1].Y - 1 && SnakeBody[0].X == SnakeBody[1].X)
                lastPathway = SnakeAttribute.SnakePathway.Down;
            if ((SnakeBody[0].Y == SnakeBody[1].Y - 1 || SnakeBody[0].Y > SnakeBody[1].Y) && SnakeBody[0].Y != SnakeBody[1].Y + 1 && SnakeBody[0].X == SnakeBody[1].X)
                lastPathway = SnakeAttribute.SnakePathway.Up;

            return lastPathway;
        }

        #endregion
    }

    /// <summary>
    /// Шаблон для умной змейки
    /// Template for a smart snake
    /// </summary>
    public abstract class SmartSnakeBase : SnakeBase
    {
        /// <summary>
        /// "Генны" змейки
        /// Snake's genes
        /// </summary>
        protected SnakeAttribute.SnakeGenes Genes;
        /// <summary>
        /// Веса на выходе
        /// Pathways weights
        /// </summary>
        protected Dictionary<SnakeAttribute.SnakePathway, int> SnakePathwaysWeights;
        /// <summary>
        /// Json генов
        /// Genes JSON
        /// </summary>
        public string GenesString => Genes.ToString();

        /// <summary>
        /// Конструктор
        /// Constructor
        /// </summary>
        public SmartSnakeBase()
        {
            SetSnakeGenes();
            SnakePathwaysWeights = new Dictionary<SnakeAttribute.SnakePathway, int>();
            SnakePathwaysWeights.Add(SnakeAttribute.SnakePathway.Up, 0);
            SnakePathwaysWeights.Add(SnakeAttribute.SnakePathway.Down, 0);
            SnakePathwaysWeights.Add(SnakeAttribute.SnakePathway.Left, 0);
            SnakePathwaysWeights.Add(SnakeAttribute.SnakePathway.Right, 0);
        }

        /// <summary>
        /// Установка геннов змеек
        /// Setting snakes' genes
        /// </summary>
        protected abstract void SetSnakeGenes();


        /// <summary>
        /// Метод просчитывает вес решений
        /// если идти через стены или не обязательно
        /// Method that calculates decision weights and determines whether should you go through the walls or not
        /// </summary>
        /// <param name="map">Карта/Map</param>
        /// <param name="xFactor">Множитель кординаты X/X coordinate multiplier</param>
        /// <param name="yFactor">Множитель кординаты Y/Y coordinate multiplier</param>
        /// <param name="path">Путь/Path</param>
        protected void CheckOtherSide(PlayingMap map, int xFactor, int yFactor, SnakeAttribute.SnakePathway path)
        {
            int genesSize = Genes.FoodGenes[SnakeAttribute.SnakePathway.Up].GetLength(0);
            int genesCenter = genesSize / 2;

            foreach (var food in map.Food.FoodCordinates)
            {
                if (   xFactor * map.sideSize + food.X - Head.X + genesCenter < genesSize
                    && yFactor * map.sideSize + food.Y - Head.Y + genesCenter < genesSize
                    && xFactor * map.sideSize + food.X - Head.X + genesCenter >= 0
                    && yFactor * map.sideSize + food.Y - Head.Y + genesCenter >= 0) // TODO: Оптимищировать используя Math.Abs()
                {
                    SnakePathwaysWeights[path] +=
                        Genes.FoodGenes[path][yFactor * map.sideSize + food.Y - Head.Y + genesCenter,
                                              xFactor * map.sideSize + food.X - Head.X + genesCenter];
                }
            }

            foreach (var barrier in map.Barriers)
            {
                if (   xFactor * map.sideSize + barrier.X - Head.X + genesCenter < genesSize
                    && yFactor * map.sideSize + barrier.Y - Head.Y + genesCenter < genesSize
                    && xFactor * map.sideSize + barrier.X - Head.X + genesCenter >= 0
                    && yFactor * map.sideSize + barrier.Y - Head.Y + genesCenter >= 0)
                {
                    SnakePathwaysWeights[path] +=
                        Genes.BarrierGenes[path][yFactor * map.sideSize + barrier.Y - Head.Y + genesCenter,
                                                 xFactor * map.sideSize + barrier.X - Head.X + genesCenter];
                }
            }

            foreach (var snake in map.Snake)
                foreach (var snakeCordinates in snake.Cordinates)
                {
                    if (   xFactor * map.sideSize + snakeCordinates.X - Head.X + genesCenter < genesSize
                        && yFactor * map.sideSize + snakeCordinates.Y - Head.Y + genesCenter < genesSize
                        && xFactor * map.sideSize + snakeCordinates.X - Head.X + genesCenter >= 0
                        && yFactor * map.sideSize + snakeCordinates.Y - Head.Y + genesCenter >= 0)
                    {
                        SnakePathwaysWeights[path] +=
                            Genes.BarrierGenes[path]
                            [yFactor * map.sideSize + snakeCordinates.Y - Head.Y + genesCenter,
                             xFactor * map.sideSize + snakeCordinates.X - Head.X + genesCenter];
                    }
                }
        }

        /// <summary>
        /// Метот ищет пусти с максимальными весами
        /// Methond is finding paths with max weights
        /// </summary>
        /// <returns>Пусти с максимальными весами/Paths with maximus weights</returns>
        protected List<SnakeAttribute.SnakePathway> FindMaxWeights ()
        {
            List<SnakeAttribute.SnakePathway> maxWeights =
                new List<SnakeAttribute.SnakePathway>();
            int maxWeght = SnakePathwaysWeights[SnakeAttribute.SnakePathway.Up];

            foreach (var pathway in (SnakeAttribute.SnakePathway[])(Enum.GetValues(typeof(SnakeAttribute.SnakePathway))))
                if (SnakePathwaysWeights[pathway] > maxWeght)
                    maxWeght = SnakePathwaysWeights[pathway];

            foreach (var pathway in (SnakeAttribute.SnakePathway[])(Enum.GetValues(typeof(SnakeAttribute.SnakePathway))))
                if (SnakePathwaysWeights[pathway] == maxWeght)
                    maxWeights.Add(pathway);

            return maxWeights;
        }

        public override SnakeAttribute.SnakePathway GetNextPathway(PlayingMap map)
        {
            foreach (var path in (SnakeAttribute.SnakePathway[])(Enum.GetValues(typeof(SnakeAttribute.SnakePathway))))
                SnakePathwaysWeights[path] = 0;

            foreach (var path in (SnakeAttribute.SnakePathway[])(Enum.GetValues(typeof(SnakeAttribute.SnakePathway))))
                for (int xFactor = -1; xFactor <= 1; xFactor++)
                    for (int yFactor = -1; yFactor <= 1; yFactor++)
                        CheckOtherSide(map, xFactor, yFactor, path);

            // Поиск самого маленького возможного веса, чтобы не допустить похода под себя =D
            // Finding minimum weight, so that snake doesn't step under itself
            int minWeight =
                Math.Abs(8 * SnakeAttribute.SnakeGenes.MinGenesValue * Genes.FoodGenes.Count * Genes.FoodGenes.Count) * 2;

            switch (LastPathway)
            {
                case SnakeAttribute.SnakePathway.Up:
                    SnakePathwaysWeights[SnakeAttribute.SnakePathway.Down] -= minWeight;
                    break;

                case SnakeAttribute.SnakePathway.Down:
                    SnakePathwaysWeights[SnakeAttribute.SnakePathway.Up] -= minWeight;
                    break;

                case SnakeAttribute.SnakePathway.Right:
                    SnakePathwaysWeights[SnakeAttribute.SnakePathway.Left] -= minWeight;
                    break;

                case SnakeAttribute.SnakePathway.Left:
                    SnakePathwaysWeights[SnakeAttribute.SnakePathway.Right] -= minWeight;
                    break;
            }

            var paths = FindMaxWeights();
            return paths[UnityEngine.Random.Range(0, paths.Count - 1)];
        }
    }
}
