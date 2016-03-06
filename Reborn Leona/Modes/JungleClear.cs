using Settings = Reborn_Leona.Config.Modes.LaneClear;

namespace Reborn_Leona.Modes
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