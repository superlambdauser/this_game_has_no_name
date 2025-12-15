using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    private CardsCollection deck;

    public void Initialize(CardsCollection deck)
    {
        this.deck = deck;
        DeckSystem.Instance.Setup(deck);

        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}