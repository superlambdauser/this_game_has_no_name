using System.Collections;
using UnityEngine;

[System.Serializable]
public class Attack : Ability // NB : All attacks have to inherit from BasicAttack !
{
    public override AbilityCategory Category => AbilityCategory.Attack;
    [SerializeField] private int attackRange;
    public int AttackRange => attackRange;
    [SerializeField] private int damages;
    public int Damages => damages;

    public override GameAction GetGameAction()
    {
        AttackGA damageGA = new(damages, attackRange);
        return damageGA;
    }
}
