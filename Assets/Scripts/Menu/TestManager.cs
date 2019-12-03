using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

using Logic;
using Map;
using Assets.Scripts.GameLogics;

public class TestManager : MonoBehaviour
{

    private GameLogicBase GameLogic;

    private PlayingMap Map;

    private string[,] SimbolMap;

    // Start is called before the first frame update
    void Start()
    {
        List<string> names = new List<string>()
        {
            //nameof(PlayerArrows),
            //nameof(PlayerWASD),
            //nameof(RandPathwaySnake),
            //nameof(FollowFoodSnake),
            nameof(Adam),
        };
        int mapSize = 50;
        int foodCount = 15;

        GameLogic = new StandartLogic(names, new AssemblySnakeFactory(), mapSize, foodCount, true);
        Map = GameLogic.GetCurrentPlayingMap();

        SimbolMap = new string[Map.sideSize, Map.sideSize];

        FillMapEmptyObjects();
        InsertElements();
        //ShowMapConsole();
        ShowMapTexture();
    }

    int timeNum = 0;
    private void Update()
    {
        if (timeNum < 5000)
        {
            Thread.Sleep(100);
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
                SimbolMap[i, j] = "_";
            }      
    }

    private void ShowMapConsole()
    {
        string map = "";
        for (int i = 0; i < Map.sideSize; i++)
        {
            for (int j = 0; j < Map.sideSize; j++)
                map += SimbolMap[j,i][0];
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

    private Color GetColor (string name)
    {
        switch (name)
        {
            case nameof(PlayerArrows):
                return Color.blue;

            case nameof(PlayerWASD):
                return Color.green;

            case nameof(RandPathwaySnake):
                return Color.yellow;

            case nameof(FollowFoodSnake):
                return Color.cyan;

            case nameof(Adam):
                return Color.magenta;

            case "F":
                return Color.red;

            case "d":
                return Color.gray;

            case "B":
                return Color.black;
        }

        return Color.white;
    }

    private void InsertElements()
    {
        foreach (var f in Map.Food.FoodCordinates)
            SimbolMap[f.X, f.Y] = "F";

        foreach (var b in Map.Barriers)
            SimbolMap[b.X, b.Y] = "B";

        foreach (var s in Map.Snake)
            foreach (var c in s.Cordinates)
            SimbolMap[c.X, c.Y] = (s.isAlive)? s.Name : "d";
    }

}
