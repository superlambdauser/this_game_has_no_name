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

    public int width, height;
    [SerializeField] private GridLayout gridLayout;
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap tempTilemap;
    public TileBase tilePrefab;
    private List<TileBase> tileBasesArray;


    #region Unity Methods

    void Awake()
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

    void Start()
    {
        GenerateGrid();
    }

    #endregion


    #region  Custom Methods
    private void GenerateGrid()
    {
        Debug.Log("Generating grid...");
        for (int x = 0;  x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                mainTilemap.SetTile(tilePosition, tilePrefab);
            }
        }
    }
    #endregion


}
