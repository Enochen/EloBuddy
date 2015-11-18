using Settings = BetterNerfIrelia.Config.Modes.KillSteal;

namespace BetterNerfIrelia.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class KillSteal : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            var useQ = Q.IsReady() && Settings.UseQ;
            var useW = W.IsReady() && Settings.UseW;
            var useE = E.IsReady() && Settings.UseE;
            var useR = R.IsReady() && Settings.UseR;

            var target =
                TargetSelector.GetTarget(
                    EntityManager.Heroes.Enemies.Where(
                        x =>
                        (useQ && Q.IsInRange(x))
                        || (useE && E.IsInRange(x))
                        || (useR && R.IsInRange(x))),
                    DamageType.Mixed);

            if (target == null)
            {
                return;
            }

            if (SpellDamage.GetTotalDamage(target, true) > target.Health)
            {
                if (useQ && Q.IsInRange(target))
                {
                    if (useW)
                    {
                        W.Cast();
                    }
                    Q.Cast(target);
                }
                if (useE && E.IsInRange(target))
                {
                    E.Cast(target);
                }
                if (useR && R.IsInRange(target) && Player.Instance.HasBuff("ireliatranscendentbladesspell"))
                {
                    R.Cast(target);
                }
            }
        }
    }
}