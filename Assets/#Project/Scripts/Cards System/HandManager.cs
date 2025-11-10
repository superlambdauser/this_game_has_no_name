using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] private CardDisplay cardPrefab;
    [SerializeField] private List<CardDisplay> cardsInHand = new List<CardDisplay>();
    [SerializeField] private Transform handPosition; // Center of the hand
    [SerializeField] private float fanRotationDelta = 5f;
    [SerializeField] private float cardsHorizontalSpacing = 5f;
    [SerializeField] private float cardsVerticalalSpacing = 5f;


    // Temp for testing
    [SerializeField] private int numOfCardsInHand = 8;
    
    public void Start()
    {
        for (int _ = 0; _ < numOfCardsInHand; _++)
        {
            AddCardToHand();
        }
    }

    public void AddCardToHand()
    {
        // Card instantiation :
        CardDisplay newCard = Instantiate(cardPrefab, handPosition.position, Quaternion.identity, handPosition);

        cardsInHand.Add(newCard);

        UpdateHandDisplay();
    }
    
    private void UpdateHandDisplay()
    {
        int cardsCount = cardsInHand.Count;
        float midpoint = (cardsCount - 1f) / 2f;

        for (int x = 0; x < cardsCount; x++)
        {
            float rotationAngle = fanRotationDelta * (x - midpoint); // Get an even spread rotation depending on the number of cards in hand

            float horizontalOffset = cardsHorizontalSpacing * (x - midpoint);

            // Set card horizontal position between -1, 1
            float normalizedPosition = (2f * x / (cardsCount - 1f) - 1f);
            float verticalOffset = cardsVerticalalSpacing * (1 - normalizedPosition * normalizedPosition);

            // Apply result 
            cardsInHand[x].transform.localRotation = Quaternion.Euler(0f, 0f, -rotationAngle); 
            cardsInHand[x].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
