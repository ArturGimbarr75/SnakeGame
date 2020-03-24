using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Snake;
using Map;

public class PlayerArrows : SnakeBase
{
    public override SnakeAttribute.SnakePathway GetNextPathway(PlayingMap map)
    {
        SnakeAttribute.SnakePathway path = LastPathway;

        if (Input.GetKey(KeyCode.UpArrow) && LastPathway != SnakeAttribute.SnakePathway.Down)
            path = SnakeAttribute.SnakePathway.Up;
         
        if (Input.GetKey(KeyCode.RightArrow) && LastPathway != SnakeAttribute.SnakePathway.Left)
            path = SnakeAttribute.SnakePathway.Right;
        
        if (Input.GetKey(KeyCode.DownArrow) && LastPathway != SnakeAttribute.SnakePathway.Up)
            path = SnakeAttribute.SnakePathway.Down;

        if (Input.GetKey(KeyCode.LeftArrow) && LastPathway != SnakeAttribute.SnakePathway.Right)
            path = SnakeAttribute.SnakePathway.Left;

        LastPathway = path;
        return path;
    }

}
