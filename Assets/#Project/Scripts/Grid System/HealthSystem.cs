using UnityEngine;

public abstract class HealthSystem : MonoBehaviour
{
    public uint MaxHealth { get; private set; }
    public uint Health { get; private set; }

    public void SetHealth(uint amount)
    {
        if (amount > MaxHealth) Health = MaxHealth;
        else if (amount < 0) return;
        else Health = amount;
    }

    public void GainHP(uint amount)
    {
        if (Health + amount > MaxHealth) Health = MaxHealth;
        // else if (amount < 0) return; // Don need it anymore since i set up everything in uint
        else Health += amount;
    }

    public void LooseHP(uint amount)
    {
        if (Health - amount < 0) Health = 0;
        else Health -= amount;
    }
}
