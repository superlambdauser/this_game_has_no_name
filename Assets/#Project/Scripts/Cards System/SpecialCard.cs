using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Special Card")]
public class SpecialCard : Card
{
    [SerializeReference] [SR] protected IMovementBehaviour movementBehaviour;
    public IMovementBehaviour MovementBehaviour => movementBehaviour;
    [SerializeReference] [SR] protected IAttackBehaviour attackBehaviour;
    public IAttackBehaviour AttackBehaviour => attackBehaviour;


    public override void Play()
    {
        movementBehaviour?.Perform();
        attackBehaviour?.Perform();

        foreach (SpecialAbility specialAbility in SpecialAbilities)
        {
            specialAbility?.Perform();
        }
    }
}
