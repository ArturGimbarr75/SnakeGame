﻿using Map;
using Snake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWASD : SnakeBase
{
    public override SnakeAttribute.SnakePathway GetNextPathway(PlayingMap map)
    {
        SnakeAttribute.SnakePathway path = LastPathway;

        if (Input.GetKey(KeyCode.W) && LastPathway != SnakeAttribute.SnakePathway.Down)
            path = SnakeAttribute.SnakePathway.Up;

        if (Input.GetKey(KeyCode.D) && LastPathway != SnakeAttribute.SnakePathway.Left)
            path = SnakeAttribute.SnakePathway.Right;

        if (Input.GetKey(KeyCode.S) && LastPathway != SnakeAttribute.SnakePathway.Up)
            path = SnakeAttribute.SnakePathway.Down;

        if (Input.GetKey(KeyCode.A) && LastPathway != SnakeAttribute.SnakePathway.Right)
            path = SnakeAttribute.SnakePathway.Left;

        LastPathway = path;
        return path;
    }

}
