using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Logic;
using Map;

public class TestManager : MonoBehaviour
{

    private GameLogicBase GameLogic;

    private PlayingMap Map;

    private char[,] SimbolMap;

    // Start is called before the first frame update
    void Start()
    {
        GameLogic = new StandartLogic();
        Map = GameLogic.GetCurrentPlayingMap();

        SimbolMap = new char[Map.sideSize, Map.sideSize];

        FillMapEmptyObjects();
        InsertElements();
    }

    private void FillMapEmptyObjects()
    {
        for(int i = 0; i < SimbolMap.Length; i++)
            for(int j = 0; j < SimbolMap.Length; j++)
            {
                SimbolMap[i, j] = '.';
            }      
    }

    private void InsertElements()
    {
        foreach (var f in Map.Food.FoodCordinates)
            SimbolMap[f.X, f.Y] = 'F';

        foreach (var b in Map.Barriers)
            SimbolMap[b.X, b.Y] = 'B';

        foreach (var s in Map.Snake)
            foreach (var c in s.Cordinates)
            SimbolMap[c.X, c.Y] = s.Name[0];
    }

}
