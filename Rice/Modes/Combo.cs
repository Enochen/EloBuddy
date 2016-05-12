namespace Rice.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                   && Player.Instance.ManaPercent > Config.Modes.Combo.Mana;
        }

        public override void Execute()
        {
            if (Config.Modes.Combo.blockAA) { Orbwalker.DisableAttacking = true; }

            var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }

            var shouldQ = Config.Modes.Combo.UseQ && Q.IsReady();
            var shouldW = Config.Modes.Combo.UseW && W.IsReady() && ModeManager.LastSpell != SpellSlot.E;
            var shouldE = Config.Modes.Combo.UseE && E.IsReady() && ModeManager.LastSpell != SpellSlot.W;
            var shouldR = Config.Modes.Combo.UseR && R.IsReady();

            var stacks = ModeManager.PassiveCount + new[] { shouldQ, shouldW, shouldE, shouldR }.Count(x => x);
            
            switch (ModeManager.PassiveCount)
            {
                case 1:
                case 2:
                    if (shouldR)
                    {
                        R.Cast();
                    }
                    else
                    {
                        if (shouldE)
                        {
                            E.Cast(target);
                        }
                        else
                        {
                            Q.PredCast(target);
                        }
                    }
                    break;

                case 3:
                    if (shouldE)
                    {
                        E.Cast(target);
                    }
                    else
                    {
                        Q.PredCast(target);
                    }
                    break;

                case 4:
                    W.Cast(target);
                    break;

                default:
                    if (shouldQ)
                    {
                        Q.PredCast(target);
                    }
                    else if (shouldE)
                    {
                        E.Cast(target);
                    }
                    else
                    {
                        W.Cast(target);
                    }
                    break;
            }
        }
    }
}