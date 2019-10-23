using Map;
using Snake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrows : SnakeBase
{
    public override SnakeAttribute.SnakePathway GetNextPathway(PlayingMap map)
    {
        SnakeAttribute.SnakePathway path = LastPathway;

        if (Input.GetKeyDown(KeyCode.UpArrow) && LastPathway != SnakeAttribute.SnakePathway.Down)
            path = SnakeAttribute.SnakePathway.Up;
         
        if (Input.GetKeyDown(KeyCode.RightArrow) && LastPathway != SnakeAttribute.SnakePathway.Left)
            path = SnakeAttribute.SnakePathway.Right;
        
        if (Input.GetKeyDown(KeyCode.DownArrow) && LastPathway != SnakeAttribute.SnakePathway.Up)
            path = SnakeAttribute.SnakePathway.Down;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && LastPathway != SnakeAttribute.SnakePathway.Right)
            path = SnakeAttribute.SnakePathway.Left;

        LastPathway = path;
        return path;
    }

}
