using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Movement Card")]
public class MovementCard : CardData
{
    [SerializeReference] [SR] protected IMovementBehaviour movementSpecs;
    public IMovementBehaviour MovementSpecs => movementSpecs;

    public uint MovementRange => movementSpecs.MovementRange;


    public override void Play()
    {
        movementSpecs.Perform();
        foreach (SpecialAbility specialAbility in SpecialAbilities)
        {
            specialAbility?.Perform();
        }
    }
}
