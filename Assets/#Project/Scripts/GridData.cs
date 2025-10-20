using UnityEngine;

public class GridData
{
    public int rows, columns;
    private TileData[,] tiles; // 2D array (rectangular)


    public GridData(int rows, int columns)
    {
        this.rows = rows;
        this.columns = columns;
        tiles = new TileData[rows, columns];

        GenerateGrid();
    }
    public void GenerateGrid()
    {
        Debug.Log("Generating grid...");
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                Vector3 position = new(x, y); // Implicitly sets z to 0
                tiles[x, y] = new TileData(position);
            }
        }
        Debug.Log("Grid generated !");
    }
    
    public TileData GetTile(Vector2Int position) // Vector2 represents the position of the tile inside the array -> int
    {
        if (position.x < 0 || position.y < 0 || position.x >= rows || position.y >= columns)
        {
            return null; // out of grid
        }

        return tiles[position.x, position.y];
    }
}
