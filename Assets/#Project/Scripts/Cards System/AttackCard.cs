using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Attack Card")]
[RequireComponent(typeof(IAttackBehaviour))]
public class AttackCard : Card
{
    [Header("Attack Cards Traits :")]
    [SerializeReference][SR] protected IAttackBehaviour attackBehaviour;
    [SerializeReference][SR] protected ISpecialAbility specialAbility;


    public override void Play()
    {
        attackBehaviour.Attack();
        specialAbility?.Perform();
    }

    #if UNITY_EDITOR // Wrapping my OnValidate() method for safety
    private void OnValidate() // Checking that my mandatory element is assigned
    {
        if (attackBehaviour == null)
        {
            Debug.LogWarning($"{name}: Movement behaviour is missing!", this);
        }
    }
    #endif
}