using UnityEngine;

[System.Serializable]
public  class BasicMovement : IMovementBehaviour
{
    private FigureData target; // Temp to not forget we need this
    public FigureData Target => target;

    private Vector2Int position;
    public Vector2Int Position => position;

    [SerializeField] private uint movementRange;
    public uint MovementRange => movementRange;


    public void Perform()
    {
        Move();
    }

    public void Move()
    {
        // TO DO : Check if in range

        // Then :
        target.SetPosition(position);
    }
}
