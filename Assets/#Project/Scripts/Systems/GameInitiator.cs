using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private DeckSystem deckSystem;
    [SerializeField] private CardHoverSystem cardHoverSystem;
    private GameplayEngine gameplayEngine;


    [Header("Data :")]
    private GridData gridData;
    

    [Header("UI :")]
    [SerializeField] private Canvas canvas;


    [Header("View :")]
    // Grid :
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GridView gridView;
    [SerializeField] private TileBase basicTile;
    [SerializeField] private TileBase highlightTile;
    [SerializeField] private TileBase player;
    [SerializeField] private Vector2Int playerStartingCell;
    [SerializeField] private TileBase enemy;
    [SerializeField] private Vector2Int enemyStartingCell;
    private Tilemap[] tilemaps;

    // Cards :
    [SerializeField] private HandView handView;
    [SerializeField] private CardView hoveredCardViewPrefab;


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
        gridView.Initialize(gridData, basicTile, highlightTile, player, playerStartingCell, enemy, enemyStartingCell);
        handView.Initialize();

        // --- Controllers ---
        gridSystem.Initialize(mainCamera, gridView, gridData, actions);
        gridSystem.gameObject.SetActive(true); // Forcing the OnEnable() of the GridControl to access inputs

        // --- Systems ---
        gameplayEngine = GameplayEngine.Instance;
        deckSystem.Initialize();
        cardHoverSystem.Initialize(hoveredCardViewPrefab);
        gameplayEngine.Initialize(gridData);
            // later : register all systems viar RegisterSystem(new ...System()) method
        gameplayEngine.RegisterSystem(gridSystem);

        // --- Game Manager ---
        gameManager.Initialize(gameplayEngine);

        // --- will have to init camera too but rn it doesnt need args ---
    }

    private void InstantiateGameObjects()
    {
        mainCamera = Instantiate(mainCamera, new Vector3(0, 0, -10), Quaternion.identity);
        
        gameplayEngine = new GameObject("GameplayEngine").AddComponent<GameplayEngine>();
        gameManager = Instantiate(gameManager);
        
        gridView = Instantiate(gridView);
        gridView.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 10f;
        gridSystem = Instantiate(gridSystem);

        handView = Instantiate(handView, canvas.transform, false);
        deckSystem = Instantiate(deckSystem);


        hoveredCardViewPrefab = Instantiate(hoveredCardViewPrefab, canvas.transform, false);
        cardHoverSystem = Instantiate(cardHoverSystem);
    }
    #endregion
}
