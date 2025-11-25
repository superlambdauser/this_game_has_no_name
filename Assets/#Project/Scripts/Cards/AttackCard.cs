using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Attack Card")]
public class AttackCard : CardData
{
    [SerializeReference][SR] protected IAttackBehaviour attackBehaviour;
    public IAttackBehaviour AttackBehaviour => attackBehaviour;

    public uint AttackRange => attackBehaviour.AttackRange;


    public override void Play()
    {
        attackBehaviour.Perform();
        foreach (SpecialAbility specialAbility in SpecialAbilities)
        {
            specialAbility?.Perform();
        }
    }
}