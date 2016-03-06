using Settings = Reborn_Leona.Config.Modes.Combo;

namespace Reborn_Leona.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                   && Player.Instance.ManaPercent > Settings.Mana;
        }

        public override void Execute()
        {
            var shouldQ = Settings.UseQ && Q.IsReady();
            var shouldW = Settings.UseW && W.IsReady();
            var shouldE = Settings.UseE && E.IsReady();
            var shouldR = Settings.UseR && R.IsReady();

            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            var targetR = TargetSelector.GetTarget(R.Range, DamageType.Magical);

            if (targetR != null && shouldR)
            {
                var rHit = Player.Instance.ServerPosition.To2D().GetBestRPos();
                if (rHit.Value >= Config.Modes.Misc.REnemiesHit)
                {
                    R.Cast(rHit.Key.To3DWorld());
                }
            }

            if (target != null)
            {
                if (Player.Instance.IsInAutoAttackRange(target))
                {
                    if (shouldW)
                    {
                        W.Cast();
                    }

                    if (shouldQ)
                    {
                        ModeManager.ShouldQ = true;
                    }
                }

                if (Player.Instance.Distance(target) > Player.Instance.GetAutoAttackRange() + 25)
                {
                    if (shouldE)
                    {
                        E.Cast(target);
                    }
                }
            }
        }
    }
}