﻿using Logic;
using Map;

namespace Situations
{
    class Dead : ICollisionWithBarrier
    {
        public void OnCollision(PlayingMapAttributes.Snake snake, PlayingMap currentMap, PlayingMap previousMap)
        {
            snake.Cordinates.RemoveAt(0);
            snake.IsAlive = false;
        }
    }
}