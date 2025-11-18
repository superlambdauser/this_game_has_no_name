using UnityEngine;

[System.Serializable]
public class BasicMovement : IMovementBehaviour
{
    private Movable target; // Temp to not forget we need this
    public Movable Target => target;

    private Vector2Int position;
    public Vector2Int Position => position;

    [SerializeField] private uint movementRange;
    public uint MovementRange => movementRange;

    public void Move()
    {
        // TO DO : Check if in range

        // Then :
        target.MoveTo(position);
    }
}
