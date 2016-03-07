using Settings = HeavyGragas.Config.Modes.Combo;

namespace HeavyGragas.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

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

            var target = TargetSelector.GetTarget(Q.Range + 200, DamageType.Magical);
            if (target == null) { return; }

            var predPos = Q.GetPrediction(target).CastPosition;

            if (Player.Instance.HasBuff("GragasWAttackBuff") && Player.Instance.IsInAutoAttackRange(target))
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }

            if (shouldW)
            {
                W.Cast();
            }

            if (target.IsValidTarget(E.Range))
            {
                if (shouldE)
                {
                    CastE(target);

                    CastQ1(predPos);
                }
            }

            var qPred = Q.GetPrediction(target);

            if (shouldQ && qPred.HitChance >= HitChance.High)
            {
                CastQ1(qPred.CastPosition);
            }
        }
    }
}