using System.Linq;
using Map;


namespace Situations
{
    class IncreaseBody : ICollisionWithFood
    {
        public void OnCollision(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap)
        {
            var tailPos = previousMap.Snake.Find(x => x == snake).Cordinates.Last();
            snake.Cordinates.Add(tailPos);
        }
    }
}
