using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Image rarityLevelFrameImage;
    [SerializeField] private Image[] typeIconImages;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardRarityLevelText;
    [SerializeField] private Card cardData;
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

        // Update frame image color based on the first card rarity level :
        rarityLevelFrameImage.color = rarityColors[(int)cardData.CardRarityLevel];

        cardNameText.text = cardData.CardName;
        
        cardRarityLevel = (int)cardData.CardRarityLevel;
        cardRarityLevelText.text = cardRarityLevel.ToString();
    }
}
