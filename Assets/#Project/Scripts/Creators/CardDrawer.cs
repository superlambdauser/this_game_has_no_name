using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CardDrawer : Singleton<CardDrawer>
{
    [SerializeField] private CardView cardViewPrefab;
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private AnimationCurve movementCurve;

    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation, Transform parent)
    {
        CardView cardView = Instantiate(cardViewPrefab, parent);

        RectTransform rt = cardView.GetComponent<RectTransform>();
        rt.position = position;
        rt.rotation = rotation;

        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, duration);

        return cardView;
    }
}