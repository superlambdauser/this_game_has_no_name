using System;
using System.Collections;
using UnityEngine;

public class CardDrawer : Singleton<CardDrawer>
{
    [SerializeField] private CardView cardViewPrefab;
    [SerializeField] private float movementDuration = 0.4f;
    [SerializeField] private float delayBetweenCards = 0.5f;
    [SerializeField] private AnimationCurve movementCurve;

    public CardView CreateCardView(CardData cardData, RectTransform from, RectTransform parent)
    {
        CardView cardView = Instantiate(cardViewPrefab, from.position, Quaternion.identity, parent);
        cardView.CardData = cardData;

        StartCoroutine(MoveCard(cardView, from.position, parent.position));

        return cardView;
    }

    private IEnumerator MoveCard(CardView card, Vector3 start, Vector3 end)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / movementDuration;
            float eased = movementCurve.Evaluate(t);

            card.transform.position = Vector3.Lerp(start, end, eased);
            yield return null;
        }
    }
}