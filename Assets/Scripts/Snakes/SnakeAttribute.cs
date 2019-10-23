using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Snake
{
    /// <summary>
    /// Атрибуты для параметров змейки
    /// </summary>
    public class SnakeAttribute
    {
        /// <summary>
        /// Направления движения змейки
        /// </summary>
        public enum SnakePathway
        {
            Up,
            Right,
            Down,
            Left
        }

        /// <summary>
        /// Кординаты части тела змейки
        /// </summary>
        public struct Cordinates
        {
            public Cordinates(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }
            public int X { get; set; }
            public int Y { get; set; }
        }

        /// <summary>
        /// "Генны" умных змеек
        /// </summary>
        public sealed class SnakeGenes
        {
            /// <summary>
            /// Создаем случайный "генном" 
            /// </summary>
            /// <param name="sideSize">Размер стороны "геннома"</param>
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
            /// </summary>
            /// <param name="foodGenes">Генном еды</param>
            /// <param name="barrierGenes">Генном </param>
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
                    //this = new SnakeGenes();
                    throw; //TODO: Посмотреть чтобы все не сломало
                }
            }

            /// <summary>
            /// Проверка геннов на валидность
            /// </summary>
            /// <param name="sideSize">Размер стороны "геннома"</param>
            /// <param name="genes">Генном</param>
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
            /// </summary>
            public Dictionary<SnakePathway, int[,]> BarrierGenes { get; }
            /// <summary>
            /// Генном еды
            /// </summary>
            public Dictionary<SnakePathway, int[,]> FoodGenes { get; }

            /// <summary>
            /// Стандартный размер стороны геннома 
            /// </summary>
            public const int StandartSideSize = 9;
            /// <summary>
            /// Максимальный размер генна
            /// </summary>
            public const int MaxGenesValue = 100;
            /// <summary>
            /// Минимальный размер генна
            /// </summary>
            public const int MinGenesValue = -100;
        }
    }
}
