using Map;
using Snake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RandPathwaySnake : SnakeBase
{
    public override SnakeAttribute.SnakePathway GetNextPathway(PlayingMap map)
    {
        SnakeAttribute.SnakePathway path = LastPathway;
        bool correctPathway = false;

        while (!correctPathway)
        {
            int randPathNum = Random.Range(0, 4);

            if (randPathNum == 0 && LastPathway != SnakeAttribute.SnakePathway.Down)
            {
                path = SnakeAttribute.SnakePathway.Up;
                correctPathway = true;
            }

            if (randPathNum == 1 && LastPathway != SnakeAttribute.SnakePathway.Left)
            {
                path = SnakeAttribute.SnakePathway.Right;
                correctPathway = true;
            }

            if (randPathNum == 2 && LastPathway != SnakeAttribute.SnakePathway.Up)
            {
                path = SnakeAttribute.SnakePathway.Down;
                correctPathway = true;
            }

            if (randPathNum == 3 && LastPathway != SnakeAttribute.SnakePathway.Right)
            {
                path = SnakeAttribute.SnakePathway.Left;
                correctPathway = true;
            }
        }

        LastPathway = path;
        return path;
    }


}
