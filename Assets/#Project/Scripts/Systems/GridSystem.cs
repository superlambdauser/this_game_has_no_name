using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridSystem : Singleton<GridSystem>, ISystem // bridge between logic & view
{
    private Camera cam;

    private GridView gridView;
    private GridData gridData;

    // Input actions :
    private const string INPUT_ACTION_MAP = "InGame";
    private const string INPUT_SELECT_ACTION = "Select";
    private InputActionAsset actions;
    private InputActionMap actionMap;
    private InputAction selectAction;

    private Vector2Int previousMousePos = new Vector2Int(int.MinValue, int.MinValue); // Making sure value is not insade grid bounds

    // Figures :
    public FigureData[,] FiguresPositions { get; set; } // Array holding info about position of figures on the grid
    private FigureData player;
    // + private EnemyAi ai later on day or never lol

    // --- HARDCODED VARS THAT DEPEND ON THE CARDS ---
    private int range = 3;
    private Color color = Color.red;


    #region  Unity events :
    private void OnEnable()
    {
        actions.FindActionMap(INPUT_ACTION_MAP).Enable();
        actions.FindActionMap(INPUT_ACTION_MAP).FindAction(INPUT_SELECT_ACTION).performed += Select;
    }

    private void OnDisable()
    {
        actions.FindActionMap(INPUT_ACTION_MAP).Disable();
        actions.FindActionMap(INPUT_ACTION_MAP).FindAction(INPUT_SELECT_ACTION).performed -= Select;
    }

    private void Update()
    {
        // Higlight area on hover :
        Vector2Int mousePos = GetMousePosition();

        if (!(mousePos == previousMousePos))
        {
            HandleHover(mousePos);

            previousMousePos = mousePos;
        }
    }
    #endregion

    #region Custom methods :
    public void Initialize(Camera cam, GridView gridView, GridData gridData, InputActionAsset actions)
    {
        this.cam = cam;
        this.gridView = gridView;
        this.gridData = gridData;
        this.actions = actions;

        // Debug.Log("cam: " + cam);
        // Debug.Log("gridView: " + gridView);
        // Debug.Log("gridView.tilemap: " + (gridView != null ? gridView.MainTilemap : "gridView null"));
        // Debug.Log("gridData: " + gridData);
        // Debug.Log("actions: " + actions);

        FiguresPositions = new FigureData[gridData.rows, gridData.columns];

        actionMap = actions.FindActionMap(INPUT_ACTION_MAP);
        // Debug.Log("actionmap: " + actionMap);
        selectAction = actions.FindActionMap(INPUT_ACTION_MAP).FindAction(INPUT_SELECT_ACTION);

        actionMap.Enable();
    }
    public void Process(GameplayEngine engine, float dt)
    {
        // :-) It's a system so i have to implement it... 
    }


    private void Select(InputAction.CallbackContext context)
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue(); // Read mouse position on screen
        Vector2Int mousePos = ScreenToGridPos(mouseScreenPos); // convert it to grid position

        Debug.Log($"Mouse clicked on cell : {mousePos}");

        HandleClick(mousePos);
    }

    private void HandleClick(Vector2Int position)
    {
        if (!InteractionsControl.Instance.CardIsSelected) return; // Clickable only if a card is selected

        // Handle out-of-grid clicks :
        if (!IsInGrid(position))
        {
            Debug.Log("Clicked outside grid → cancel card");
            InteractionsControl.Instance.CancelCardSelection();
            gridView.HighlightMap.ClearAllTiles();
            return;
        }

        // things to do when clicked (ex : change color to begin with)
        ActionSystem.Instance.Perform(new PlayCardGA(InteractionsControl.Instance.SelectedCard.Card));
    }

    private void HandleHover(Vector2Int position)
    {
        if (!InteractionsControl.Instance.CardIsSelected) // Hover highlight effect only when a card is selected
        {
            gridView.HighlightMap.ClearAllTiles();
            return;
        }

        if (!IsInGrid(position))
        {
            gridView.HighlightMap.ClearAllTiles();
            return; // Handle out-of-grid
        }

        // Highlight area on hover :
        List<Vector2Int> area = GetArea(position, range);
        gridView.Highlight(area);
    }

    private Vector2Int GetMousePosition()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue(); // Read mouse position on screen
        Vector2Int mousePos = ScreenToGridPos(mouseScreenPos); // convert it to grid position

        return mousePos;
    }

    private Vector2Int ScreenToGridPos(Vector3 mousePos)
    {
        // Debug.Log($"Mouse screen: {mousePos}");
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f)); // convert mouse position to world position
        // Debug.Log($"World pos: {worldPos}");

        GridLayout gridLayout = gridView.GetComponentInParent<GridLayout>();
        Vector3Int cellPos = gridLayout.WorldToCell(worldPos); // convert it again to a cell from the tilemap
        // Debug.Log($"Cell pos: {cellPos}");

        return new Vector2Int(cellPos.x, cellPos.y); // return a Vector2Int that represents the cell
    }

    private List<Vector2Int> GetArea(Vector2Int center, int range)
    {
        List<Vector2Int> area = new(); // check if correct way to write it 

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector2Int position = new Vector2Int(center.x + x, center.y + y); // Starting from the center, get all cells withing range
                TileData tile = gridData.GetTile(position);

                if (tile != null) // And is walkable/not occupied etc.
                {
                    area.Add(position);
                }
            }
        }

        // foreach (Vector2Int v in area) Debug.Log($"cell : {v}");

        return area;
    }

    private bool IsInGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 &&
               pos.x < gridData.rows &&
               pos.y < gridData.columns;
    }

    private void SpawnFigureAt(FigureData figure, Vector2Int cell)
    {
        Vector3 position = gridData.GetTile(cell).Position;
        FigureData figureInstance = Instantiate(figure, position, Quaternion.identity);

        FiguresPositions[cell.x, cell.y] = figureInstance; // Set figure inside array representing board
    }
    #endregion
}
