using Settings = BetterNerfIrelia.Config.Modes.Combo;

namespace BetterNerfIrelia.Modes
{
    using System.Linq;

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
            var target = TargetSelector.GetTarget(R.Range, DamageType.Mixed);
            if (target == null)
            {
                return;
            }
            if (Orbwalker.IsAutoAttacking && W.IsReady() && Settings.UseW)
            {
                W.Cast();
            }
            if (Settings.UseQ && Q.IsReady())
            {
                var minion =
                    EntityManager.MinionsAndMonsters.CombinedAttackable.Where(
                        m => Q.GetRealDamage(m) > m.Health && m.Distance(target) < Player.Instance.Distance(target) && Q.IsInRange(m)).OrderBy(m => m.Distance(target)).FirstOrDefault();
                if (minion.IsValidTarget())
                {
                    Q.Cast(target);
                }
                if (!Player.Instance.IsInAutoAttackRange(target) && Player.Instance.IsInRange(target, Q.Range))
                {
                    if (!W.IsOnCooldown && Settings.UseW)
                    {
                        W.Cast();
                    }
                    Q.Cast(target);
                }
            }
            if (Settings.UseE && E.IsReady() && Player.Instance.IsInRange(target, E.Range))
            {
                if (!W.IsOnCooldown && Settings.UseW)
                {
                    W.Cast();
                }
                E.Cast(target);
            }
            if (Settings.UseR && R.IsReady() && Player.Instance.IsInRange(target, R.Range) && Player.Instance.HasBuff("ireliatranscendentbladesspell"))
            {
                var minion =
                    EntityManager.MinionsAndMonsters.CombinedAttackable.Where(
                        m => R.GetRealDamage(m) + Q.GetRealDamage(m) > m.Health && R.GetRealDamage(m) < m.Health && m.Distance(target) < Player.Instance.Distance(target) && Q.IsInRange(m)).OrderBy(m => m.Distance(target)).FirstOrDefault();
                if (minion.IsValidTarget())
                {
                    R.Cast(minion);
                }
                else
                {
                    R.Cast(target);
                }
            }
            if ((Ignite.Slot != SpellSlot.Unknown) && Settings.UseF && Ignite.IsReady() && Ignite.GetRealDamage(target) > target.Health)
            {
                Ignite.Cast(target);
            }
        }
    }
}