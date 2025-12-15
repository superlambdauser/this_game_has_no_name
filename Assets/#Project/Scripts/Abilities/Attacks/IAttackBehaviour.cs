using UnityEngine;

public interface IAttackBehaviour : IAbility
{
    public uint AttackRange { get; }
    public uint Damages { get; }
    public void Attack();
}
