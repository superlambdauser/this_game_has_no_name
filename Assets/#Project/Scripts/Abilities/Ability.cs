using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that has an effect on the board
/// </summary>
[System.Serializable]
public abstract class Ability
{
    public abstract AbilityCategory Category { get; }
    
    public abstract GameAction GetGameAction();
    public enum AbilityCategory
    {
        Attack,
        Movement
    }
}
