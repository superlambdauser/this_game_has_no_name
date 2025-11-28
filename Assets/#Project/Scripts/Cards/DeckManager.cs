using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deck of cards. Governs discard pile and works with Hand script.
/// </summary>
public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] private CardsCollection playerDeck;
    private List<CardData> stackPile = new List<CardData>(); 
    private List<CardData> discardPile = new List<CardData>();
    [HideInInspector] public List<CardData> HandCards { get; private set; }
    private int currentIndex = 0;

    private void Start()
    {
        // (Temporary) Load all card assets from Ressources folder :
        CardData[] cards = Resources.LoadAll<CardData>("CardDatas"); // !!! The given path MUST be within a folder named "Resources" 

        stackPile.AddRange(cards); // (Temporary) Adds all cards scriptable objects from the ressource folder
    }

    public void DrawCard(HandManager handManager)
    {
        if (stackPile.Count == 0) return;

        CardData nextCard = stackPile[currentIndex];

        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % stackPile.Count; // Increment index but set it to 0 when reaching end of stackPile
    }
}
