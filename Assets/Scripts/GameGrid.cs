using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameGrid : MonoBehaviour
{
    public Tilemap ghostLadderTilemap,destroyTilemap;
    private void Start()
    {
        destroyTilemap.color = Color.white;
        ghostLadderTilemap.color = Color.clear;
    }
}
