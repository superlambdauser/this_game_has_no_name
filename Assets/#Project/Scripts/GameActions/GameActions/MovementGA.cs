using UnityEngine;

public class MovementGA : GameAction
{
    public int Amount { get; set; }

    public MovementGA(int amount)
    {
        Amount = amount;
    }
}
