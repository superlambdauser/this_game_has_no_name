using UnityEngine;

/// <summary>
/// Small static factory helper that instantiates UI prefabs in a single point entry pattern
/// NB : Must be used with a UIRoot : MonoBehaviour in a GameInitiator
/// </summary>
/// - UI creation consistent and clean
/// - Parents always assigned correctly
public static class UIBuilder
{
    /// <summary>
    /// Instatiates a UI prefab that has a Component type, place it under a given parent & return it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefab"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static T Initiate<T>(T prefab, RectTransform parent) where T : Component
    {
        return Object.Instantiate(prefab, parent, false);
    }

    /// <summary>
    /// Instantiates a plain GameObject and parents it to the given UI slot & return it
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject Initiate(GameObject prefab, RectTransform parent)
    {
        return Object.Instantiate(prefab, parent);
    }
}
