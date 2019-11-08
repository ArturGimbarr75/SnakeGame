using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Snake;
using Map;
using Assets.Scripts.GameLogics;

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
        protected PlayingMap Map;
        /// <summary>
        /// Поле указывающее оставлять ли мертвое тело змейки на карте как барьер
        /// </summary>
        protected bool LeftDeadSnakeBody;
        /// <summary>
        /// Объект позволяющий получить любую инстанцию змейки по имени
        /// </summary>
        protected ISnakeFactory SnakeFactory;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="snakeFactory">Производство змеек</param>
        /// <param name="mapSideSize">Сторона карты</param>
        /// <param name="foodCount">Максимальное колличество еды</param>
        public GameLogicBase (ISnakeFactory snakeFactory, int mapSideSize, int foodCount, bool leftDeadSnakeBody = false)
        {
            this.SnakeFactory = snakeFactory;
            this.SnakesForLogic = new GameLogicsAttributes.SnakesForLogic();
            Map = new PlayingMap(mapSideSize, foodCount);
            LeftDeadSnakeBody = leftDeadSnakeBody;
        }

        /// <summary>
        /// Метод получения игровой карты
        /// </summary>
        /// <returns>текущая игровая карта</returns>
        public PlayingMap GetCurrentPlayingMap() => Map;

        /// <summary>
        /// Метот считывает все следующие шаги змеек.
        /// Производит логические операции 
        /// </summary>
        /// <returns>Возвращает карту с новым положением объектов</returns>
        public abstract PlayingMap GetNextPlayingMap();

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
    }
}