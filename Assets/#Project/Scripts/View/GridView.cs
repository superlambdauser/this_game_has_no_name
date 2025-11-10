using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridView : MonoBehaviour
{
    private Tilemap mainTilemap;
    public Tilemap MainTilemap
    {
        get
        {
            return mainTilemap;
        }
    }
    private Tilemap highlightMap;
    private TileBase tile;
    private TileBase highlightTile; // tbd


    public void Initiate(GridData gridData, Tilemap mainTilemap, Tilemap highlightMap, TileBase tile)
    {
        this.mainTilemap = mainTilemap;
        this.highlightMap = highlightMap;
        this.tile = tile;

        if (mainTilemap == null || tile == null)
        {
            Debug.LogError("GridView is missing Tilemap or Tile assignment!");
            return;
        }


        for (int x = 0; x < gridData.rows; x++)
        {
            for (int y = 0; y < gridData.columns; y++)
            {
                mainTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    public void Highlight(IEnumerable<Vector2Int> cellsPositions, Color color) // Ideally, add a IEnumerator of Vector2Int argument that represents the area of action and contains all the cells inside the area
    {
        highlightMap.ClearAllTiles();

        foreach (Vector2Int cellPosition in cellsPositions)
        {
            highlightMap.SetTile((Vector3Int)cellPosition, tile); //(Vector3Int)nameOfVector2Int is the same as nameOfVector2Int.Vector3Int method
        }
    }
}
