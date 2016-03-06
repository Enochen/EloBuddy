namespace Reborn_Leona.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Settings = Config.Modes.KillSteal;

    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            var shouldQ = Settings.UseQ && Q.IsReady();
            var shouldE = Settings.UseE && E.IsReady();
            var shouldR = Settings.UseR && R.IsReady();

            var targets = EntityManager.Heroes.Enemies.Where(x => x.Distance(Player.Instance) < R.Range);

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
                    Q.Cast(target);
                }
            }
        }
    }
}