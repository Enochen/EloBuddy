using Settings = SpaceAIDS.Config.Modes.Harass;

namespace SpaceAIDS.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (Settings.UseQ && Q.IsReady())
            {
                if (target.IsValidTarget(Q.Range))
                {
                    Q.Cast(target);
                }
            }
            if (Settings.UseW && W.IsReady())
            {
                if (target.IsValidTarget(W.Range))
                {
                    W.Cast(target);
                }
            }
            if (Settings.UseE && E.IsReady())
            {
                if (target.IsValidTarget(E.Range))
                {
                    E.Cast(target);
                }
            }
            if (Settings.UseR && R.IsReady())
            {
                if ((target.IsValidTarget(R.Range)) && R.GetRealDamage(target) > target.Health)
                {
                    this.R.Cast(target);
                }
            }
        }
    }
}