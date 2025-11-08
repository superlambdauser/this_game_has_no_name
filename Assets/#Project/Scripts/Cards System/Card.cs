using System.Collections.Generic;
using UnityEngine;


public abstract class Card : ScriptableObject
{
    [Header("Basic Cards Traits :")]
    [SerializeField] private string cardName;
    public string CardName => cardName;
    [SerializeField] private CardRarity cardRarityLevel;
    public CardRarity CardRarityLevel => cardRarityLevel;
    [SerializeField] private List<CardType> cardTypes;
    public List<CardType> CardTypes => cardTypes;


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
