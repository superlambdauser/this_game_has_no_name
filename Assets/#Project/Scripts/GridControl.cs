using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridControl : MonoBehaviour, ISystem // bridge between logic & view
{
    private Camera cam;
    private GridView gridView;
    private GridData gridData;
    private InputActionAsset actions;
    private InputAction selectAction;
    private const string INPUT_ACTION_MAP = "InGameActions";
    private const string INPUT_SELECT_ACTION = "Select";


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
    #endregion

    #region Custom methods :
    public void Initiate(Camera cam, GridView gridView, GridData gridData, InputActionAsset actions)
    {
        this.cam = cam;
        this.gridView = gridView;
        this.gridData = gridData;
        this.actions = actions;
        
        Debug.Log("cam: " + cam);
        Debug.Log("gridView: " + gridView);
        Debug.Log("gridView.tilemap: " + (gridView != null ? gridView.MainTilemap : "gridView null"));
        Debug.Log("gridData: " + gridData);
        Debug.Log("actions: " + actions);

        selectAction = actions.FindActionMap(INPUT_ACTION_MAP).FindAction(INPUT_SELECT_ACTION);
    }
    public void Process(GameplayEngine engine, float dt)
    {
        // :-) It's a system so i have to implement it... 
    }

    private void Select(InputAction.CallbackContext context)
    {
        Vector2Int mousePos = ScreenToGridPos(context.ReadValue<Vector2>()); // Getting the mouse position from the context

        HandleClick(mousePos);
    }

    private Vector2Int ScreenToGridPos(Vector3 mousePos)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos); // convert mouse position in world position
        Vector3Int cellPos = gridView.MainTilemap.WorldToCell(worldPos); // convert it again to a cell from the tilemap

        return new Vector2Int(cellPos.x, cellPos.y); // return a Vector2Int that 
    }

    private void HandleClick(Vector2Int position)
    {
        TileData tile = gridData.GetTile(position);

        if (tile == null) return; //Handle out-of-grid clicks

        // things to do when clicked (ex : change color to begin with)
        List<Vector2Int> area = GetArea(position, range);
        gridView.Highlight(area, color);
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

        return area;
    }
    #endregion
}
