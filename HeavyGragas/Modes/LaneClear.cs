using Settings = HeavyGragas.Config.Modes.LaneClear;

namespace HeavyGragas.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                   && Player.Instance.ManaPercent > Settings.Mana;
        }

        public override void Execute()
        {
            var targets =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.Distance(Player.Instance) < R.Range) as
                Obj_AI_Minion[];

            if (targets == null)
            {
                return;
            }

            var shouldQ = Settings.UseQ && Q.IsReady();
            var shouldW = Settings.UseW && W.IsReady();
            var shouldE = Settings.UseE && E.IsReady();
            var shouldR = Settings.UseR && R.IsReady();
            
        }
    }
}