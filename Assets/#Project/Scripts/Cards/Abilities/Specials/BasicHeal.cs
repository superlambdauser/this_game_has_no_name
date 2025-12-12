using UnityEngine;

[System.Serializable]
public class BasicHealing : SpecialAbility
{
    [SerializeField] private uint healAmount;
    public uint HealAmount => healAmount;
    private FigureData targetToHeal;
    public FigureData TargetToHeal => targetToHeal;


    public override void Perform()
    {
        Heal(targetToHeal);
    }
    
    private void Heal(FigureData target)
    {
        target.GainHP(healAmount);
    }
}
