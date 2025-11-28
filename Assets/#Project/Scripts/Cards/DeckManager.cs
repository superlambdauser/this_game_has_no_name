using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Deck of cards. Governs discard pile and works with Hand script.
/// </summary>
public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] private CardsCollection playerDeck;
    private List<CardData> stackPile = new List<CardData>(); 
    private List<CardData> discardPile = new List<CardData>();
    [SerializeField] private HandManager handManager;
    [HideInInspector] public List<CardData> HandCards { get; private set; }
    private int currentIndex = 0;

    private void Start()
    {
        // (Temporary) Load all card assets from Ressources folder :
        CardData[] cards = Resources.LoadAll<CardData>("CardDatas"); // !!! The given path MUST be within a folder named "Resources" 

        stackPile.AddRange(cards); // (Temporary) Adds all cards scriptable objects from the ressource folder
    }

    public void DrawCard()
    {
        if (stackPile.Count == 0) return;

        CardData nextCard = stackPile[currentIndex];

        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % stackPile.Count; // Increment index but set it to 0 when reaching end of stackPile
    }

    public void DiscardCard(CardData card)
    {
        if (HandCards.Contains(card))
        {
            HandCards.Remove(card);
            discardPile.Add(card);
        }
    }

    public void DrawHand(int amount = 5)
    {
        for (int i = 0; i < amount; i++)
        {
            DrawCard();
        }
    }

    public void Shuffle()
    {
        for (int i = stackPile.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, stackPile.Count);
            CardData temp = stackPile[i];
            stackPile[i] = stackPile[rnd];
            stackPile[rnd] = temp;
        }
    }
}
