using UnityEngine;

[System.Serializable]
public class BasicAttack : IAttackBehaviour
{
    private HealthSystem target; // Probably need to make it a public var since target changes every turn ?
    public HealthSystem Target => target;

    [SerializeField] private uint damages;
    public uint Damages => damages;

    [SerializeField] private uint attackRange;
    public uint AttackRange => attackRange;

    public void Attack()
    {
        target.LooseHP(damages);
    }
}
