using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Snake;
using Map;

namespace Logic
{
    /// <summary>
    /// Абстрактный класс для создания разных логик
    /// </summary>
    public abstract class GameLogicBase
    {
        /// <summary>
        /// Список змеек учавствующих в игре
        /// </summary>
        protected GameLogicsAttributes.SnakesForLogic SnakesForLogic;
        /// <summary>
        /// Игровая карта
        /// </summary>
        protected PlayingMap map;

        /// <summary>
        /// Метот считывает все следующие шаги змеек.
        /// Производит логические операции 
        /// </summary>
        /// <returns>Возвращает карту с новым положением объектов</returns>
        public abstract PlayingMap GetNextPlayingMap();

        /// <summary>
        /// Метод проверяет колизию со змейками
        /// </summary>
        /// <param name="cordinates">Кординаты для проверки</param>
        /// <returns>True если есть колизия со змейкой</returns>
        protected bool CollisionWithSnakes (SnakeAttribute.Cordinates cordinates)
        {
            foreach (var snake in map.Snake)
            {
                foreach (var snakesCordinates in snake.Cordinates)
                    if (snakesCordinates == cordinates)
                    {
                        return true;
                    }
            }
            return false;
        }

        /// <summary>
        /// Метод проверяет колизию с барьерами
        /// </summary>
        /// <param name="cordinates">Кординаты для проверки</param>
        /// <returns>True если есть колизия с барьером</returns>
        protected bool CollisionWithBarriers(SnakeAttribute.Cordinates cordinates)
        {
            foreach (var barrier in map.Barriers)
                if (barrier == cordinates)
                {
                    return true;

                }
            return false;
        }

        /// <summary>
        /// Метод проверяет колизию с едой
        /// </summary>
        /// <param name="cordinates">Кординаты для проверки</param>
        /// <returns>True если есть колизия с едой</returns>
        protected bool CollisionWithFood(SnakeAttribute.Cordinates cordinates)
        {
            foreach (var food in map.Food.FoodCordinates)
                if (food == cordinates)
                {
                    return true;
                }
            return false;
        }
    }
}