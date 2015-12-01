namespace Rice.Modes
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
            var shouldW = Settings.UseW && W.IsReady() && ModeManager.LastSpell != SpellSlot.E;
            var shouldE = Settings.UseE && E.IsReady() && ModeManager.LastSpell != SpellSlot.W;

            var enemies = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(Q.Range));

            foreach (var enemy in enemies)
            {
                var health = enemy.Health + 50;
                if (shouldQ && !Q.GetPrediction(enemy).Collision && Q.GetRealDamage(enemy) > health)
                {
                    Q.PredCast(enemy);
                }
                if (!enemy.IsValidTarget(W.Range) && !enemy.IsValidTarget(E.Range))
                {
                    continue;
                }
                if (shouldW && W.GetRealDamage(enemy) > health)
                {
                    W.Cast(enemy);
                }
                if (shouldE && E.GetRealDamage(enemy) > health)
                {
                    E.Cast(enemy);
                }
            }
        }
    }
}