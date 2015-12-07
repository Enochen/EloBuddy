namespace Rice
{
    using System;
    using System.Collections.Generic;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Utils;

    using Rice.Modes;

    public static class ModeManager
    {
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
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

            Modes.AddRange(
                new ModeBase[]
                    {
                        new PermaActive(), new Combo(), new LaneClear(), new JungleClear(), new Harass(), new LastHit(),
                        new PassiveStack()
                        //new TearStack()
                        //new Flee()
                    });

            Game.OnUpdate += OnUpdate;
            Spellbook.OnCastSpell += OnCastSpell;

            if (Config.Modes.Misc.AutoWGapCloser)
            {
                Gapcloser.OnGapcloser += OnGapCloser;
            }

            if (Config.Modes.Misc.AutoWInterruptible)
            {
                Interrupter.OnInterruptableSpell += OnInterruptibleSpell;
            }
        }

        private static void OnInterruptibleSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (args.DangerLevel > DangerLevel.Medium && sender.IsEnemy && sender.IsValidTarget(SpellManager.W.Range)
                && SpellManager.W.IsReady())
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
            if (!sender.Owner.IsMe
                || (args.Slot != SpellSlot.E && args.Slot != SpellSlot.W && args.Slot != SpellSlot.Q
                    && args.Slot != SpellSlot.R) || LastSpell == args.Slot)
            {
                return;
            }
            LastSpell = args.Slot;
        }

        public static void Initialize()
        {
        }

        private static void OnUpdate(EventArgs args)
        {
            Orbwalker.DisableAttacking = false;
            if (Config.Modes.Humanizer.Humanize)
            {
                if (!Humanizer.CheckDelay(Humanizer.General))
                {
                    return;
                }
                Humanizer.ChangeDelay(
                    Humanizer.General,
                    new Random().Next(Config.Modes.Humanizer.MinDelay, Config.Modes.Humanizer.MaxDelay));
            }
            Modes.ForEach(
                mode =>
                    {
                        try
                        {
                            if (!mode.ShouldBeExecuted()) { return; }
                            mode.Execute();
                        }
                        catch (Exception e)
                        {
                            Logger.Log(LogLevel.Error, "Error in mode '{0}'\n{1}", mode.GetType().Name, e);
                        }
                    });
        }
    }
}