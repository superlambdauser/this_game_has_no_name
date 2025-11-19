using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Special Card")]
public class SpecialCard : Card
{
    [Header("Special Card Traits :")]
    [SerializeReference] [SR] protected IMovementBehaviour movementBehaviour;
    [SerializeReference] [SR] protected IAttackBehaviour attackBehaviour;
    [SerializeReference][SR] protected ISpecialAbility specialAbility;


    public override void Play()
    {
        movementBehaviour?.Move();
        attackBehaviour?.Attack();
        specialAbility?.Perform();
    }
}
