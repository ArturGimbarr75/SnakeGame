using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.GameLogics;
using Logic;
using Snake;

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
        public static HashSet<GameLogicsAttributes.GameoverPredicates> GameoverPredicates =
            new HashSet<GameLogicsAttributes.GameoverPredicates>()
            { GameLogicsAttributes.GameoverPredicates.DeadAllSnakes};
        /// <summary>
        /// Имена змеек
        /// </summary>
        public static List<string> SnakeNames = new List<string>();
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
        /// <summary>
        /// Статистика игравших змеек
        /// </summary>
        public static List<SnakeStatistics> SnakeStatistics = new List<SnakeStatistics>();
    }
}
