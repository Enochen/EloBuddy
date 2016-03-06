namespace Rice.Modes
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class TearStack : ModeBase
    {
        public int LastTick;

        public override bool ShouldBeExecuted()
        {
            return Config.Modes.TearStack.AutoStackQ;
        }

        public override void Execute()
        {
            //No enemies, not recalling, required mana, not in base, too many stacks, Q not ready, etc
            if (EntityManager.MinionsAndMonsters.CombinedAttackable.Any(x => x.IsValidTarget(Q.Range + 50))) { return; }
            if (EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(Q.Range + 100))) { return; }
            if (Player.Instance.IsRecalling() || (Config.Modes.TearStack.OnlyFountain && !Shop.CanShop)) { return; }
            if (Player.Instance.ManaPercent < Config.Modes.TearStack.AutoStackMana) { return; }
            if (ModeManager.PassiveCount >= Config.Modes.TearStack.MaxStacks || !Q.IsReady() || Game.CursorPos.IsZero) { return; }
            if ((Environment.TickCount - LastTick < Config.Modes.TearStack.StackTimer * 1000 - (100 + (Game.Ping / 2)))) { return; }

            LastTick = Environment.TickCount;
            Q.Cast(Game.CursorPos);
        }
    }
}