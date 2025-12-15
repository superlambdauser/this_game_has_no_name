using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Updates dynamically the visuals of each card depending on given data.
/// </summary>
public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler // Required interfaces when using the OnPointer[...] methods
{
    [SerializeField] private Card card;
    public Card Card
    {
        get => card;
        set
        {
            card = value;
            UpdateCardDisplay(card); // Whenever CardData is assigned (normal card, hover card, pooled cards...), immediately refresh UI
        }
    }

    [Serializable]
    private struct CardTypeIcon
    {
        public CardData.CardType type; // Flag
        public Image icon; // UI icon
    }

    [Header("Main UI :")]
    [SerializeField] private Image cardImage;
    [SerializeField] private Image rarityLevelFrameImage;

    [Header("Type icons :")]
    [SerializeField] private CardTypeIcon[] typeIconImages;

    [Header("Texts :")]
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardRarityLevelText;
    [SerializeField] private TMP_Text cardAttackRangeText;
    [SerializeField] private TMP_Text cardMovementRangeText;

    private int cardRarityLevel;
    private bool pointerAlreadyEntered = false; // May be useless but keeping it atm

    private Color[] rarityColors =
    {
        Color.grey, // Common
        Color.green, // Uncommon
        Color.blue, // Rare
        Color.magenta, // Epic
        Color.yellow // Unique
    };

    public void OnPointerEnter(PointerEventData eventData) // For hovering effect
    {
        Debug.Log($"Mouse entered {gameObject.name} + Pointer already entered : {pointerAlreadyEntered}");

        if (!CompareTag("Hover"))
        {
            Vector2 hoverPos = new Vector2(transform.position.x, transform.position.y);
            Vector3 hoverRot = transform.rotation.eulerAngles;

            CardHoverSystem.Instance.Show(Card, hoverPos, hoverRot);

            pointerAlreadyEntered = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CompareTag("Hover"))
        {
            Debug.Log($"The cursor exited {gameObject.name}.");

            pointerAlreadyEntered = false;

            CardHoverSystem.Instance.Hide();
        }
    }

    public void UpdateCardDisplay(Card card)
    {
        Debug.Log($"{name} CardView updating. card = {card?.Name}, cardImage = {(cardImage == null ? "NULL" : "OK")}");

        if (card == null) return;

        Debug.Log($"Updating card {this.card?.Name ?? "NULL"}");
        Debug.Log($"{Card.Name} flags = {Card.Type}");


        cardNameText.text = Card.Name; // Update title

        // Update frame color & displayed rarity level based on card's rarity level enum index :
        cardRarityLevel = (int)Card.RarityLevel;

        rarityLevelFrameImage.color = rarityColors[cardRarityLevel];
        cardRarityLevelText.text = cardRarityLevel.ToString();

        // Update range values :
        cardAttackRangeText.gameObject.SetActive(false);
        cardMovementRangeText.gameObject.SetActive(false);

        if (Card.HasSpecial) // Handling special cards logic
        {
            if (Card.HasAttack && Card.AttackRange > 0)
            {
                cardAttackRangeText.text = Card.AttackRange.ToString();
                cardAttackRangeText.gameObject.SetActive(true);
            }

            if (Card.HasMovement && Card.MovementRange > 0)
            {
                cardMovementRangeText.text = Card.MovementRange.ToString();
                cardMovementRangeText.gameObject.SetActive(true);
            }
        }

        else if (Card.HasAttack && Card.AttackRange > 0)
        {
            cardAttackRangeText.text = Card.AttackRange.ToString();
            cardAttackRangeText.gameObject.SetActive(true);
        }

        else if (Card.HasMovement && Card.MovementRange > 0)
        {
            cardMovementRangeText.text = Card.MovementRange.ToString();
            cardMovementRangeText.gameObject.SetActive(true);
        }

        // Resetting all icons to inactive
        CardData.CardType flags = Card.Type;

        foreach (CardTypeIcon item in typeIconImages)
        {
            if (item.icon == null) continue; // Safety check -> skip item
            
            // Else if the card has this flag, show the icon
            bool active = flags.HasFlag(item.type);
            item.icon.gameObject.SetActive(active); // On if has flag, OFF if not
        }
    }
}
