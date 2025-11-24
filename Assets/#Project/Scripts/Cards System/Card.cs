using System;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;


public abstract class Card : ScriptableObject
{
    [Header("Basic Cards Traits :")]
    [SerializeField] private string cardName;
    public string CardName => cardName;
    [SerializeField] private CardRarity cardRarityLevel;
    public CardRarity CardRarityLevel => cardRarityLevel;
    [SerializeField] private List<CardType> cardTypes = new List<CardType>();
    public List<CardType> CardTypes => cardTypes;
    [SerializeReference] [SR] protected List<SpecialAbility> specialAbilities = new List<SpecialAbility>();
    public List<SpecialAbility> SpecialAbilities => specialAbilities;

    public CardType types;


    [Flags] public enum CardType // [Flags] attribute indicates that our enum consists of bit fields -> this indicates to the compiler that the enum has to be treated in a way that its values are not exclusive -> Values combinations are possible
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

    public abstract void Play();

}
