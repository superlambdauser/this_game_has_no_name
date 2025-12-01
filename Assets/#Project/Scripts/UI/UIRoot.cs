using UnityEngine;

/// <summary>
/// Holds references to slots in the Canvas where UI elements should appear
/// </summary>
// Everything assigned manually in the inspector cleanly & explicitly
public class UIRoot : MonoBehaviour
{
    [Header("Cards & Hand area :")]
    [SerializeField] private RectTransform handsViewParent;
    public RectTransform HandsViewParent => handsViewParent;

    [Header("Buttons :")]
    [SerializeField] private RectTransform gameplayButtonsParent;
    public RectTransform GameplayButtonsParent;

    // Later : Probably will add Menu buttons etc.

}