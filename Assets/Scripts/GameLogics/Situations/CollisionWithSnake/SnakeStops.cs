using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Situations
{
    class SnakeStops : ICollisionWithSnake
    {
        public void OnCollision(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap)
        {
            var tailPos = previousMap.Snake.Find(x => x == snake).Cordinates.Last();
            snake.Cordinates.Add(tailPos);
            snake.Cordinates.RemoveAt(0);
        }
    }
}
