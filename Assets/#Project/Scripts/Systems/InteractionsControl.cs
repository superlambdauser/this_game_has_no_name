using UnityEngine;

public class InteractionsControl : Singleton<InteractionsControl>
{
    public bool CardIsSelected { get; set; } = false;

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
}
