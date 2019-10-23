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
        protected GameLogicsAttributes.SnakesForLogic SnakesForLogic;
        protected PlayingMap map;
    }
}