using Logic;
using Map;

namespace Situations
{
    class Destroy : ICollisionWithSnake
    {
        public void OnColision(PlayingMapAttributes.Snake snake, PlayingMap mapCopy, PlayingMap map) // проверяй еще с текущей картой
        {
            var head = snake.Cordinates[0];
            snake.Cordinates.RemoveAt(0);
            if (GameLogicBase.CollisionWithSnakesBody(head, map)
                || GameLogicBase.CollisionWithDeadSnakes(head, map)
                || GameLogicBase.CollisionWithSnakesHead(head, map)
                || GameLogicBase.CollisionWithSnakesTail(head, map))
            {
                snake.IsAlive = false;
            }
            else
                snake.Cordinates.Insert(0, head);
        }
    }
}
