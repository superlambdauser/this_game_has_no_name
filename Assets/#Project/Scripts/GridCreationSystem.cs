using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCreationSystem : MonoBehaviour
{
        // Singleton pattern
    private static GridCreationSystem instance;
    public static GridCreationSystem Instance
    {
        get
        {
            return instance;
        }
    }

    public int rows, columns;
    private GridLayout gridLayout;
    private Tilemap mainTilemap;
    private Tilemap tempTilemap;
    public TileBase tilePrefab;
    private List<TileBase> tileBasesArray;


    #region Unity Methods
    private void Awake()
    {
        if (instance == null) // Is there already an instance of this object ?
        {
            DontDestroyOnLoad(gameObject); // Keeping the same object between scenes
            instance = this; // Instantiate the gameManager as one and only gameManager
        }
        else if (instance != this) // If so, destroy unnecessary gameManager
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateGrid();
    }
    #endregion


    #region  Custom Methods
    public void Initialize(GridLayout gridLayout, Tilemap mainTilemap, Tilemap tempTilemap, TileBase tilePrefab, int rows, int columns)
    {
        this.gridLayout = gridLayout;
        this.mainTilemap = mainTilemap;
        this.tempTilemap = tempTilemap;
        this.tilePrefab = tilePrefab;
        this.rows = rows;
        this.columns = columns;

        mainTilemap.transform.SetParent(transform);
        tempTilemap.transform.SetParent(transform);
    }

    private void GenerateGrid()
    {
        Debug.Log("Generating grid...");
        for (int x = 0;  x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                mainTilemap.SetTile(tilePosition, tilePrefab);
            }
        }
    }
    #endregion
}
