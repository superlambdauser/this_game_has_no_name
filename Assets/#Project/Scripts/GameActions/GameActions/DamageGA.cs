using UnityEngine;

public class AttackGA : GameAction
{
    public int Amount { get; set; }
    public int Range { get; set; }
    public AttackGA(int amount, int range)
    {
        Amount = amount;
        Range = range;
    }
}
