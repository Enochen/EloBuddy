namespace Rice.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && Player.Instance.ManaPercent > Config.Modes.LastHit.Mana;
        }

        public override void Execute()
        {
            if (Orbwalker.IsAutoAttacking) { return; }
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget(Q.Range)).OrderBy(x=>x.Health);

            var shouldQ = Config.Modes.LastHit.UseQ && Q.IsReady();

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
            }
        }
    }
}