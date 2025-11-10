using System;
using UnityEngine;

public abstract class Card // abstract bc has to be implemented further with childs
{
    public RarityLevels Rarity { get; protected set; } //NB : protected = this class and its childs
    public Types Type { get; protected set; }
    public abstract string Name { get; protected set; }
    public abstract Color Color { get; protected set; }


    public enum Types
    {
        MOVEMENT = 0,
        ATTACK = 1,
        SPECIAL = 2
    }

    public enum RarityLevels
    {
        COMMON = 0,
        UNCOMMON = 1,
        RARE = 2,
        EPIC = 3,
        UNIQUE = 4 //One per run
    }

    public Types GetCardType()
    {
        return Type;
    }

    public RarityLevels GetCardRarity()
    {
        return Rarity;
    }

    public  
}
