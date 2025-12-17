using UnityEngine;

public class HealActionGA : GameAction
{
    public int Amount { get; set; }

    public HealActionGA(int amount)
    {
        Amount = amount;
    }
}
