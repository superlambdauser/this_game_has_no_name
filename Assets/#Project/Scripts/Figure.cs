using UnityEngine;

public abstract class Figure : MonoBehaviour
{
    [SerializeField] GridData grid;

    public int CurrentX {get; set;}
    public int CurrentY {get; set;}

    public void SetPosition(Vector2Int position)
    {
        CurrentX = position.x;
        CurrentY = position.y;
    }

    public virtual bool [,] PossibleMove()
    {
        return new bool [grid.rows, grid.columns]; // Returns an array of rows x columns of booleans that corresponds to the cells of the grid
    }
}