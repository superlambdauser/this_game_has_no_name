using UnityEngine;

public class CardHoverSystem : Singleton<CardHoverSystem>
{
    private CardView hoveredCardView;
    public CardView HoveredCardView
    {
        get => hoveredCardView;
        set => hoveredCardView = value;
    }


    public void Initiate(CardView hoveredCardPrefab)
    {
        hoveredCardView = hoveredCardPrefab;
    }
    public void Show(CardData card, Vector2 position, Vector3 rotation)
    {
        hoveredCardView.gameObject.SetActive(true);        

        // hoveredCardView.transform.SetAsLastSibling();

        hoveredCardView.CardData = card;
        hoveredCardView.UpdateCardDisplay();
        

        hoveredCardView.GetComponent<RectTransform>().position = position;
        hoveredCardView.GetComponent<RectTransform>().rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

    public void Hide()
    {
        hoveredCardView.gameObject.SetActive(false);
    }
}