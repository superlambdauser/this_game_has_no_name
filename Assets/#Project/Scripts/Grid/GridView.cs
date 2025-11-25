using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
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

        mainTilemap.transform.SetParent(transform);
        highlightMap.transform.SetParent(transform);

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
            Debug.Log($"Clearing highlightMap at Initiate. Tile count before: {CountTiles(highlightMap)}");
            highlightMap.ClearAllTiles();
            Debug.Log($"Tile count after ClearAllTiles: {CountTiles(highlightMap)}");
        }

        highlightMap.SetTile(new Vector3Int(0, 0, 0), highlightTile);
        highlightMap.SetColor(new Vector3Int(0, 0, 0), Color.red);
    }

    private int CountTiles(Tilemap tilemap)
    {
        if (tilemap == null) return 0;

        int count = 0;

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] block = tilemap.GetTilesBlock(bounds);

        foreach (TileBase cell in block) if (cell != null) count++;

        return count;
    }

    public void Highlight(IEnumerable<Vector2Int> cellsPositions, Color color) // Ideally, add a IEnumerator of Vector2Int argument that represents the area of action and contains all the cells inside the area
    {
        Debug.Log($"Highlight() called! HighlightMap: {highlightMap != null}, HighlightTile: {highlightTile != null}");

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

        Debug.Log($"Clearing highlightMap (before count: {CountTiles(highlightMap)})");
        highlightMap.ClearAllTiles();
        Debug.Log($"After ClearAllTiles (count: {CountTiles(highlightMap)})");

        int placed = 0;
        foreach (Vector2Int cellPosition in cellsPositions)
        {
            Debug.Log($"Highlighting cell {cellPosition}");
            Vector3Int pos = new Vector3Int(cellPosition.x, cellPosition.y, 0);
            highlightMap.SetTile(pos, highlightTile); // Set a visible tile
            // highlightMap.SetColor(pos, color);       // Then color it
            placed++;
        }

        Debug.Log($"Highlight placed tiles: {placed}");
    }
}
