using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card")]
public abstract class Card : ScriptableObject
{
    public string CardName { get; private set; }
    public List<CardType> CardTypes { get; private set; }
    public List<CardRarity> CardRarities { get; private set; }


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
