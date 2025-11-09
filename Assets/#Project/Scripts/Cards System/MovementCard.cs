using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Movement Card")]
public class MovementCard : Card
{
    [Header("Movement Cards Traits :")]
    [SerializeReference] [SR] protected IMovementBehaviour movementBehaviour;
    [SerializeReference] [SR] protected ISpecialAbility specialAbility;


    public override void Play()
    {
        movementBehaviour.Move();
        specialAbility?.Perform();
    }

    #if UNITY_EDITOR // Wrapping my OnValidate() method for safety
    private void OnValidate() // Checking that my mandatory element is assigned
    {
        if (movementBehaviour == null)
        {
            Debug.LogWarning($"{name}: Movement behaviour is missing!", this);
        }
    }
    #endif
}
