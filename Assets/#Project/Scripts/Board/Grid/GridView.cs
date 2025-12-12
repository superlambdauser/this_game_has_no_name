using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GridView : Singleton<GridView>
{
    private Tilemap mainTilemap;
    public Tilemap MainTilemap => mainTilemap;
    private Tilemap highlightMap;
    public Tilemap HighlightMap => highlightMap;
    private Tilemap[] tilemaps;
    private TileBase tile;
    private TileBase highlightTile;

    private void Start()
    {
        Debug.Log("Highlight instance ID = " + highlightMap.GetInstanceID());
    }
    public void Initiate(GridData gridData, TileBase tile, TileBase highlightTile)
    {
        tilemaps = GetComponentsInChildren<Tilemap>();

        foreach (var map in tilemaps)
        {
            if (map.gameObject.name.Contains("Main")) mainTilemap = map;
            else if (map.gameObject.name.Contains("Highlight")) highlightMap = map;
        }

        Debug.Log("Tilemap count: " + tilemaps.Length);

        this.tile = tile;
        this.highlightTile = highlightTile;

        Debug.Log($"GridView.Initiate -> mainTilemap: {(mainTilemap != null ? mainTilemap.gameObject.name : "null")}, highlightMap: {(highlightMap != null ? highlightMap.gameObject.name : "null")}, tile: {(tile != null)}, highlightTile: {(highlightTile != null)}");


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

        if (highlightMap != null)
        {
            highlightMap.ClearAllTiles();
        }
    }

    private int CountTiles(Tilemap tilemap) // For debugging 
    {
        if (tilemap == null) return 0;

        int count = 0;

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] block = tilemap.GetTilesBlock(bounds);

        foreach (TileBase cell in block) if (cell != null) count++;

        return count;
    }

    public void Highlight(IEnumerable<Vector2Int> cellsPositions) // Ideally, add a IEnumerator of Vector2Int argument that represents the area of action and contains all the cells inside the area
    {
        // Debug.Log($"Highlight() called! HighlightMap: {highlightMap != null}, HighlightTile: {highlightTile != null}");
        // Debug.Log("Highlight instance ID = " + highlightMap.GetInstanceID());
        if (highlightMap == null)
        {
            Debug.LogError("HighlightMap missing.");
            return;
        }

        if (highlightTile == null)
        {
            Debug.Log("HighlightTile missing.");
            return;
        }

        // Debug.Log($"Clearing highlightMap (before count: {CountTiles(highlightMap)})");
        highlightMap.ClearAllTiles();
        // Debug.Log($"After ClearAllTiles (count: {CountTiles(highlightMap)})");

        foreach (Vector2Int cellPosition in cellsPositions)
        {
            Vector3Int pos = new Vector3Int(cellPosition.x, cellPosition.y, 0);
            highlightMap.SetTile(pos, highlightTile); // Set a visible tile
        }
    }
}
