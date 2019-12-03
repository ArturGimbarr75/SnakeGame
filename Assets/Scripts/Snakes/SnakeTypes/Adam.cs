using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Snake;

class Adam : SmartSnakeBase
{
    protected override void SetSnakeGenes()
    {
        Dictionary<SnakeAttribute.SnakePathway, int[,]> FoodGenes = new Dictionary<SnakeAttribute.SnakePathway, int[,]>();
        Dictionary<SnakeAttribute.SnakePathway, int[,]> BarrierGenes = new Dictionary<SnakeAttribute.SnakePathway, int[,]>();

        int[,]
            upFood = new int[,]
            {
                    { 4,  5,  6,  7,  8, 10,  8,  7,  6,  5,  4 },
                    { 4,  4,  6,  8, 12, 15, 12,  8,  6,  4,  4 },
                    { 3,  3, 10, 10, 12, 20, 12, 10, 10,  3,  3 },
                    { 2,  2, 10, 10, 13, 25, 13, 10, 10,  2,  2 },
                    { 1,  2,  8, 12, 15, 35, 15, 12,  8,  2,  1 },
                    { 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0 },
                    { 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0 },
                    { 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0 },
                    { 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0 },
                    { 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0 },
                    { 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0 }
            },

            upBarrier = new int[,]
            {
                    { 0,  0,  0,  0,  0,   0,  0,  0,  0,  0,  0 },
                    { 0,  0,  0,  0,  0,   0,  0,  0,  0,  0,  0 },
                    { 0,  0, -1, -1, -1,  -2, -1, -1, -1,  0,  0 },
                    { 0,  0, -1, -2, -3,  -7, -3, -2, -1,  0,  0 },
                    { 0,  0, -1, -3, -5, -99, -5, -3, -1,  0,  0 },
                    { 0,  0,  1,  2, 10,   0, 10,  2,  1,  0,  0 },
                    { 0,  0,  1,  1,  5,  10,  5,  1,  1,  0,  0 },
                    { 0,  0,  1,  0,  1,   2,  1,  0,  1,  0,  0 },
                    { 0,  0,  1,  0,  0,   0,  0,  0,  1,  0,  0 },
                    { 0,  0,  0,  0,  0,   0,  0,  0,  0,  0,  0 },
                    { 0,  0,  0,  0,  0,   0,  0,  0,  0,  0,  0 }
            },

            rightFood = new int[11, 11], downFood = new int[11, 11], leftFood = new int[11, 11],
            rightBarrier = new int[11, 11], downBarrier = new int[11, 11], leftBarrier = new int[11, 11];

        for (int i = 0; i < 11; i++)
            for (int j = 0; j < 11; j++)
            {
                leftBarrier[i, j] = upBarrier[j, i];
                leftFood[i, j] = upFood[j, i];

                downBarrier[i, j] = upBarrier[10 - i, j];
                downFood[i, j] = upFood[10 - i, j];

                rightBarrier[i, j] = leftBarrier[i, 10 - j];
                rightFood[i, j] = leftFood[i, 10 - j];
            }

        FoodGenes.Add(SnakeAttribute.SnakePathway.Up, upFood);
        FoodGenes.Add(SnakeAttribute.SnakePathway.Down, downFood);
        FoodGenes.Add(SnakeAttribute.SnakePathway.Right, rightFood);
        FoodGenes.Add(SnakeAttribute.SnakePathway.Left, leftFood);

        BarrierGenes.Add(SnakeAttribute.SnakePathway.Up, upBarrier);
        BarrierGenes.Add(SnakeAttribute.SnakePathway.Down, downBarrier);
        BarrierGenes.Add(SnakeAttribute.SnakePathway.Right, rightBarrier);
        BarrierGenes.Add(SnakeAttribute.SnakePathway.Left, leftBarrier);

        Genes = new SnakeAttribute.SnakeGenes(FoodGenes, BarrierGenes);
    }
}
