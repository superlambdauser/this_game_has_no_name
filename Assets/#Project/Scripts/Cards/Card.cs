using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to point to CardData datas as runtime instances -> can modify values of the instance and not all the CardDatas
/// </summary>
public class Card
{
    private readonly CardData data;
    public CardData Data => data;

    // Immutable runtime values :
    public string Name => data.CardName;
    public CardData.CardRarity RarityLevel => data.CardRarityLevel;
    public CardData.CardType Type => data.TypeFlags;
    public List<Ability> SpecialAbilities => data.Abilities;
    public List<Effect> Effects => data.Effects;

    // Runtime stats :
    public int AttackDamage { get; private set; }
    public int AttackRange { get; private set; }
    public int MovementRange { get; private set; }
    public bool HasAttack => data.TypeFlags.HasFlag(CardData.CardType.Attack);
    public bool HasMovement => data.TypeFlags.HasFlag(CardData.CardType.Movement);
    public bool HasSpecial => data.TypeFlags.HasFlag(CardData.CardType.Special);

    public Card(CardData data)
    {
        this.data = data;

        // Reset stats
        AttackDamage = 0;
        AttackRange = 0;
        MovementRange = 0;

        // Extract runtime values from abilities
        if (HasAttack)
        {
            if (data.AttackSpecs == null)
                Debug.LogError($"Attack card '{data.CardName}' must have AttackSpecs assigned.");
            else
            {
                AttackDamage = data.AttackSpecs.Damages;
                AttackRange = data.AttackSpecs.AttackRange;
            }

            // Attack cards cannot have movement specs
            if (HasMovement)
                Debug.LogError($"Attack card '{data.CardName}' cannot have Movement type or MovementSpecs.");
        }

        if (HasMovement)
        {
            if (data.MovementSpecs == null)
                Debug.LogError($"Movement card '{data.CardName}' must have MovementSpecs assigned.");
            else
                MovementRange = data.MovementSpecs.MovementRange;

            // Movement cards cannot have attack specs
            if (HasAttack)
                Debug.LogError($"Movement card '{data.CardName}' cannot have Attack type or AttackSpecs.");
        }

        if (HasSpecial)
        {
            if (data.AttackSpecs != null)
            {
                AttackDamage = data.AttackSpecs.Damages;
                AttackRange = data.AttackSpecs.AttackRange;
            }

            if (data.MovementSpecs != null)
                MovementRange = data.MovementSpecs.MovementRange;
        }
    }
}
