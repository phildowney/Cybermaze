﻿using Assets.Scripts;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public TileTypes TileType;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        GlobalData.SelectedTileType = TileType;
    }
}
