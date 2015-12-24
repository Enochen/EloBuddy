using Settings = Rice.Config.Modes.LaneClear;

namespace Rice.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) && Player.Instance.ManaPercent > Settings.Mana;
        }

        public override void Execute()
        {
            var monsters = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(Q.Range)).OrderByDescending(x => x.MaxHealth);

            var shouldQ = Settings.UseQ && Q.IsReady();
            var shouldW = Settings.UseW && W.IsReady() && ModeManager.LastSpell != SpellSlot.E;
            var shouldE = Settings.UseE && E.IsReady() && ModeManager.LastSpell != SpellSlot.W;
            var shouldR = Settings.UseR && R.IsReady();

            var stacks = ModeManager.PassiveCount + new[] { shouldQ, shouldW, shouldE, shouldR }.Count(x => x);

            foreach (var monster in monsters)
            {
                switch (ModeManager.PassiveCount)
                {
                    case 2:
                        if (Q.IsReady() && W.IsReady() && E.IsReady())
                        {
                            if (shouldE)
                            {
                                E.Cast(monster);
                                if (shouldQ)
                                {
                                    Q.PredCast(monster);
                                }
                            }
                            if (shouldQ && !Q.GetPrediction(monster).Collision)
                            {
                                Q.PredCast(monster);
                            }
                        }
                        break;

                    case 4:
                        if (shouldW)
                        {
                            W.Cast(monster);
                            if (shouldQ && !Q.GetPrediction(monster).Collision)
                            {
                                Q.PredCast(monster);
                            }
                        }
                        if (shouldR
                            && (ModeManager.PassiveCharged || SpellDamage.GetTotalDamage(monster) > monster.Health))
                        {
                            R.Cast();
                        }
                        break;
                }

                if (shouldW)
                {
                    W.Cast(monster);
                    if (shouldQ && !Q.GetPrediction(monster).Collision)
                    {
                        Q.PredCast(monster);
                    }
                }
                if (shouldR && (stacks > 3 || ModeManager.PassiveCharged))
                {
                    R.Cast();
                }
                if (shouldE)
                {
                    E.Cast(monster);
                }
                if (shouldQ && !Q.GetPrediction(monster).Collision)
                {
                    Q.PredCast(monster);
                }
            }
        }
    }
}