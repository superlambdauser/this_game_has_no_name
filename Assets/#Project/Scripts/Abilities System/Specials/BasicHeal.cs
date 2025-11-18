using UnityEngine;

public class BasicHealing : ISpecialAbility
{
    [SerializeField] private uint healAmount;
    public uint HealAmount => healAmount;
    private HealthSystem targetToHeal;
    public HealthSystem TargetToHeal => targetToHeal;


    public void Perform()
    {
        Heal(targetToHeal);
    }
    
    private void Heal(HealthSystem target)
    {
        target.GainHP(healAmount);
    }
}
