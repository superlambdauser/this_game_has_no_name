using System;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

/// <summary>
/// Holds data for each individual card.
/// </summary>
[CreateAssetMenu(fileName = "New card", menuName = "Card")]
public class CardData : ScriptableObject
{
    [Header("Cards Traits :")]
    [SerializeField] private string cardName;
    public string CardName => cardName;
    [SerializeField] private CardRarity cardRarityLevel;
    public CardRarity CardRarityLevel => cardRarityLevel;
    [SerializeField] private CardType typeFlags;
    public CardType TypeFlags => typeFlags;

    [Header("Specs :")]
    [SerializeReference][SR] protected Attack attackSpecs;
    public Attack AttackSpecs => attackSpecs;
    [SerializeReference][SR] protected Movement movementSpecs;
    public Movement MovementSpecs => movementSpecs;
    [SerializeReference][SR] protected List<Ability> abilities = new List<Ability>();
    public List<Ability> Abilities => abilities;
    [SerializeReference][SR] protected List<Effect> effects;
    public List<Effect> Effects => effects;


    [Flags]
    public enum CardType // [Flags] attribute indicates that our enum consists of bit fields -> this indicates to the compiler that the enum has to be treated in a way that its values are not exclusive -> Values combinations are possible
    {
        // NB : The enum indexes should always be powers of 2 (for a binary reason that is beyond my comprehension)
        // The using of [Flag] attribute limits the number of elements inside the enum to 32 !
        None = 0,
        Attack = 1,
        Movement = 2,
        Special = 4
    }

    public enum CardRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Unique
    }


    public void Play()
    {
        foreach (Ability ability in abilities) ability?.GetGameAction();

        if (attackSpecs != null) attackSpecs.GetGameAction();

        if (movementSpecs != null) movementSpecs.GetGameAction();

        foreach (Effect effect in effects) effect?.GetGameAction();
    }
}
