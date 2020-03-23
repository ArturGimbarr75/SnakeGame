using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Newtonsoft.Json;

namespace Snake
{
    /// <summary>
    /// Атрибуты для параметров змейки
    /// Attributes for snake's parameters
    /// </summary>
    public class SnakeAttribute
    {
        /// <summary>
        /// Направления движения змейки
        /// Snake's direction
        /// </summary>
        public enum SnakePathway
        {
            Up,
            Right,
            Down,
            Left
        }

        /// <summary>
        /// Кординаты объектов игрового поля
        /// Objects' coordinates in the map
        /// </summary>
        public struct Cordinates
        {
            /// <summary>
            /// Стандартный конструктор
            /// Standart constructor
            /// </summary>
            /// <param name="X">Кордината Х/X coordinate</param>
            /// <param name="Y">Кордината Y/Y coordinate</param>
            public Cordinates(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }
            /// <summary>
            /// Копи конструктор
            /// Copy constructor
            /// </summary>
            /// <param name="X">Кордината Х/X coordinate</param>
            /// <param name="Y">Кордината Y/Y coordinate</param>
            public Cordinates(Cordinates cordinates)
            {
                this.X = cordinates.X;
                this.Y = cordinates.Y;
            }

            /// <summary>
            /// Кордината Х
            /// X coordinate
            /// </summary>
            public int X { get; set; }
            /// <summary>
            /// Кордината Y
            /// Y coordinate
            /// </summary>
            public int Y { get; set; }

            /// <summary>
            /// Проверка на равенство кординат
            /// Coordinate equality check
            /// </summary>
            /// <param name="cordinate1">Кордината/Coordinate</param>
            /// <param name="cordinate2">Кордината/Coordinate</param>
            /// <returns>True если кординаты равны/Return true if coordinates are equal</returns>
            public static bool operator == (Cordinates cordinate1, Cordinates cordinate2)
                => (cordinate1.X == cordinate2.X && cordinate1.Y == cordinate2.Y);

            /// <summary>
            /// Проверка на неравенство кординат
            /// Inequality check
            /// </summary>
            /// <param name="cordinate1">Кордината/Coordinate</param>
            /// <param name="cordinate2">Кордината/Coordinate</param>
            /// <returns>True если кординаты не равны/Returns true if coordinates are inequal</returns>
            public static bool operator != (Cordinates cordinate1, Cordinates cordinate2)
                => !(cordinate1.X == cordinate2.X && cordinate1.Y == cordinate2.Y);
        }

        /// <summary>
        /// "Генны" умных змеек
        /// "Genes" of smart snakes
        /// </summary>
        public sealed class SnakeGenes
        {
            /// <summary>
            /// Создаем случайный "генном" 
            /// Creating a rangom genome
            /// </summary>
            /// <param name="sideSize">Размер стороны "геннома"/Side size of genome</param>
            public SnakeGenes (int sideSize = StandartSideSize)
            {
                FoodGenes = new Dictionary<SnakePathway, int[,]>();
                BarrierGenes = new Dictionary<SnakePathway, int[,]>();

                foreach (var pathway in (SnakePathway[])(Enum.GetValues(typeof(SnakePathway))))
                {
                    FoodGenes[pathway] = new int[sideSize, sideSize];
                    BarrierGenes[pathway] = new int[sideSize, sideSize];

                    for (int i = 0; i < sideSize; i++)
                        for (int j = 0; j < sideSize; j++)
                        {
                            FoodGenes[pathway][i, j] = Random.Range(MinGenesValue, MaxGenesValue);
                            BarrierGenes[pathway][i, j] = Random.Range(MinGenesValue, MaxGenesValue);
                        }
                }
            }

            /// <summary>
            /// Конструктор в который передется генном еды и преград
            /// В случае невалидности геннов создается случайный генном
            /// Constructor which creates a random genome in case of invalid food and obstacle genomes
            /// </summary>
            /// <param name="foodGenes">Генном еды/Food genome</param>
            /// <param name="barrierGenes">Генном/ Obstacle genome </param>
            [JsonConstructor]
            public SnakeGenes (Dictionary<SnakePathway, int[,]> foodGenes, Dictionary<SnakePathway, int[,]> barrierGenes)
            {
                int sideSize = foodGenes[SnakePathway.Up].GetLength(0);
                try
                { 
                    CheckGenesSidesEquality(sideSize, foodGenes);
                    CheckGenesSidesEquality(sideSize, barrierGenes);
                    BarrierGenes = barrierGenes;
                    FoodGenes = foodGenes;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    throw; //TODO: Посмотреть чтобы все не сломало
                }
            }

            public override string ToString()
            {
                if (BarrierGenes == null || FoodGenes == null)
                {
                    return null;
                }

                return JsonConvert.SerializeObject(this);
            }

            /// <summary>
            /// Проверка геннов на валидность
            /// Checking if genomes are valid
            /// </summary>
            /// <param name="sideSize">Размер стороны "геннома"/Side size of genome</param>
            /// <param name="genes">Генном/Genome</param>
            private static void CheckGenesSidesEquality(int sideSize, Dictionary<SnakePathway, int[,]> genes)
            {
                if (genes == null)
                    throw new ArgumentNullException(nameof(genes), "Genes could not be null");    

                if (sideSize % 2 == 0)
                    throw new ArgumentException(nameof(sideSize),"Size should be uneven");

                foreach (var pathway in (SnakePathway[])(Enum.GetValues(typeof(SnakePathway))))
                {
                    if (!(genes[pathway].GetLength(1) == sideSize
                        && genes[pathway].GetLength(0) == genes[pathway].GetLength(1)))
                        throw new Exception($"Height and Weight should be equal. In pathway: {pathway}.");

                    for (int i = 0; i < sideSize; i++)
                        for (int j = 0; j < sideSize; j++)
                        {
                            int? value = genes[pathway][i, j];
                            if (value == null)
                                throw new Exception($"Gene[{i}][{j}] should be initialised");
                        }
                }
            }
            /// <summary>
            /// Генном преград
            /// Obstacle genome
            /// </summary>
            public Dictionary<SnakePathway, int[,]> BarrierGenes { get; }
            /// <summary>
            /// Генном еды
            /// Food genome
            /// </summary>
            public Dictionary<SnakePathway, int[,]> FoodGenes { get; }

            /// <summary>
            /// Стандартный размер стороны геннома 
            /// Standart side size of genome
            /// </summary>
            public const int StandartSideSize = 11;
            /// <summary>
            /// Максимальный размер генна
            /// Max size of genome
            /// </summary>
            public const int MaxGenesValue = 100;
            /// <summary>
            /// Минимальный размер генна
            /// Min size of genome
            /// </summary>
            public const int MinGenesValue = -100;
        }
    }
}
