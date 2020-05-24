using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Snake;

namespace Map
{
    /// <summary>
    /// Класс хранящий информацию о поле игры
    /// Class containing game map information
    /// </summary>
    public sealed class PlayingMap
    {
        /// <summary>
        /// Минимальный размер карты
        /// Minimus isze of the map
        /// </summary>
        public const int MinSize = 20;
        /// <summary>
        /// Максимальный размер карты
        /// Maximus size of the map
        /// </summary>
        public const int MaxSize = 200; 
        /// <summary>
        /// Высота карты
        /// Map's hight
        /// </summary>
        public int sideSize { get; }
        /// <summary>
        /// Количество еды и ее кординаты
        /// Amount of food and it's coordinates
        /// </summary>
        public PlayingMapAttributes.Food Food { get; set; } = new PlayingMapAttributes.Food();
        /// <summary>
        /// Змейки на карте
        /// Snakes in the game map
        /// </summary>
        public List<PlayingMapAttributes.Snake> Snake { get; set; } = new List<PlayingMapAttributes.Snake>();
        /// <summary>
        /// Кординаты барьеров на карте
        /// Obstacles' coordinates in the game map
        /// </summary>
        public List<SnakeAttribute.Cordinates> Barriers { get; set; } = new List<SnakeAttribute.Cordinates>();

        /// <summary>
        /// Конструктор создающий игровую карту
        /// Game creating constructor
        /// </summary>
        /// <param name="sideSize">Длина стороны карты/Side size of the map</param>
        /// <param name="maxFoodCount">Максимальное количество еды/Max amount of food</param>
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
        /// Copy constructor
        /// </summary>
        /// <param name="playingMap">Объект для копирования/Object for copying</param>
        public PlayingMap (PlayingMap playingMap)
        {
            sideSize = playingMap.sideSize;
            Food = new PlayingMapAttributes.Food();
            Food.MaxCount = playingMap.Food.MaxCount;

            Food.FoodCordinates = new List<SnakeAttribute.Cordinates>();
            for (int i = 0; i < playingMap.Food.FoodCordinates.Count; i++)
                Food.FoodCordinates.Add(new SnakeAttribute.Cordinates(playingMap.Food.FoodCordinates[i]));

            Snake = new List<PlayingMapAttributes.Snake>();
            for (int i = 0; i < playingMap.Snake.Count; i++)
            {
                Snake.Add(new PlayingMapAttributes.Snake
                    (
                    playingMap.Snake[i].Name,                   
                    new List<SnakeAttribute.Cordinates>(),
                    playingMap.Snake[i].SnakeB,
                    playingMap.Snake[i].SnakeStatistics
                    ));
                for (int j = 0; j < playingMap.Snake[i].Cordinates.Count; j++)
                    Snake[i].Cordinates.Add(new SnakeAttribute.Cordinates(playingMap.Snake[i].Cordinates[j]));
            }

            Barriers = new List<SnakeAttribute.Cordinates>();
            for (int i = 0; i < playingMap.Barriers.Count; i++)
                Food.FoodCordinates.Add(new SnakeAttribute.Cordinates(playingMap.Barriers[i]));
        }
    }
}