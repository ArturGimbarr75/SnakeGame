using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    /// <summary>
    /// Представляет собой шаблон для реализации и использования змеек в игре
    /// </summary>
    public abstract class SnakeBase
    {
        #region Params

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
        public SnakeAttribute.SnakePathway CurrentPathway { get; } = SnakeAttribute.SnakePathway.Up;
        /// <summary>
        /// Предыдущее направление
        /// </summary>
        public SnakeAttribute.SnakePathway LastPathway { get; protected set; } = SnakeAttribute.SnakePathway.Up;
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
        public List<SnakeAttribute.Cordinates> SnakeBody { get; } = new List<SnakeAttribute.Cordinates>();
        /// <summary>
        /// Значиние указывающее на то, жива ли змейка
        /// </summary>
        public bool isAlive = true;

        #endregion

        #region Methods

        /// <summary>
        /// Метод который возвращает следующее движение змейки
        /// </summary>
        /// <param name="map">Текущее игровое поле</param>
        /// <returns></returns>
        public virtual SnakeAttribute.SnakePathway GetNextPathway(PlayingMap map) => LastPathway;

        #endregion
    }

    /// <summary>
    /// Шаблон для умной змейки
    /// </summary>
    public abstract class SmartSnakeBase : SnakeBase //TODO: написать шаблон умной змейки // А надо ли?
    {
        /// <summary>
        /// "Генны" змейки
        /// </summary>
        protected SnakeAttribute.SnakeGenes Genes;
    }
}
