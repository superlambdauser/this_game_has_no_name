using UnityEngine;

public interface IAttackBehaviour : IAbility
{
    public uint AttackRange { get;}
    public void Attack();
}
