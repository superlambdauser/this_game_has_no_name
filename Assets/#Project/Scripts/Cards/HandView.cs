using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Bridge between cardView & cardData, only displaying hand
/// </summary>
public class HandView : Singleton<HandView>
{
    private List<CardView> cardsInHand = new List<CardView>();
    public List<CardView> CardsInHand => cardsInHand;
    public int numOfCardsInHand = 8; // Temp for testing
    public int maxHandSize = 5;

    [SerializeField] private CardView cardPrefab;
    private RectTransform handPosition;
    private float canvasHeight;
    [SerializeField] private RectTransform stackPilePoint;
    [SerializeField] private RectTransform discardPilePoint;

    [Header("Circle for fan shape settings :")]
    [SerializeField, Range(-1f, 1f)] private float fanVerticalPosition = 0.35f;
    [SerializeField, Range(0f, 1f)] private float fanStrength = 0.5f; // 0 = flat shape, 1 = wide arc
    [SerializeField, Range(0f, 180f)] private float maxAngle = 80f; // startAngle = -arcAnlge/2 (& endAngle = arcAngle/2) already set correct boundaries if the given angle is bigger than 180°, I clamp it in the inspector for clarity.
    [SerializeField] private float radius = 300f; // Depth of the curve (and radius of the circle shape)



    private void Update()
    {
        UpdateHandDisplay();
    }


    #region Custom Methods
    public void Initialize()
    {
        Debug.Log("HandView Initiate() called");
        handPosition = GetComponent<RectTransform>();
        canvasHeight = handPosition.rect.height;

        cardsInHand.Clear();

        UpdateHandDisplay();
    }

    /// <summary>
    /// Gets the CardView linked to a given CardData
    /// </summary>
    /// <param name="cardData"></param>
    /// <returns></returns>
    private CardView DataToView(CardData cardData)
    {
        // Loop through each CardView element in the list and return it if CardDatas match
        foreach (CardView cardView in cardsInHand)
        {
            if (cardView.CardData == cardData) return cardView; // Return if found
        }

        return null; // Return null if no match found
    }
    /// <summary>
    /// Instantiate a given card & display it on screen
    /// </summary>
    /// <param name="cardData"></param>
    public void AddCardToHand(CardData cardData)
    {
        if (cardsInHand.Count >= maxHandSize) return;

        CardView card = CardDrawer.Instance.CreateCardView(cardData, stackPilePoint, handPosition);

        cardsInHand.Add(card);
    }

    public CardView RemoveCardFromHand(CardData cardData)
    {
        CardView cardView = DataToView(cardData);

        cardsInHand.Remove(cardView); // Remove from the list

        UpdateHandDisplay(); // Update displayed hand

        return cardView;
    }

    private void UpdateHandDisplay()
    {
        int cardsCount = cardsInHand.Count;
        if (cardsCount == 0) return;

        float arcAngle = fanStrength * maxAngle;
        float startAngle = -arcAngle / 2;
        float angleStep = (cardsCount > 1) ? arcAngle / (cardsCount - 1f) : 0f; // (count - 1) because for N cards there will always be (N-1) steps between 1st and last card

        RectTransform rt = handPosition;
        float canvasHeight = rt.rect.height;

        for (int i = 0; i < cardsCount; i++)
        {
            float angleInDegrees = startAngle + (i * angleStep); // Get current card's angle in the circle

            // Circle coordinates :
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad; // We need radians to work with Mathf.Sin() and Mathf.Cos();

            float x = radius * Mathf.Sin(angleInRadians);
             float y = radius * Mathf.Cos(angleInRadians) - radius/2f - canvasHeight * fanVerticalPosition;

            // Set current card's position & rotation :
            cardsInHand[i].transform.localPosition = new Vector3(x, y, 0f); // Could maybe make it a Vector2 instead ?
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0, 0, -angleInDegrees);
        }
    }
    #endregion
}
