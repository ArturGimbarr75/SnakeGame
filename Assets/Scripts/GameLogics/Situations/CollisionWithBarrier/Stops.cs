using Logic;
using Map;

namespace Situations
{
    class Stops : ICollisionWithBarrier
    {
        public void OnColision(PlayingMapAttributes.Snake snake, PlayingMap mapCopy)
        {
            if (GameLogicBase.CollisionWithBarriers(snake.Cordinates[0], mapCopy)
                || GameLogicBase.CollisionWithDeadSnakes(snake.Cordinates[0], mapCopy))
                snake.Cordinates.RemoveAt(0);
        }
    }
}
