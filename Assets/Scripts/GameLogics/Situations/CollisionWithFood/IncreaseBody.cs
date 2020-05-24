using Logic;
using Map;


namespace Situations
{
    class IncreaseBody : ICollisionWithFood
    {
        public bool OnCollision(PlayingMapAttributes.Snake snake, PlayingMap mapCopy)
        {
            return GameLogicBase.CollisionWithFood(snake.Cordinates[0], mapCopy);
        }
    }
}
