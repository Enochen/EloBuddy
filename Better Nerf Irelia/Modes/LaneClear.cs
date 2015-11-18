using Settings = BetterNerfIrelia.Config.Modes.LaneClear;

namespace BetterNerfIrelia.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Player.Instance.ManaPercent > Settings.Mana;
        }

        public override void Execute()
        {
            var target =
                EntityManager.MinionsAndMonsters.CombinedAttackable.FirstOrDefault(m => m.Distance(Player.Instance) < Q.Range && Q.GetRealDamage(m) > m.Health);
            if (target == null)
            {
                return;
            }
            if (Orbwalker.IsAutoAttacking && W.IsReady() && Settings.UseW)
            {
                W.Cast();
            }
            if (Settings.UseQ && Q.IsReady() && Player.Instance.IsInRange(target, Q.Range))
            {
                if (!W.IsOnCooldown && Settings.UseW)
                {
                    W.Cast();
                }
                Q.Cast(target);
            }
        }
    }
}