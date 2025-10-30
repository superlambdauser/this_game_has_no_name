using UnityEngine;

public class MovementCard : Card
{
    public override void Play()
    {
        throw new System.NotImplementedException();
    }

    private void Move(Movable figure, Vector2Int targetPosition)
    {
        figure.transform.position = targetPosition;
    }
}