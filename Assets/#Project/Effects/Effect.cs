using UnityEngine;

/// <summary>
/// Card effect
/// </summary>
[System.Serializable]
public abstract class Effect
{
    /// <summary>
    /// Converts the effect into a GameAction
    /// </summary>
    /// <returns></returns>
    public abstract GameAction GetGameAction();
}