using Settings = SpaceAIDS.Config.Modes.Combo;

namespace SpaceAIDS.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (ModeManager.UsingR)
            {
                return;
            }
            if (Settings.UseQ && Q.IsReady())
            {
                if (target.IsValidTarget(Q.Range))
                { 
                    Q.Cast(Q.GetPrediction(target).CastPosition);
                }
            }
            if (Settings.UseW && W.IsReady())
            {
                if (target.IsValidTarget(W.Range))
                {
                    W.Cast(W.GetPrediction(target).CastPosition);
                }
            }
            if (Settings.UseE && E.IsReady())
            {
                if (target.IsValidTarget(E.Range))
                {
                    E.Cast(target);
                }
            }
            if ((Fire != null) && Settings.UseIgnite && Fire.IsReady())
            {
                if ((target.IsValidTarget(Fire.Range)) && Fire.GetRealDamage(target) >= target.Health)
                {
                    this.Fire.Cast(target);
                }
            }
            if (Settings.UseR && R.IsReady() && !Q.IsReady(500) && !W.IsReady(500) && !E.IsReady(500))
            {
                if ((target.IsValidTarget(R.Range)) && R.GetRealDamage(target) <= target.Health)
                {
                    this.R.Cast(target);
                    ModeManager.UsingR = true;
                }
            }
        }
    }
}