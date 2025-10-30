public class AttackCard : Card
{
    public uint Damages { get; private set; }
    private HealthSystem target;

    public override void Play()
    {
        Attack(target);
    }

    private void Attack(HealthSystem target)
    {
        target.LooseHP(Damages);
    }
}