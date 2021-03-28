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
    /// Global initializers of game logic
    /// </summary>
    struct GameInits
    {
        /// <summary>
        /// Предикаты окончания игры
        /// Game ending predicates
        /// </summary>
        public static HashSet<GameLogicsAttributes.GameoverPredicates> GameoverPredicates =
            new HashSet<GameLogicsAttributes.GameoverPredicates>()
            { GameLogicsAttributes.GameoverPredicates.DeadAllSnakes};
        /// <summary>
        /// Имена змеек
        /// Snakes' names
        /// </summary>
        public static List<string> SnakeNames = new List<string>();
        /// <summary>
        /// Фабрика змеек
        /// Snake factory
        /// </summary>
        public static ISnakeFactory Assembly = new AssemblySnakeFactory();
        /// <summary>
        /// Размеры карты
        /// Map size
        /// </summary>
        public static int MapSize = 30;
        /// <summary>
        /// Количество еды на карте
        /// Food amount in the map
        /// </summary>
        public static int FoodCount = 20;
        /// <summary>
        /// Оставлять ли мертвые тела
        /// Should you leave dead snake bodies
        /// </summary>
        public static bool LeftDeadSnakeBody = false;
        /// <summary>
        /// Статистика игравших змеек
        /// Statistics of snakes that were in the game
        /// </summary>
        public static List<SnakeStatistics> SnakeStatistics = new List<SnakeStatistics>();
        /// <summary>
        /// Скорость змеек
        /// </summary>
        public static int Speed = 7;
        /// <summary>
        /// Пауза междую шагами змеек
        /// </summary>
        public static float Pause => (float)(0.31 - Speed * 0.03);
        public static SituationsInit.SituationsObjects Situations = SituationsInit.Instance.GetSituationsObjects();
    }
}
