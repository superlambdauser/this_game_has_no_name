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
    private TileBase highlightTile;


    public void Initiate(GridData gridData, Tilemap mainTilemap, Tilemap highlightMap, TileBase tile, TileBase highlightTile)
    {
        this.mainTilemap = mainTilemap;
        this.highlightMap = highlightMap;
        this.tile = tile;
        this.highlightTile = highlightTile;

        if (mainTilemap == null || tile == null)
        {
            Debug.LogError("Tilemap or Tile missing.");
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
        if (highlightMap == null)
        {
            Debug.LogError("HighlightMap missing.");
            return;
        }

        if (highlightTile == null)
        {
            Debug.Log("HighlightTile missing.");
        }

        highlightMap.ClearAllTiles();
        
        foreach (Vector2Int cellPosition in cellsPositions)
        {
            Vector3Int pos = new Vector3Int(cellPosition.x, cellPosition.y, 0);
            highlightMap.SetTile(pos, highlightTile); // Set a visible tile
            highlightMap.SetColor(pos, color);       // Then color it
        }
    }
}
