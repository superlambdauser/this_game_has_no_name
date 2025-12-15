using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;


/// <summary>
/// Bridge between cardView (scene) & Card (data), only displaying hand
/// </summary>
public class HandView : Singleton<HandView>
{
    [SerializeField] private SplineContainer splineContainer;
    private List<CardView> cardsInHand = new List<CardView>();
    public List<CardView> CardsInHand => cardsInHand;
    public int numOfCardsInHand = 8; // Temp for testing
    public int maxHandSize = 5;

    [SerializeField] private CardView cardPrefab;
    private RectTransform handPosition;
    private float canvasHeight;
    private float canvasWidth;
    [SerializeField] private RectTransform stackPilePoint;
    public RectTransform StackPilePoint => stackPilePoint;
    [SerializeField] private RectTransform discardPilePoint;
    public RectTransform DiscardPilePoint => discardPilePoint;

    // [Header("Circle for fan shape settings :")]
    // [SerializeField, Range(-1f, 1f)] private float fanVerticalPosition = 0.35f;
    // [SerializeField, Range(0f, 1f)] private float fanStrength = 0.5f; // 0 = flat shape, 1 = wide arc
    // [SerializeField, Range(0f, 180f)] private float maxAngle = 80f; // startAngle = -arcAnlge/2 (& endAngle = arcAngle/2) already set correct boundaries if the given angle is bigger than 180°, I clamp it in the inspector for clarity.
    // [SerializeField] private float radius = 300f; // Depth of the curve (and radius of the circle shape)

    #region Custom Methods
    public void Initialize()
    {
        Debug.Log("HandView Initiate() called");
        handPosition = GetComponent<RectTransform>();
        canvasHeight = handPosition.rect.height;
        canvasWidth = handPosition.rect.width;

        cardsInHand.Clear();
    }

    public IEnumerator AddCard(CardView cardView)
    {
        cardsInHand.Add(cardView);
        yield return UpdateCardPosition(0.15f);
    }

    public CardView RemoveCard(Card card)
    {
        CardView cardView = DataToView(card);

        if (cardView == null) return null;

        cardsInHand.Remove(cardView);

        StartCoroutine(UpdateCardPosition(0.15f));

        return cardView;
    }

    private IEnumerator UpdateCardPosition(float duration)
    {
        if (cardsInHand.Count == 0) yield break;

        float cardSpacing = 1f / cardsInHand.Count; // Splines are always of 1f length
        // Placing cards depending on center of the spline :
        float firstCardPosition = 0.5f - (cardsInHand.Count - 1) * cardSpacing / 2;

        Spline spline = splineContainer.Spline;

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            float position = firstCardPosition + i * cardSpacing;

            Vector3 splineLocalPos = spline.EvaluatePosition(position);

            Vector3 tangent = spline.EvaluateTangent(position);
            tangent.Normalize();
            // Vector3 forward = spline.EvaluateTangent(position);
            // Vector3 up = spline.EvaluateUpVector(position);
            // Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);
            float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            RectTransform cardRect = cardsInHand[i].GetComponent<RectTransform>();
            cardRect.DOAnchorPos(new Vector2(splineLocalPos.x, splineLocalPos.y), duration);
            // cardsInHand[i].transform.DOLocalMove(new Vector3(0, 0, -0.01f * i), duration);
            cardsInHand[i].transform.DOLocalRotate(rotation.eulerAngles, duration);
        }

        yield return new WaitForSeconds(duration);
    }

    /// <summary>
    /// Gets the CardView linked to a given Card
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    private CardView DataToView(Card card)
    {
        // Loop through each CardView element in the list and return it if Card match
        foreach (CardView cardView in cardsInHand)
        {
            if (cardView.Card == card) return cardView; // Return if found
        }

        return null; // Return null if no match found
    }

    Vector2 WorldToCanvasPos(Vector3 worldPos)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(handPosition, screenPoint, Camera.main, out Vector2 localPoint);

        return localPoint;
    }
    #endregion
}
