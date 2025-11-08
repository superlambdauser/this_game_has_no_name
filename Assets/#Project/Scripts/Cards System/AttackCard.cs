using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Attack Card")]
public class AttackCard : Card
{
    private HealthSystem target; // Probably need to make it a public var since target changes every turn ?
    public HealthSystem Target => target;

    [Header("Attack Cards Traits :")]
    [SerializeField] private uint damages;
    public uint Damages => damages;
    [SerializeField] private uint range;
    public uint Range => range;


    public override void Play()
    {
        Attack(target);
    }

    private void Attack(HealthSystem target)
    {
        target.LooseHP(damages);
    }
}