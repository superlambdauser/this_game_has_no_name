using UnityEngine;

[System.Serializable]
public class BasicHealing : SpecialAbility
{
    [SerializeField] private uint healAmount;
    public uint HealAmount => healAmount;
    private HealthSystem targetToHeal;
    public HealthSystem TargetToHeal => targetToHeal;


    public override void Perform()
    {
        Heal(targetToHeal);
    }
    
    private void Heal(HealthSystem target)
    {
        target.GainHP(healAmount);
    }
}
