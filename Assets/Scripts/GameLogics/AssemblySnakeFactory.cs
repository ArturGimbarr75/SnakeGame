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
        /// </summary>
        private Dictionary<string, TypeInfo> snakeTypes;
        /// <summary>
        /// Имена змеек
        /// </summary>
        private List<string> snakeNames;

        /// <summary>
        /// Конструктор который создает словарь из типов змеек
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
        /// </summary>
        /// <param name="snakeName">Имя змейки</param>
        /// <param name="cordinates">Кондинаты головы и тела</param>
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
                snake.SnakeBody.Clear();
                foreach (var c in cordinates)
                    snake.SnakeBody.Add(c);

                return snake;
            }

            throw new NotSupportedException($"Snake type {snakeName} not found.");
        }

        /// <summary>
        /// Возвращает названия доступных змеек.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllSnakeTypes() => snakeNames;
    }
}
