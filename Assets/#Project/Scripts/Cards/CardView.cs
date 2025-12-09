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
    [SerializeField] private CardData cardData;
    public CardData CardData
    {
        get => cardData;
        set => cardData = value;
    }
    
    [SerializeField] private Image cardImage;
    [SerializeField] private Image rarityLevelFrameImage;
    [SerializeField] private Image[] typeIconImages;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardRarityLevelText;
    [SerializeField] private TMP_Text cardAttackRangeText;
    [SerializeField] private TMP_Text cardMovementRangeText;
    private int cardRarityLevel;
    private CardData.CardType flags;
    private bool pointerAlreadyEntered = false;


    private Color[] rarityColors =
    {
        Color.grey, // Common
        Color.green, // Uncommon
        Color.blue, // Rare
        Color.magenta, // Epic
        Color.yellow // Unique
    };


    private void Start()
    {
        flags = CardData.TypeFlags;
        UpdateCardDisplay();
    }

    public void OnPointerEnter(PointerEventData eventData) // For hovering effect
    {
        Debug.Log($"Mouse entered {gameObject.name} + Pointer already entered : {pointerAlreadyEntered}");

        if (!pointerAlreadyEntered)
        {   
            Vector2 hoverPos = new Vector2(transform.position.x, transform.position.y);
            Vector3 hoverRot = transform.rotation.eulerAngles;
            CardHoverSystem.Instance.Show(CardData, hoverPos, hoverRot);
            pointerAlreadyEntered = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("The cursor exited the selectable UI element.");
        pointerAlreadyEntered = false;
        CardHoverSystem.Instance.Hide();
    }

    public void UpdateCardDisplay()
    {
        Debug.Log($"Updating card {cardData?.name ?? "NULL"}");

        cardNameText.text = CardData.CardName; // Update title

        // Update frame color & displayed rarity level based on card's rarity level enum index :
        cardRarityLevel = (int)CardData.CardRarityLevel;

        rarityLevelFrameImage.color = rarityColors[cardRarityLevel];
        cardRarityLevelText.text = cardRarityLevel.ToString();

        // Update range :
        cardAttackRangeText.gameObject.SetActive(false);
        cardMovementRangeText.gameObject.SetActive(false);

        if (CardData is SpecialCard specialCard)
        {
            bool hasAttack = specialCard.AttackBehaviour != null; // Set bool to true if Attack Behaviour not null
            bool hasMovement = specialCard.MovementBehaviour != null; // Idem with Movement Behaviour

            if (hasAttack && specialCard.AttackBehaviour.AttackRange > 0)
            {
                cardAttackRangeText.text = specialCard.AttackBehaviour.AttackRange.ToString();
                cardAttackRangeText.gameObject.SetActive(true);
            }

            if (hasMovement && specialCard.MovementBehaviour.MovementRange > 0)
            {
                cardMovementRangeText.text = specialCard.MovementBehaviour.MovementRange.ToString();
                cardMovementRangeText.gameObject.SetActive(true);
            }
        }

        else if (CardData is AttackCard attackCard && attackCard.AttackRange > 0)
        {
            cardAttackRangeText.text = attackCard.AttackRange.ToString();
            cardAttackRangeText.gameObject.SetActive(true);
        }

        else if (CardData is MovementCard movementCard && movementCard.MovementRange > 0)
        {
            cardMovementRangeText.text = movementCard.MovementRange.ToString();
            cardMovementRangeText.gameObject.SetActive(true);
        }

        // Resetting all icons to inactive
        foreach (Image icon in typeIconImages)
        {
            if (icon != null) icon.gameObject.SetActive(false);
        }

        // Activate only the icons that match card's type(s)
        foreach (CardData.CardType type in Enum.GetValues(typeof(CardData.CardType))) // For each given type 
        {
            if (type == CardData.CardType.None) continue; // Skip bit 0

            if ((flags & type) != 0) // Type is enabled in flags
            {
                int index = (int)type; // Getting enum index in my card's list of active type(s)

                if (index >= 0 && index < typeIconImages.Length) typeIconImages[index].gameObject.SetActive(true);

            }
        }
    }
}
