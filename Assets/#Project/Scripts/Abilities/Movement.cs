using UnityEngine;

[System.Serializable]
public  class Movement : Ability // All movements need to inherit from this
{
    public override AbilityCategory Category => AbilityCategory.Movement;
    [SerializeField] private int movementRange;
    public int MovementRange => movementRange;

    public override GameAction GetGameAction()
    {
        MovementGA movementGA = new(movementRange);
        return movementGA;
    }

}
