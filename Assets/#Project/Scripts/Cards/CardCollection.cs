using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Generic collection of CardData objects. A deck, a booster pack, etc.
/// </summary>
public class CardsCollection : ScriptableObject
{
    public List<CardData> CardsInCollection { get; private set; }
    [SerializeField] bool allowsDuplicates = true;

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
