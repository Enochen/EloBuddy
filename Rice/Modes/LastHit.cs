using Settings = Rice.Config.Modes.LastHit;

namespace Rice.Modes
{
    using System.Linq;

    using EloBuddy.SDK;

    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            if (Orbwalker.IsAutoAttacking) { return; }
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget(Q.Range)).OrderBy(x=>x.Health);

            var shouldQ = Settings.UseQ && Q.IsReady();
            var shouldW = Settings.UseW && W.IsReady();
            var shouldE = Settings.UseE && E.IsReady();
            var shouldR = Settings.UseR && R.IsReady();

            foreach (var minion in minions)
            {
                var health = minion.Health;

                if (minion.IsDead)
                {
                    continue;
                }
                if (shouldQ && Q.GetRealDamage(minion) > health + 50 && !Q.GetPrediction(minion).Collision && !minion.IsDead)
                {
                    Orbwalker.ForcedTarget = null;
                    SpellManager.Q2.PredCast(minion);
                }
                if (shouldW && W.GetRealDamage(minion) > health)
                {
                    W.Cast(minion);
                }
                if (shouldE && E.GetRealDamage(minion) > health)
                {
                    E.Cast(minion);
                }
                if (shouldR && Q.GetRealDamage(minion) > health)
                {
                    R.Cast();
                }
            }
        }
    }
}