using UnityEngine;

public class DiscardAllCardsGA : DiscardCardsGA
{
    public DiscardAllCardsGA() : base(DeckSystem.Instance.HandCards.Count) // Automatically set the amount to amount of cards in hand
    {
    }
}