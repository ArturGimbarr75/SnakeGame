using Logic;
using Map;

namespace Situations
{
    class SnakeDead : ICollisionWithSnake
    {
        public void OnCollision(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap)
        {
            snake.Cordinates.RemoveAt(0);
            snake.IsAlive = false;
        }
    }
}
