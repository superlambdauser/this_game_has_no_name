using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] List<Card> allCards = new List<Card>();
    private int currentIndex = 0;

    private void Start()
    {
        // Load all card assets from Ressources folder :
        Card[] cards = Resources.LoadAll<Card>("CardDatas"); // !!! The given path must be within a folder named "Resources" 

        allCards.AddRange(cards);
    }

    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0) return;

        Card nextCard = allCards[currentIndex];

        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count; 
    }
}
