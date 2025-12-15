using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Attack Card")]
public class AttackCard : CardData
{
    [SerializeReference][SR] protected IAttackBehaviour attackSpecs;
    public IAttackBehaviour AttackSpecs => attackSpecs;

    public uint AttackRange => attackSpecs.AttackRange;
    public uint Damage => attackSpecs.Damages;


    public override void Play()
    {
        attackSpecs.Perform();
        foreach (SpecialAbility specialAbility in SpecialAbilities)
        {
            specialAbility?.Perform();
        }
    }
}