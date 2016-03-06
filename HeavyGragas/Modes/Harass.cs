namespace HeavyGragas.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)
                   && Player.Instance.ManaPercent > Config.Modes.Harass.Mana;
        }

        public override void Execute()
        {
        }
    }
}