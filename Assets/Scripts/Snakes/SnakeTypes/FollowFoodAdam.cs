using System;
using System.Collections.Generic;

using Snake;
using Map;

class FollowFoodAdam : SmartSnakeBase
{
    private SnakeAttribute.Cordinates nearestFoodCor;
    private double minDistance;
    private int XFactor, YFactor;

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

    public override SnakeAttribute.SnakePathway GetNextPathway(PlayingMap map)
    {
        foreach (var path in (SnakeAttribute.SnakePathway[])(Enum.GetValues(typeof(SnakeAttribute.SnakePathway))))
            SnakePathwaysWeights[path] = 0;

        foreach (var path in (SnakeAttribute.SnakePathway[])(Enum.GetValues(typeof(SnakeAttribute.SnakePathway))))
            for (int xFactor = -1; xFactor <= 1; xFactor++)
                for (int yFactor = -1; yFactor <= 1; yFactor++)
                    CheckOtherSide(map, xFactor, yFactor, path);

        // Поиск самого маленького возможного веса, чтобы не допустить похода под себя =D
        // Finding minimum weight, so that snake doesn't step under itself
        int minWeight =
            Math.Abs(8 * SnakeAttribute.SnakeGenes.MinGenesValue * Genes.FoodGenes.Count * Genes.FoodGenes.Count) * 2;

        switch (LastPathway)
        {
            case SnakeAttribute.SnakePathway.Up:
                SnakePathwaysWeights[SnakeAttribute.SnakePathway.Down] -= minWeight;
                break;

            case SnakeAttribute.SnakePathway.Down:
                SnakePathwaysWeights[SnakeAttribute.SnakePathway.Up] -= minWeight;
                break;

            case SnakeAttribute.SnakePathway.Right:
                SnakePathwaysWeights[SnakeAttribute.SnakePathway.Left] -= minWeight;
                break;

            case SnakeAttribute.SnakePathway.Left:
                SnakePathwaysWeights[SnakeAttribute.SnakePathway.Right] -= minWeight;
                break;
        }

        SnakePathwaysWeights[GetNearPathwayToFood(map)] += 20;
        var paths = FindMaxWeights();
        LastPathway = paths[UnityEngine.Random.Range(0, paths.Count - 1)];
        return LastPathway;
    }

    private SnakeAttribute.SnakePathway GetNearPathwayToFood(PlayingMap map)
    {
        SnakeAttribute.SnakePathway path = LastPathway;
        minDistance = Math.Pow(map.sideSize, 2);

        // Смотрим все проходы за стены
        // Checking all pather beyond walls
        for (int xFactor = -1; xFactor <= 1; xFactor++)
            for (int yFactor = -1; yFactor <= 1; yFactor++)
                CheckOtherSide(map, xFactor, yFactor);

        List<SnakeAttribute.SnakePathway> correctPathways = new List<SnakeAttribute.SnakePathway>();

        // Выбираем направления
        // Choosing direction
        if (Head.Y < nearestFoodCor.Y + map.sideSize * YFactor)
            correctPathways.Add(SnakeAttribute.SnakePathway.Down);
        else
            correctPathways.Add(SnakeAttribute.SnakePathway.Up);

        if (Head.X < nearestFoodCor.X + map.sideSize * XFactor)
            correctPathways.Add(SnakeAttribute.SnakePathway.Right);
        else
            correctPathways.Add(SnakeAttribute.SnakePathway.Left);

        bool isLongestPathVertical =
            (Math.Abs(Head.X - nearestFoodCor.X + map.sideSize * XFactor)
            > Math.Abs(Head.Y - nearestFoodCor.Y + map.sideSize * YFactor))
            ? false : true;

        // Смотрим не пойдет ли змейка в себя
        // Checking if snake collides with itself
        foreach (var pathway in correctPathways)
        {
            if (pathway == SnakeAttribute.SnakePathway.Up && LastPathway == SnakeAttribute.SnakePathway.Down)
            {
                correctPathways.Remove(pathway);
                break;
            }
            if (pathway == SnakeAttribute.SnakePathway.Right && LastPathway == SnakeAttribute.SnakePathway.Left)
            {
                correctPathways.Remove(pathway);
                break;
            }
            if (pathway == SnakeAttribute.SnakePathway.Down && LastPathway == SnakeAttribute.SnakePathway.Up)
            {
                correctPathways.Remove(pathway);
                break;
            }
            if (pathway == SnakeAttribute.SnakePathway.Left && LastPathway == SnakeAttribute.SnakePathway.Right)
            {
                correctPathways.Remove(pathway);
                break;
            }
        }

        // Выбераем окончательный путь
        // Coosing final path
        if (correctPathways.Count > 1)
        {
            foreach (var pathway in correctPathways)
            {
                if (isLongestPathVertical)
                {
                    if (pathway == SnakeAttribute.SnakePathway.Down || pathway == SnakeAttribute.SnakePathway.Up)
                    {
                        path = pathway;
                        break;
                    }
                    break;
                }
                path = pathway;
            }
        }
        else
        {
            path = correctPathways[0];
        }

        LastPathway = path;
        return path;
    }

    /// <summary>
    /// Метод просчитывает растояние до еды,
    /// если идти через стены или не обязательно
    /// Method counts distance to the food, should you go through walls or not
    /// </summary>
    /// <param name="map">Карта/Map</param>
    /// <param name="xFactor">Множитель кординаты X/Multiplier of X coordinate</param>
    /// <param name="yFactor">Множитель кординаты Y/Multiplier of Y coordinate</param>
    private void CheckOtherSide(PlayingMap map, int xFactor, int yFactor)
    {
        // Ищем ближайшую еду
        foreach (var food in map.Food.FoodCordinates)
        {
            double dist = Math.Sqrt(
                Math.Pow(Head.X - (food.X + map.sideSize * xFactor), 2)
                + Math.Pow(Head.Y - (food.Y + map.sideSize * yFactor), 2));

            if (dist < minDistance)
            {
                minDistance = dist;
                nearestFoodCor = food;
                XFactor = xFactor;
                YFactor = yFactor;
            }
        }
    }
}
