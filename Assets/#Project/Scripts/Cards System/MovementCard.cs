using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Movement Card")]
public class MovementCard : Card
{
    [Header("Movement Cards Traits :")]
    [SerializeReference] [SR] protected IMovementBehaviour movementBehaviour;
    public IMovementBehaviour MovementBehaviour => movementBehaviour;

    public uint MovementRange => movementBehaviour.MovementRange;


    public override void Play()
    {
        movementBehaviour.Perform();
        foreach (SpecialAbility specialAbility in SpecialAbilities)
        {
            specialAbility?.Perform();
        }
    }
}
