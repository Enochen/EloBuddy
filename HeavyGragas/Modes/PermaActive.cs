namespace HeavyGragas.Modes
{
    using System.Linq;

    using EloBuddy.SDK;

    using Settings = Config.Modes;

    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            if (EventManager.QBarrel != null && Q.IsReady())
            {
                var escapeQ =
                    EntityManager.Heroes.Enemies.Any(
                        x =>
                        x.Distance(EventManager.QBarrel.Position) > Q.Width - 75
                        && x.Distance(EventManager.QBarrel.Position) < Q.Width
                        && !x.IsFacing(EventManager.QBarrel.Position.To2D()));

                var ksQ =
                    EntityManager.Heroes.Enemies.Any(
                        x => x.Distance(EventManager.QBarrel.Position) < Q.Width && Q.GetRealDamage(x) > x.Health);

                //Chat.Print(enemies.First().Distance(ModeManager.QBarrel.Position));
                //Chat.Print(Q.Width);

                if ((escapeQ && Settings.Misc.AutoQ2Enemy) || (ksQ && Settings.KillSteal.UseQ))
                {
                    Q2.Cast();
                }
            }
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(R.Range + R.Width)))
            {
                if (E.IsReady() && Settings.KillSteal.UseE && E.GetRealDamage(enemy) > enemy.Health)
                {
                    CastE(enemy);
                }
                else if (R.IsReady() && Settings.KillSteal.UseR && R.GetRealDamage(enemy) > enemy.Health)
                {
                    R.Cast(enemy);
                }
            }
        }
    }
}