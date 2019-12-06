using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

using Logic;
using Map;
using Assets.Scripts.GameLogics;
using Assets.Scripts.DataBase;
using System;
using System.Linq;

public class TestManager : MonoBehaviour
{

    private GameLogicBase GameLogic;

    private PlayingMap Map;

    private string[,] SimbolMap;

    private static System.Random random = new System.Random();
    /*public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }*/
    void Start()
    {
        /*SnakesTable t = new SnakesTable();
        var name = RandomString(5);*/
        List<string> names = new List<string>()
        {
            nameof(PlayerArrows),
            nameof(PlayerIJKL),
            /*nameof(PlayerWASD),
            nameof(RandPathwaySnake),
            nameof(FollowFoodSnake),*/
            nameof(Adam),
        };
        int mapSize = 50;
        int foodCount = 15;

        GameLogic = new StandartLogic(new HashSet<GameLogicsAttributes.GameoverPredicates>
        { /*GameLogicsAttributes.GameoverPredicates.Achieved30Cels,*/ GameLogicsAttributes.GameoverPredicates.LeftOneAliveSnake },
            names, new AssemblySnakeFactory(), mapSize, foodCount, true);
        Map = GameLogic.GetCurrentPlayingMap();

        SimbolMap = new string[Map.sideSize, Map.sideSize];

        FillMapEmptyObjects();
        InsertElements();
        //ShowMapConsole();
        ShowMapTexture();
    }

    bool showOnes = true;
    private void Update()
    {
        if (!GameLogic.IsGameEnded())
        {
            Thread.Sleep(100);
            Map = GameLogic.GetNextPlayingMap();
            FillMapEmptyObjects();
            InsertElements();
            //ShowMapConsole();
            ShowMapTexture();
        }
        else if (showOnes)
        {
            showOnes = false;
            Map = GameLogic.GetNextPlayingMap();
            string info = "";
            foreach (var s in Map.Snake)
            {
                info += s.Name + " -> " + s.SnakeStatistics + "\n";
            }
            Debug.Log(info);
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

            case nameof(PlayerIJKL):
                return Color.HSVToRGB(0.1f, 0.5f, 0.9f);

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
