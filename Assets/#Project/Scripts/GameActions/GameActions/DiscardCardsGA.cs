using UnityEngine;

public class DiscardCardsGA : GameAction
{
    public int Amount { get; set; }

    public DiscardCardsGA(int amount)
    {
        Amount = amount;
    }
}
