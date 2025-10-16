using UnityEngine;
using UnityEngine.Tilemaps;

public class GameInitializer : MonoBehaviour
{
    private static GameInitializer instance;
    public static GameInitializer Instance
    {
        get
        {
            return instance;
        }
    }

    // [Header("Game data :")] -> to do

    [Header("Game manager :")]
    [SerializeField] private GameManager gameManager;

    [Header("Grid creation data :")]
    [SerializeField] GridCreationSystem grid;
    [SerializeField] private GridLayout gridLayout;
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap tempTilemap;
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    [SerializeField] private GridGenerator gridGenerator;

    #region Unity Events
    private void Start()
    {
        InstatiateObjects();
        InitializeObjects();

        gridGenerator.GenerateGrid();

        Destroy(gameObject);
    }
    #endregion

    #region Custom methods
    private void InstatiateObjects()
    {
        // grid = Instantiate(grid);
        // mainTilemap = Instantiate(mainTilemap);
        // tempTilemap = Instantiate(tempTilemap);
        gridGenerator = Instantiate(gridGenerator);
    }
    private void InitializeObjects()
    {
        // grid.Initialize(gridLayout, mainTilemap, tempTilemap, tilePrefab, rows, columns);
        gridGenerator.Initialize(rows, columns, tilePrefab);
    }
    #endregion
}
