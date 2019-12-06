using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Snake;
using Map;

public class PlayerIJKL : SnakeBase
{
    public override SnakeAttribute.SnakePathway GetNextPathway(PlayingMap map)
    {
        SnakeAttribute.SnakePathway path = LastPathway;

        if (Input.GetKeyDown(KeyCode.I) && LastPathway != SnakeAttribute.SnakePathway.Down)
            path = SnakeAttribute.SnakePathway.Up;

        if (Input.GetKeyDown(KeyCode.L) && LastPathway != SnakeAttribute.SnakePathway.Left)
            path = SnakeAttribute.SnakePathway.Right;

        if (Input.GetKeyDown(KeyCode.K) && LastPathway != SnakeAttribute.SnakePathway.Up)
            path = SnakeAttribute.SnakePathway.Down;

        if (Input.GetKeyDown(KeyCode.J) && LastPathway != SnakeAttribute.SnakePathway.Right)
            path = SnakeAttribute.SnakePathway.Left;

        LastPathway = path;
        return path;
    }

}
