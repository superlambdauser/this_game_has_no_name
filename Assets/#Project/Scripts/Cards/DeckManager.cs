using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] List<CardData> allCards = new List<CardData>(); // TEMP. All existing objects of Card type 
    private int currentIndex = 0;

    private void Start()
    {
        // Load all card assets from Ressources folder :
        CardData[] cards = Resources.LoadAll<CardData>("CardDatas"); // !!! The given path must be within a folder named "Resources" 

        allCards.AddRange(cards);
    }

    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0) return;

        CardData nextCard = allCards[currentIndex];

        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count; 
    }
}
