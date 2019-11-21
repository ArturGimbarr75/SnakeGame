using Map;
using Snake;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FollowFoodSnake : SnakeBase
{
    private SnakeAttribute.Cordinates nearestFoodCor;
    private double minDistance;
    private int XFactor, YFactor;

    public override SnakeAttribute.SnakePathway GetNextPathway(PlayingMap map)
    {
        SnakeAttribute.SnakePathway path = LastPathway;
        minDistance = Math.Pow(map.sideSize, 2);

        // Смотрим все проходы за стены
        for (int xFactor = -1; xFactor <= 1; xFactor++)
            for (int yFactor = -1; yFactor <= 1; yFactor++)
                CheckOtherSide (map, xFactor, yFactor);

        List<SnakeAttribute.SnakePathway> correctPathways = new List<SnakeAttribute.SnakePathway>();

        // Выбираем направления
        if (Head.Y < nearestFoodCor.Y + map.sideSize * YFactor)
            correctPathways.Add (SnakeAttribute.SnakePathway.Down);
        else
            correctPathways.Add(SnakeAttribute.SnakePathway.Up);

        if (Head.X < nearestFoodCor.X + map.sideSize * XFactor)
            correctPathways.Add(SnakeAttribute.SnakePathway.Right);
        else
            correctPathways.Add(SnakeAttribute.SnakePathway.Left);

        bool isLongestPathVertical =
            ( Math.Abs(Head.X - nearestFoodCor.X + map.sideSize * XFactor)
            > Math.Abs(Head.Y - nearestFoodCor.Y + map.sideSize * YFactor))
            ? false : true;

        // Смотрим не пойдет ли змейка в себя
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
    /// </summary>
    /// <param name="map">Карта</param>
    /// <param name="xFactor">Множитель кординаты X</param>
    /// <param name="yFactor">Множитель кординаты Y</param>
    private void CheckOtherSide (PlayingMap map, int xFactor, int yFactor)
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
