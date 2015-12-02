using System;
using System.Collections.Generic;
using Rice.Modes;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Utils;

namespace Rice
{
    using System.Linq;

    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;

    public static class ModeManager
    {
        private static List<ModeBase> Modes { get; set; }

        public static SpellSlot LastSpell;

        public static int PassiveCount
        {
            get
            {
                var data = Player.Instance.GetBuff("RyzePassiveStack");
                if (data == null || data.Count == -1)
                {
                    return 0;
                }
                return data.Count == 0 ? 1 : data.Count;
            }
        }
        public static bool PassiveCharged
        {
            get
            {
                return Player.Instance.HasBuff("ryzepassivecharged");
            }
        }

        static ModeManager()
        {
            Modes = new List<ModeBase>();

            Modes.AddRange(new ModeBase[]
            {
                new PermaActive(),
                new Combo(),
                new LaneClear(),
                new JungleClear(),
                new Harass(),
                new LastHit(),
                new PassiveStack(),
                //new TearStack()
                //new Flee()
            });

            Game.OnTick += OnTick;
            Spellbook.OnCastSpell += OnCastSpell;

            if(Config.Modes.Misc.AutoWGapCloser)
            Gapcloser.OnGapcloser += OnGapCloser;

            if(Config.Modes.Misc.AutoWInterruptible)
            Interrupter.OnInterruptableSpell += OnInterruptibleSpell;

        }

        private static void OnInterruptibleSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (args.DangerLevel > DangerLevel.Medium && sender.IsEnemy && sender.IsValidTarget(SpellManager.W.Range) && SpellManager.W.IsReady())
            {
                SpellManager.W.Cast(sender);
            }
        }

        private static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            if (sender.IsEnemy && sender.IsValidTarget(SpellManager.W.Range) && SpellManager.W.IsReady())
            {
                SpellManager.W.Cast(sender);
            }
        }

        private static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!sender.Owner.IsMe || (args.Slot != SpellSlot.E && args.Slot != SpellSlot.W && args.Slot != SpellSlot.Q && args.Slot != SpellSlot.R) || LastSpell == args.Slot)
            {
                return;
            }
            LastSpell = args.Slot;
        }

        public static void Initialize()
        {
        }

        private static void OnTick(EventArgs args)
        {
            Orbwalker.DisableAttacking = false;
            Modes.ForEach(mode =>
            {
                try
                {
                    if (mode.ShouldBeExecuted())
                    {
                        mode.Execute();
                    }
                }
                catch (Exception e)
                {
                    Logger.Log(LogLevel.Error, "Error in mode '{0}'\n{1}", mode.GetType().Name, e);
                }
            });
        }
    }
}
