using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.GameLogics;
using Logic;

namespace Assets.Scripts.Menu.Attributes
{
    /// <summary>
    /// Глобальные инициализаторы игровой логики
    /// </summary>
    struct GameInits
    {
        /// <summary>
        /// Предикаты окончания игры
        /// </summary>
        public static HashSet<GameLogicsAttributes.GameoverPredicates> gameoverPredicates = new HashSet<GameLogicsAttributes.GameoverPredicates>();
        /// <summary>
        /// Имена змеек
        /// </summary>
        public static List<string> snakeNames = new List<string>();
        /// <summary>
        /// Фабрика змеек
        /// </summary>
        public static ISnakeFactory Assembly = new AssemblySnakeFactory();
        /// <summary>
        /// Размеры карты
        /// </summary>
        public static int MapSize = 30;
        /// <summary>
        /// Количество еды на карте
        /// </summary>
        public static int FoodCount = 20;
        /// <summary>
        /// Оставлять ли мертвые тела
        /// </summary>
        public static bool LeftDeadSnakeBody = false;
    }
}
