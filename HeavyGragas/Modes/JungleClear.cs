using Settings = HeavyGragas.Config.Modes.LaneClear;

namespace HeavyGragas.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)
                   && Player.Instance.ManaPercent > Settings.Mana;
        }

        public override void Execute()
        {
            var shouldQ = Settings.UseQ && Q.IsReady();
            var shouldW = Settings.UseW && W.IsReady();
            var shouldE = Settings.UseE && E.IsReady();

            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);

            if (target != null)
            {
                
            }
        }
    }
}