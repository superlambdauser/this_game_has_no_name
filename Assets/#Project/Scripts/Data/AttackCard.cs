using UnityEngine;

public class AttackCard : Card
{
    private Card Card;
    public AttackCard(RarityLevels rarity = 0)
    {
        Type = Types.ATTACK;
        Rarity = rarity;
        Color = Color.red;
    }
}
