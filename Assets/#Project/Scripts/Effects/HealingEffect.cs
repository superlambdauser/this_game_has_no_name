using UnityEngine;

[System.Serializable]
public class HealingEffect : Effect
{
    [SerializeField] private int healAmount;

    public override GameAction GetGameAction()
    {
        return new HealActionGA(healAmount);
    }
}
