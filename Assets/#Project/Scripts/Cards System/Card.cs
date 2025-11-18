using System.Collections.Generic;
using UnityEngine;
using SerializeReferenceEditor;


public abstract class Card : ScriptableObject
{
    [Header("Basic Cards Traits :")]
    [SerializeField] private string cardName;
    public string CardName => cardName;
    [SerializeField] private CardRarity cardRarityLevel;
    public CardRarity CardRarityLevel => cardRarityLevel;
    [SerializeField] private List<CardType> cardTypes;
    public List<CardType> CardTypes => cardTypes;

    // Optional behaviours -> SerializeReference (protected for children to access)
    protected uint attackRange = 0;
    public virtual uint AttackRange => attackRange;
    protected uint movementRange = 0;
    public virtual uint MovementRange => movementRange;


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
