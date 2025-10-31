public class AttackCard : Card
{
    public uint Damages { get; private set; }
    private HealthSystem target;
    
    public uint Range { get; private set; }

    public override void Play()
    {
        Attack(target);
    }

    private void Attack(HealthSystem target)
    {
        target.LooseHP(Damages);
    }
}