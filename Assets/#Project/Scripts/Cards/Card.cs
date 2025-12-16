using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to point to CardData datas as runtime instances -> can modify values of the instance and not all the CardDatas
/// </summary>
public class Card
{
    private readonly CardData data;
    public CardData Data => data;

    // CardData immutables :
    public string Name => data.CardName;
    public CardData.CardRarity RarityLevel => data.CardRarityLevel;
    public CardData.CardType Type => data.TypeFlags;
    public List<SpecialAbility> SpecialAbilities => data.SpecialAbilities;
    public List<Effect> Effects => data.Effects;

    // Specs :
    public uint AttackDamage { get; private set; }
    public uint AttackRange { get; private set; }
    public uint MovementRange { get; private set; }
    public bool HasAttack => data.TypeFlags.HasFlag(CardData.CardType.Attack);
    public bool HasMovement => data.TypeFlags.HasFlag(CardData.CardType.Movement);
    public bool HasSpecial => data.TypeFlags.HasFlag(CardData.CardType.Special);

    public Card(CardData data)
    {
        this.data = data;

        if (data is AttackCard attackData)
        {
            AttackDamage = attackData.Damage;
            AttackRange = attackData.AttackRange;
        }
        else if (data is MovementCard movementData)
        {
            MovementRange = movementData.MovementRange;
        }
        else if (data is SpecialCard specialData)
        {
            if (specialData.AttackSpecs != null)
            {
                AttackDamage = specialData.AttackSpecs.Damages;
                AttackRange = specialData.AttackSpecs.AttackRange;
            }
            if (specialData.MovementSpecs != null)
            {
                MovementRange = specialData.MovementSpecs.MovementRange;
            }
        }

    }
}
