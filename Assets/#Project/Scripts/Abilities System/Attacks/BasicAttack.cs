using UnityEngine;

[System.Serializable]
public class BasicAttack : IAttackBehaviour // NB : All attacks have to inherit from BasicAttack !
{
    private HealthSystem target;
    public HealthSystem Target { get; }

    [SerializeField] private uint damages;
    public uint Damages { get; }

    [SerializeField] private uint attackRange;
    public uint AttackRange => attackRange;


    public void Perform()
    {
        Attack();
    }

    public virtual void Attack()
    {
        target.LooseHP(damages);
    }

}
