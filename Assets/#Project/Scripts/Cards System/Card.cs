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


    public enum CardType
    {
        Attack,
        Movement,
        Special
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
