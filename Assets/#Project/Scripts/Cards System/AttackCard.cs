using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Attack Card")]
public class AttackCard : Card
{
    [Header("Attack Cards Traits :")]
    [SerializeReference][SR] protected BasicAttack basicAttack;
    [SerializeReference][SR] protected ISpecialAbility specialAbility;
    [SerializeField] private uint attackRange;
    public uint AttackRange => attackRange;


#if UNITY_EDITOR // Wrapping my OnValidate() method for safety
    private void OnValidate() // Checking that my mandatory element is assigned
    {
        if (basicAttack == null)
        {
            Debug.LogWarning($"{name}: Movement behaviour is missing!", this);
        }
    }
#endif

    private void Awake()
    {
        attackRange = basicAttack.AttackRange;
    }

    public override void Play()
    {
        basicAttack.Attack();
        specialAbility?.Perform();
    }
}