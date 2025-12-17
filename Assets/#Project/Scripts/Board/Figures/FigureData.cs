using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

/// <summary>
/// Holds data for each individual figure on the board.
/// </summary>
public abstract class FigureData : ScriptableObject
{
    [SerializeField] GridData grid; // To initialize later

    [SerializeReference][SR] protected Attack attackSpecs;
    public Attack AttackSpecs => attackSpecs;
    [SerializeReference][SR] protected Movement movementSpecs;
    public Movement MovementSpecs => movementSpecs;
    [SerializeReference] [SR] protected List<Ability> specialAbilities = new List<Ability>();
    public List<Ability> SpecialAbilities => specialAbilities;

    public int CurrentX { get; set; }
    public int CurrentY { get; set; }

    public uint MaxHealth { get; private set; }
    public uint Health { get; private set; }


    public void SetPosition(Vector2Int position)
    {
        CurrentX = position.x;
        CurrentY = position.y;
    }

    protected virtual bool[,] PossibleMove()
    {
        CustomInstructions();
        return new bool[grid.rows, grid.columns]; // Returns an array of rows x columns of booleans that corresponds to the cells of the grid
    }

    protected abstract void CustomInstructions();

    public void SetHealth(uint amount)
    {
        if (amount > MaxHealth) Health = MaxHealth;
        else if (amount < 0) return;
        else Health = amount;
    }

    public virtual void GainHP(uint amount)
    {
        if (Health + amount > MaxHealth) Health = MaxHealth;
        // else if (amount < 0) return; // Don need it anymore since i set up everything in uint
        else Health += amount;
    }

    public virtual void LooseHP(uint amount)
    {
        if (Health - amount < 0) Health = 0;
        else Health -= amount;
    }
}