using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private CardDisplay cardPrefab;
    private List<CardDisplay> cardsInHand = new List<CardDisplay>();
    [SerializeField] private Transform handPosition; // Center of the CIRCLE shape that will determine our the fan shape used to display our hand (not the center of the hand itself)
    // NB : I think later in development this should be given by the GameInitiator
    public int numOfCardsInHand = 8; // Temp for testing
    public int maxHandSize = 5;

    [Header("Circle for fan shape settings :")]
    [SerializeField, Range(0f, 1f)] private float fanStrength = 0.5f; // 0 = flat shape, 1 = wide arc
    [SerializeField, Range(0f, 180f)] private float maxAngle = 80f; // startAngle = -arcAnlge/2 (& endAngle = arcAngle/2) already set correct boundaries if the given angle is bigger than 180°, I clamp it in the inspector for clarity.
    [SerializeField] private float radius = 300f; // Depth of the curve (and radius of the circle shape)



    private void Start()
    {
        for (int i = 0;  i < numOfCardsInHand; i++)
        {
            deckManager.DrawCard(this);
        }
    }

    private void Update()
    {
        UpdateHandDisplay();
    }

    public void AddCardToHand(Card cardData)
    {
        if (cardsInHand.Count < maxHandSize)
        {
            // Card instantiation :
            CardDisplay newCard = Instantiate(cardPrefab, handPosition.position, Quaternion.identity, handPosition);

            cardsInHand.Add(newCard);

            newCard.GetComponent<CardDisplay>().cardData = cardData; // Set the given cardData to instantiated card
        }

        UpdateHandDisplay();
    }

    private void UpdateHandDisplay()
    {
        int cardsCount = cardsInHand.Count;
        if (cardsCount == 0) return;

        float arcAngle = fanStrength * maxAngle;
        float startAngle = -arcAngle / 2;
        float angleStep = (cardsCount > 1) ? arcAngle / (cardsCount - 1f) : 0f; // (count - 1) because for N cards there will always be (N-1) steps between 1st and last card

        for (int i = 0; i < cardsCount; i++)
        {
            float angleInDegrees = startAngle + (i * angleStep); // Get current card's angle in the circle

            // Circle coordinates :
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad; // We need radians to work with Mathf.Sin() and Mathf.Cos();

            float x = radius * Mathf.Sin(angleInRadians);
            float y = radius * Mathf.Cos(angleInRadians);

            // Set current card's position & rotation :
            cardsInHand[i].transform.localPosition = new Vector3(x, y, 0f); // Could maybe make it a Vector2 instead ?
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0, 0, -angleInDegrees);
        }
    }
}
