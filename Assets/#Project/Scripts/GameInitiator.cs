using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameInitiator : MonoBehaviour
{
    // Singleton pattern
    private GameInitiator instance;
    public GameInitiator Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("Settings :")]
    //Implement Game Data SO later
    [SerializeField] private int gridRows;
    [SerializeField] private int gridColumns;


    [Header("Inputs :")]
    [SerializeField] private InputActionAsset actions;


    [Header("Systems :")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GridControl gridControl;
    private GameplayEngine gameplayEngine;


    [Header("Data :")]
    private GridData gridData;


    [Header("View :")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GridView gridView;
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap highlightMap;
    [SerializeField] private TileBase basicTile;


    #region Unity events
    private void Start()
    {
        BuildGame();
        Destroy(gameObject); // Destroys itself after instantiation
    }
    #endregion


    #region Custom methods
    private void BuildGame()
    {
        // --- Data ---
        gridData = new GridData(gridRows, gridColumns);

        // --- Systems ---
        gameplayEngine = new GameplayEngine(gridData);
        // later : register all systems viar RegisterSystem(new ...System()) method
        gameplayEngine.RegisterSystem(gridControl);

        // --- View ---
        gridView.Initiate(gridData, mainTilemap, highlightMap, basicTile);

        // --- Controllers ---
        gridControl.Initiate(mainCamera, gridView, gridData, actions);

        // --- Game Manager ---
        gameManager.Initiate(gameplayEngine);

        // --- will have to init camera too but rn it doesnt need args ---
    }

    private void InstantiateGameObjects()
    {
        mainCamera = Instantiate(mainCamera);
        gridView = Instantiate(gridView);
        gridControl = Instantiate(gridControl);
        gameManager = Instantiate(gameManager);
    }
    #endregion
}
