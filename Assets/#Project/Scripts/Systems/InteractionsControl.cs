using UnityEngine;

public class InteractionsControl : Singleton<InteractionsControl>
{
    public bool CardIsSelected { get; set; } = false;
    public CardView SelectedCard { get; private set; }

    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.IsPerforming) return true;
        else return false;
    }

    public bool PlayerCanHover()
    {
        if (CardIsSelected) return false;
        return true;
    }

    public void SelectCard(CardView cardView)
    {
        if (cardView == null) return;

        SelectedCard = cardView;
        CardIsSelected = true;
    }

    public void CancelCardSelection()
    {
        CardHoverSystem.Instance.Hide();
        SelectedCard = null;
        CardIsSelected = false;
    }
}
