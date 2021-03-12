using Logic;
using Map;
using System.Linq;

namespace Situations
{
    class Stops : ICollisionWithBarrier
    {
        public void OnCollision(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap)
        {
            var tailPos = previousMap.Snake.Find(x => x == snake).Cordinates.Last();
            snake.Cordinates.Add(tailPos);
            snake.Cordinates.RemoveAt(0);
        }
    }
}
