using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Special Card")]
public class SpecialCard : CardData
{
    [SerializeReference] [SR] protected IMovementBehaviour movementSpecs;
    public IMovementBehaviour MovementSpecs => movementSpecs;
    [SerializeReference] [SR] protected IAttackBehaviour attackBehaviour;
    public IAttackBehaviour AttackBehaviour => attackBehaviour;


    public override void Play()
    {
        movementSpecs?.Perform();
        attackBehaviour?.Perform();

        foreach (SpecialAbility specialAbility in SpecialAbilities)
        {
            specialAbility?.Perform();
        }
    }
}
