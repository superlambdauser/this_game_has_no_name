using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Deck of cards. Governs discard pile and works with Hand script.
/// </summary>
public class DeckSystem : Singleton<DeckSystem>
{
    private List<CardData> stackPile = new List<CardData>();
    private List<CardData> discardPile = new List<CardData>();
    private HandView handView;
    public List<CardData> HandCards { get; set; } = new List<CardData>();

    #region Unity Events :
    private void OnEnable()
    {
        // Attaching performers :
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardCardsGA>(DiscardCardsPerformer);
        ActionSystem.AttachPerformer<DiscardSingleCardGA>(DiscardSingleCardPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);

        // Subscribing reactions :
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ActionSystem.ReactionTiming.Pre);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ActionSystem.ReactionTiming.Post);
    }

    private void OnDisable()
    {
        // Detaching performers :
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardCardsGA>();
        ActionSystem.DetachPerformer<DiscardSingleCardGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();

        // Unsubscribing reactions :
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ActionSystem.ReactionTiming.Pre);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ActionSystem.ReactionTiming.Post);
    }
    #endregion


    #region Custom Methods
    public void Initiate()
    {
        Debug.Log("Deck System Initiation called");
        handView = HandView.Instance;
        // Debug.Log($"Deck : {(handView != null ? handView.gameObject.name : "null")}");

        // Load all card assets from Ressources folder :
        CardsCollection cardsCollection = Resources.Load<CardsCollection>("Cards Collections/TempDeckTest"); // !!! The given path MUST be within a folder named "Resources" 
        CardData[] deck = cardsCollection.CardsInCollection.ToArray();

        // Debug.Log($"Deck : {(deck != null ? "not null" : "null")}");

        stackPile.AddRange(deck); // Adds all cards scriptable objects from the ressource folder to draw pile
        // Debug.Log($"Stackpile : {(deck != null ? stackPile.Count : "null")}");

        StartCoroutine(DrawHand(handView.maxHandSize)); // Draw initial hand
    }

    #region Performers
    // Performers :
    private IEnumerator DrawCardsPerformer(DrawCardsGA gameAction)
    {
        int actualAmount = Mathf.Min(gameAction.Amount, stackPile.Count); // Draw what's left in the stackPile if it's less than given amount
        int notDrawnAmount = gameAction.Amount - actualAmount; // What's left to draw after shuffle & refill of the deck

        // Draw cards :
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }

        // Shuffle & refill deck if needed :
        if (notDrawnAmount > 0)
        {
            // Here we do not SuffleDeck() because cards are already drawn randomly within the DrawCard() method
            RefillDeck();

            for (int i = 0; i < notDrawnAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }

    private IEnumerator DiscardCardsPerformer(DiscardCardsGA gameAction)
    {
        int actualAmount = Mathf.Min(gameAction.Amount, HandCards.Count); // Discard a maximum of all cards in hand amount

        // Create a copy of the 1st N cards :
        List<CardData> cardsToDiscard = HandCards.Take(actualAmount).ToList();

        foreach (CardData card in cardsToDiscard)
        {
            yield return DiscardCard(card);
        }
    }

    private IEnumerator DiscardSingleCardPerformer(DiscardSingleCardGA gameAction)
    {
        yield return DiscardCardsPerformer(gameAction);
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA gameAction)
    {
        yield return DiscardCardsPerformer(gameAction);
    }
    #endregion

    #region Reactions
    private void EnemyTurnPreReaction(EnemyTurnGA gameAction)
    {
        DiscardAllCardsGA discardAllCardsGA = new DiscardAllCardsGA();
        ActionSystem.Instance.AddReaction(discardAllCardsGA);
    }
    private void EnemyTurnPostReaction(EnemyTurnGA gameAction)
    {
        DrawCardsGA drawCardsGA = new DrawCardsGA(handView.maxHandSize);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }
    #endregion

    public IEnumerator DrawCard()
    {
        Debug.Log("DrawCard() called");
        CardData card = stackPile.DrawRandom();

        HandCards.Add(card);
        handView.AddCardToHand(card);

        yield return HandCards;
    }

    public IEnumerator DiscardCard(CardData card)
    {
        if (HandCards.Contains(card))
        {
            HandCards.Remove(card);
        }

        handView.RemoveCardFromHand(card);

        discardPile.Add(card);

        yield break;
    }

    public IEnumerator DrawHand(int amount = 5)
    {
        Debug.Log("DrawHand() called");
        for (int i = 0; i < amount; i++)
        {
            yield return DrawCard();
        }
    }

    private void ShuffleDeck()
    {
        for (int i = stackPile.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, stackPile.Count);
            CardData temp = stackPile[i];
            stackPile[i] = stackPile[rnd];
            stackPile[rnd] = temp;
        }
    }

    private void RefillDeck()
    {
        stackPile.AddRange(discardPile);
        discardPile.Clear();
    }
    #endregion
}
