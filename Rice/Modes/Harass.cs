using EloBuddy;
using EloBuddy.SDK;
using Settings = Rice.Config.Modes.Harass;

namespace Rice.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            // TODO: Add harass logic here
            // See how I used the Settings.UseQ and Settings.Mana here, this is why I love
            // my way of using the menu in the Config class!
            if (/*Settings.UseQ && Player.Instance.ManaPercent > Settings.Mana &&*/ Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
                if (target != null)
                {
                    Q.PredCast(target);
                }
            }
        }
    }
}
