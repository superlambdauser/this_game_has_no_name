using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of extensions for list types objects
/// </summary>
public static class ListExtensions
{

    /// <summary>
    /// Draw an element in a given list at a given index & removes it from the list 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T DrawAtIndex<T>(this List<T> list, int index)
    {
        if (list.Count == 0 || list == null) return default; // Make sure there is somthing to draw

        if (index < 0 || index >= list.Count) return default; // Clamp index to a valid range

        T t = list[index];
        list.RemoveAt(index);

        return t;
    }

    /// <summary>
    /// Draw an element in a given list at a random index & removes it from the list 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T DrawRandom<T>(this List<T> list) // "this List<T>" makes this method usable on any List, where T means the item type stored inside that list.
    {
        if (list == null || list.Count == 0) return default; // Double check because we use list.Count before calling DrawAtIndex() and its safety checks

        int rnd = Random.Range(0, list.Count);

        return list.DrawAtIndex(rnd);
    }
}
