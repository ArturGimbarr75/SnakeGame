using Logic;
using Map;

namespace Situations
{
    class LeftCurrentSize : ICollisionWithFood
    {
        public bool OnCollision(PlayingMapAttributes.Snake snake, PlayingMap mapCopy)
        {
            if (GameLogicBase.CollisionWithFood(snake.Cordinates[0], mapCopy))
            {
                snake.Cordinates.RemoveAt(snake.Cordinates.Count - 1);
                return true;
            }
            return false;
        }
    }
}
