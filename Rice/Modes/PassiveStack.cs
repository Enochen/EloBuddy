using Settings = Rice.Config.Modes.AutoStack;

namespace Rice.Modes
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class PassiveStack : ModeBase
    {
        public int LastTick;

        public override bool ShouldBeExecuted()
        {
            return Settings.AutoStackQ;
        }

        public override void Execute()
        {
            //No enemies, not recalling, required mana, too many stacks, Q not ready, etc
            if (EntityManager.MinionsAndMonsters.CombinedAttackable.Any(x => x.IsValidTarget(Q.Range + 50))) { return; }
            if (EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(Q.Range + 100))) { return; }
            if (Player.Instance.IsRecalling() || Game.CursorPos.IsZero) { return; }
            if (Player.Instance.ManaPercent < Settings.AutoStackMana) { return; }
            if (ModeManager.PassiveCount >= Settings.MaxStacks || !Q.IsReady()) { return; }
            if ((Environment.TickCount - LastTick < Settings.StackTimer * 1000 - (100 + (Game.Ping / 2)))) { return; }

            LastTick = Environment.TickCount;
            Q.Cast(Game.CursorPos);
        }
    }
}