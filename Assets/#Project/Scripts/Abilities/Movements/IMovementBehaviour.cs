using UnityEngine;

public interface IMovementBehaviour : IAbility
{
    public uint MovementRange { get; }
    public void Move();
}
