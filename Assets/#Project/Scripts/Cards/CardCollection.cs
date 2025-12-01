using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Generic collection of CardData objects. A deck, a booster pack, etc.
/// </summary>
[CreateAssetMenu(fileName = "New card collection", menuName = "Card Collection")]
public class CardsCollection : ScriptableObject
{
    [SerializeField] bool allowsDuplicates = true;
    [SerializeField] private List<CardData> cardsInCollection;
    public List<CardData> CardsInCollection => cardsInCollection;

    public void RemoveCard(CardData card)
    {
        if (CardsInCollection.Contains(card)) CardsInCollection.Remove(card);
        else Debug.Log($"Cards collection does not contain {card}.");
    }

    public void AddCard(CardData card)
    {
        if (allowsDuplicates) CardsInCollection.Add(card);
        else if (!allowsDuplicates && !CardsInCollection.Contains(card)) CardsInCollection.Add(card);
        else Debug.Log($"{card} is already in the collection, which does not allows duplicates.");
    }
}
