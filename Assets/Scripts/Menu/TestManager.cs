using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

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
        GameLogic = new StandartLogic(4);
        Map = GameLogic.GetCurrentPlayingMap();

        SimbolMap = new char[Map.sideSize, Map.sideSize];

        FillMapEmptyObjects();
        InsertElements();
        ShowMapConsole();
        ShowMapTexture();
    }

    int timeNum = 0;
    private void Update()
    {
        if (timeNum < 500)
        {
            Thread.Sleep(250);
            Map = GameLogic.GetNextPlayingMap();
            FillMapEmptyObjects();
            InsertElements();
            ShowMapConsole();
            ShowMapTexture();
            timeNum++;
        }
    }

    private void FillMapEmptyObjects()
    {
        for(int i = 0; i < Map.sideSize; i++)
            for(int j = 0; j < Map.sideSize; j++)
            {
                SimbolMap[i, j] = '_';
            }      
    }

    private void ShowMapConsole()
    {
        string map = "";
        for (int i = 0; i < Map.sideSize; i++)
        {
            for (int j = 0; j < Map.sideSize; j++)
                map += SimbolMap[j,i];
            map += "\n";
        }
        print(map);
    }

    GameObject mapObject;
    SpriteRenderer spriteRenderer;
    private void ShowMapTexture()
    {
        Texture2D txr2d = new Texture2D(Map.sideSize, Map.sideSize);
        Destroy(mapObject);
        Destroy(spriteRenderer);
        mapObject = new GameObject("Map");
        spriteRenderer = mapObject.AddComponent<SpriteRenderer>();

        for (int i = 0; i < Map.sideSize; i++)
            for (int j = 0; j < Map.sideSize; j++)
               txr2d.SetPixel(j, Map.sideSize - 1 - i, GetColor(SimbolMap[j, i]));

        txr2d.filterMode = FilterMode.Point;
        txr2d.Apply();
        Rect rect = new Rect(0, 0, Map.sideSize, Map.sideSize);
        Sprite sprite = Sprite.Create(txr2d, rect, Vector2.one / 2.0f, 1, 0, SpriteMeshType.FullRect);
        spriteRenderer.sprite = sprite;
    }

    private Color GetColor (char ch)
    {
        switch (ch)
        {
            case 'F':
                return Color.red;

            case 'R':
                return Color.green;

            case 'P':
                return Color.blue;

            case 'd':
                return Color.gray;
        }

        return Color.white;
    }

    private void InsertElements()
    {
        foreach (var f in Map.Food.FoodCordinates)
            SimbolMap[f.X, f.Y] = 'F';

        foreach (var b in Map.Barriers)
            SimbolMap[b.X, b.Y] = 'B';

        foreach (var s in Map.Snake)
            foreach (var c in s.Cordinates)
            SimbolMap[c.X, c.Y] = (s.isAlive)? s.Name[0] : 'd';
    }

}
