using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Snake;

namespace Assets.Scripts.GameLogics
{
    class AssemblySnakeFactory : ISnakeFactory
    {
        /// <summary>
        /// Словарь типов змеек
        /// Dictionary of snake types
        /// </summary>
        private Dictionary<string, TypeInfo> snakeTypes;
        /// <summary>
        /// Имена змеек
        /// Snakes' names
        /// </summary>
        private List<string> snakeNames;

        /// <summary>
        /// Конструктор который создает словарь из типов змеек
        /// Constructor which creates dictionary from snakes' types
        /// </summary>
        public AssemblySnakeFactory()
        {
            snakeTypes =
            typeof(AssemblySnakeFactory).Assembly.DefinedTypes
            .Where(t => typeof(SnakeBase).IsAssignableFrom(t) && !t.IsAbstract)
            .ToDictionary(t => t.Name, t => t);

            snakeNames = snakeTypes.Keys.ToList();
        }

        /// <summary>
        /// Создает новый объект змейки по имени
        /// и присваевает ему кординаты.
        /// Creates new snake object based on name and assigns coordinates.
        /// </summary>
        /// <param name="snakeName">Имя змейки/Snake's name</param>
        /// <param name="cordinates">Кондинаты головы и тела/Head and body coordinates</param>
        /// <returns></returns>
        public SnakeBase GetSnakeByName(string snakeName, List<SnakeAttribute.Cordinates> cordinates)
        {
            if (cordinates == null || cordinates.Count == 0)
                throw new ArgumentException(nameof(cordinates),
                    "Cordinates sould be initialised and have at least one element");

            if (snakeTypes.TryGetValue(snakeName, out var snakeType))
            {
                var snake = (SnakeBase)Activator.CreateInstance(snakeType);
                // На всякий случай
                //Just in case of a problem
                snake.SnakeBody.Clear();
                foreach (var c in cordinates)
                    snake.SnakeBody.Add(c);

                return snake;
            }

            throw new NotSupportedException($"Snake type {snakeName} not found.");
        }

        /// <summary>
        /// Возвращает названия доступных змеек.
        /// Returns avalable snakes' names
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllSnakeTypes() => snakeNames;
    }
}
