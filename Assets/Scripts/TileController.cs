using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class TileController : MonoBehaviour {

    public TileTypes TileType;

    void OnTriggerStay2D(Collider2D other)
    {
        var floorBounds = gameObject.GetComponent<BoxCollider2D>().bounds;
        var playerBounds = other.bounds;

        if (floorBounds.ContainsBounds(playerBounds))
        {
            SceneManager.LoadScene("winner");
        }

    }
}

public enum TileTypes
{
    Floor,
    Exit,
    Key,
    Door,
    Ground,
    Ground2
}
