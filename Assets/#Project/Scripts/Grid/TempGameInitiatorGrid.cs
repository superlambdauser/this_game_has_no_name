using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameInitiator : Singleton<GameInitiator>
{
    [Header("Settings :")]
    //Implement Game Data SO later
    [SerializeField] private int gridRows;
    [SerializeField] private int gridColumns;


    [Header("Inputs :")]
    [SerializeField] private InputActionAsset actions;


    [Header("Systems :")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GridSystem gridSystem;
    private GameplayEngine gameplayEngine;


    [Header("Data :")]
    private GridData gridData;

    [Header("UI :")]
    [SerializeField] private Canvas canvas;


    [Header("View :")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GridView gridView;
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap highlightMap;
    [SerializeField] private TileBase basicTile;
    [SerializeField] private TileBase highlightTile;


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
        InstantiateGameObjects();

        // --- Data ---
        gridData = new GridData(gridRows, gridColumns);

        // --- View ---
        gridView.Initiate(gridData, mainTilemap, highlightMap, basicTile, highlightTile);

        // --- Controllers ---
        gridSystem.Initiate(mainCamera, gridView, gridData, actions);
        gridSystem.gameObject.SetActive(true); // Forcing the OnEnable() of the GridControl to access inputs

        // --- Systems ---
        gameplayEngine = new GameplayEngine(gridData);
        // later : register all systems viar RegisterSystem(new ...System()) method
        gameplayEngine.RegisterSystem(gridSystem);

        // --- Game Manager ---
        gameManager.Initiate(gameplayEngine);

        // --- will have to init camera too but rn it doesnt need args ---
    }

    private void InstantiateGameObjects()
    {
        mainCamera = Instantiate(mainCamera, new Vector3(0, 0, -10), Quaternion.identity);
        gridView = Instantiate(gridView);
        gridView.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 10f;
        gridSystem = Instantiate(gridSystem);
        gameManager = Instantiate(gameManager);
        mainTilemap = Instantiate(mainTilemap);
        highlightMap = Instantiate(highlightMap);
    }
    #endregion
}
