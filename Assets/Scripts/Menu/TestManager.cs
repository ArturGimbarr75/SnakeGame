using System.Collections.Generic;
using UnityEngine;
using System.Threading;

using Logic;
using Map;
using Assets.Scripts.GameLogics;
using Assets.Scripts.Menu.Attributes;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Text;

public class TestManager : MonoBehaviour
{

    private GameLogicBase GameLogic;

    private PlayingMap Map;

    private string[,] SimbolMap;

    void Start()
    {
        GameLogic = new StandartLogic
            (
            GameInits.GameoverPredicates,
            GameInits.SnakeNames,
            new AssemblySnakeFactory(),
            GameInits.MapSize,
            GameInits.FoodCount,
            GameInits.LeftDeadSnakeBody
            );
        Map = GameLogic.GetCurrentPlayingMap();

        SimbolMap = new string[Map.sideSize, Map.sideSize];

        FillMapEmptyObjects();
        InsertElements();
        //ShowMapConsole();
        ShowMapTexture();
    }

    int cccc = 0;
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
        else
        {
            cccc++;
            string fileName = "Bug" + cccc.ToString() +".txt";

            // Check if file already exists. If yes, delete it.     
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Create a new file     
            using (FileStream fs = File.Create(fileName))
            {
                // Add some text to file    
                Byte[] title = new UTF8Encoding(true).GetBytes("New Text File");
                fs.Write(title, 0, title.Length);
                byte[] author = new UTF8Encoding(true).GetBytes("Mahesh Chand");
                fs.Write(author, 0, author.Length);
            }

            Map = GameLogic.GetNextPlayingMap();
            var statistics = GameLogic.GetSnakeStatistics();
            GameInits.SnakeStatistics = statistics;
            SceneManager.LoadScene(3);
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
