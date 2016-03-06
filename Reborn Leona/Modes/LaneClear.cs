using Settings = Reborn_Leona.Config.Modes.LaneClear;

namespace Reborn_Leona.Modes
{
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                   && Player.Instance.ManaPercent > Config.Modes.LaneClear.Mana;
        }

        public override void Execute()
        {
            var targets = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.Distance(Player.Instance) < R.Range) as Obj_AI_Minion[];

            if (targets == null)
            {
                return;
            }

            var shouldQ = Settings.UseQ && Q.IsReady();
            var shouldW = Settings.UseW && W.IsReady();
            var shouldE = Settings.UseE && E.IsReady();
            var shouldR = Settings.UseR && R.IsReady();

            if (shouldW && targets.Length > 2)
            {
                W.Cast();
            }

            foreach (var target in targets)
            {
                if (target == null)
                {
                    return;
                }

                var canQKill = target.Health <= Q.GetRealDamage(target);
                var canEKill = target.Health <= E.GetRealDamage(target);
                var canRKill = target.Health <= R.GetRealDamage(target);

                if (shouldR && canRKill)
                {
                    var rHit = Player.Instance.ServerPosition.To2D().GetBestRPos(target);
                    if (rHit.Value >= Config.Modes.Misc.REnemiesHit)
                    {
                        R.Cast(rHit.Key.To3DWorld());
                    }
                }

                if (target.IsValidTarget(E.Range) && shouldE && canEKill)
                {
                    E.Cast(target);
                }

                if (target.IsValidTarget(Q.Range) && shouldQ && canQKill)
                {
                    ModeManager.ShouldQ = true;
                }
            }
        }
    }
}