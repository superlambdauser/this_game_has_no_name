using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGenerator : MonoBehaviour
{
    private int rows, columns;
    private Transform isometricTile;


    #region Unity Events
    private void Start()
    {

    }
    #endregion

    #region Custom methods
    public void Initialize(int rows, int columns, Transform tile)
    {
        this.rows = rows;
        this.columns = columns;
        isometricTile = tile;
    }

    public void GenerateGrid()
    {
        for (int x = rows; x>= 0; x--)
        {
            for (int y = 0; y < columns; y++)
            {
                float xOffset = (x + y) / 2f;
                float yOffset = (x - y) / 4f;
                Transform tile = Instantiate(isometricTile, new Vector3(xOffset, yOffset, 0), Quaternion.identity);
                // Debug.Log($"x = {x}, y = {y}, xOff = {xOffset}, yOff = {yOffset}");
            }
        }
    }
    #endregion
}
