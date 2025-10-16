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
    [SerializeField] private TileBase tilePrefab;
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    #region Unity Events
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
        InstatiateObjects();
        InitializeObjects();
    }
    #endregion

    #region Custom methods
    private void InstatiateObjects()
    {
        grid = Instantiate(grid);
    }
    private void InitializeObjects()
    {
        grid.Initialize(gridLayout, tilePrefab, rows, columns);
    }
    #endregion
}
