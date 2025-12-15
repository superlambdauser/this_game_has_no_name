using System.Collections.Generic;
using UnityEngine;

public class TestSystem : MonoBehaviour
{
    private CardsCollection deck;

    public void Initialize(CardsCollection deck)
    {
        this.deck = deck;
        DeckSystem.Instance.Setup(deck);
    }
}