using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image rarityLevelFrameImage;
    [SerializeField] private Image[] typeIconImages;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardRarityLevelText;
    private int cardRarityLevel;



    private Color[] rarityColors =
    {
        Color.grey, // Common
        Color.green, // Uncommon
        Color.blue, // Rare
        Color.magenta, // Epic
        Color.yellow // Unique
    };


    public void Start()
    {
        UpdateCardDisplay();
    }


    private void UpdateCardDisplay()
    {
        Debug.Log($"Updating card {cardData?.name ?? "NULL"}");

        cardNameText.text = cardData.CardName; // Update title

        // Update frame color & displayed rarity level based on card's rarity level enum index :
        cardRarityLevel = (int)cardData.CardRarityLevel;

        rarityLevelFrameImage.color = rarityColors[cardRarityLevel];
        cardRarityLevelText.text = cardRarityLevel.ToString();

        // Resetting all icons to inactive
        foreach (Image icon in typeIconImages)
        {
            if (icon != null) icon.gameObject.SetActive(false);
        }
        
        // Activate only the icons that match card's type(s)
        foreach (Card.CardType cardType in cardData.CardTypes) // For each given type 
        {
            int index = (int)cardType; // Getting enum index in my card's list of active type(s)
            if (typeIconImages[index] != null && index >= 0 && index < typeIconImages.Length) // Safety check
            {
                typeIconImages[index].gameObject.SetActive(true); 
            }
        }
    }
}
