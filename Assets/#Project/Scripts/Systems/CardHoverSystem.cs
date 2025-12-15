using UnityEngine;

public class CardHoverSystem : Singleton<CardHoverSystem>
{
    private CardView hoveredCardView;
    public CardView HoveredCardView
    {
        get => hoveredCardView;
        set => hoveredCardView = value;
    }


    public void Initialize(CardView hoveredCardPrefab, Transform parent)
    {
        hoveredCardView = Instantiate(hoveredCardPrefab, parent);
        hoveredCardView.gameObject.SetActive(false);
    }
    
    public void Show(Card card, Vector2 position, Vector3 rotation)
    {
        hoveredCardView.gameObject.SetActive(true);        

        hoveredCardView.Card = card;
        
        RectTransform rt = hoveredCardView.GetComponent<RectTransform>();

        rt.position = position;
        rt.rotation = Quaternion.Euler(rotation);

        hoveredCardView.transform.SetAsLastSibling();

    }

    public void Hide()
    {
        hoveredCardView.gameObject.SetActive(false);
    }
}