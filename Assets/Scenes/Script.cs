using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Script : MonoBehaviour
{
    public Tilemap Tile;
    public Vector3Int Position;
    public TileBase[] Base;
    public Grid grid;

    public float scale = 1;
    public int side = 1;

    private bool Ok = true;
    private const float XInFunc = 54;

    void Start()
    {

    }

    void Update()
    {
        int s = side / 2;
        Position = new Vector3Int();
        for (int i = -s; i < s; i++)
            for (int j = -s + 1; j <= s; j++)
            {
                Position.x = i;
                Position.y = -j;
                Tile.SetTile(Position, Base[0]);
            }
        scale = XInFunc / side;
        grid.transform.localScale = new Vector3(scale, scale);
    }
}
