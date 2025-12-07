using UnityEngine;

public class CardHoverSystem : Singleton<CardHoverSystem>
{
    private CardView hoveredCardView;
    public CardView HoveredCardView
    {
        get => hoveredCardView;
        set => hoveredCardView = value;
    }


    public void Initiate(CardView cardPrefab)
    {
        hoveredCardView = cardPrefab;
    }
    public void Show(CardData card, Vector2 position)
    {
        hoveredCardView.gameObject.SetActive(true);

        hoveredCardView.CardData = card;
        hoveredCardView.UpdateCardDisplay();

        hoveredCardView.GetComponent<RectTransform>().position = position;
    }

    public void Hide()
    {
        hoveredCardView.gameObject.SetActive(false);
    }
}