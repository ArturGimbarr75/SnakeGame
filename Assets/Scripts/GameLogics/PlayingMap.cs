using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Snake;

namespace Map
{
    /// <summary>
    /// Класс хранящий информацию о поле игры
    /// </summary>
    public sealed class PlayingMap
    {
        /// <summary>
        /// Минимальный размер карты
        /// </summary>
        public const int MinSize = 20;
        /// <summary>
        /// Максимальный размер карты
        /// </summary>
        public const int MaxSize = 200; 
        /// <summary>
        /// Высота карты
        /// </summary>
        public int sideSize { get; }
        /// <summary>
        /// Количество еды и ее кординаты
        /// </summary>
        public PlayingMapAttributes.Food Food { get; set; } = new PlayingMapAttributes.Food();
        /// <summary>
        /// Змейки на карте
        /// </summary>
        public List<PlayingMapAttributes.Snake> Snake { get; set; } = new List<PlayingMapAttributes.Snake>();
        /// <summary>
        /// Кординаты барьеров на карте
        /// </summary>
        public List<SnakeAttribute.Cordinates> Barriers { get; set; } = new List<SnakeAttribute.Cordinates>();

        /// <summary>
        /// Конструктор создающий игровую карту
        /// </summary>
        /// <param name="sideSize">Длина стороны карты</param>
        /// <param name="maxFoodCount">Максимальное количество еды</param>
        public PlayingMap(int sideSize, int maxFoodCount)
        {
            if (sideSize < 20)
                throw new ArgumentException(nameof(sideSize), "Side size must be more than 19");

            this.sideSize = sideSize;
            Food.MaxCount = maxFoodCount;
            Food.FoodCordinates = new List<SnakeAttribute.Cordinates>();
            Snake = new List<PlayingMapAttributes.Snake>();
        }

        /// <summary>
        /// Копи конструктор
        /// </summary>
        /// <param name="playingMap">Объект для копирования</param>
        public PlayingMap (PlayingMap playingMap)
        {
            sideSize = playingMap.sideSize;
            Food = new PlayingMapAttributes.Food();
            Food.MaxCount = playingMap.Food.MaxCount;
            Food.FoodCordinates = new List<SnakeAttribute.Cordinates>(playingMap.Food.FoodCordinates);
            Snake = new List<PlayingMapAttributes.Snake>(playingMap.Snake);
            Barriers = new List<SnakeAttribute.Cordinates>(playingMap.Barriers);
        }
    }
}