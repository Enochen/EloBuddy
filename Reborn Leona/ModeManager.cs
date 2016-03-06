namespace Reborn_Leona
{
    using System;
    using System.Collections.Generic;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Constants;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Utils;

    using Reborn_Leona.Modes;

    public static class ModeManager
    {
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        private static List<ModeBase> Modes { get; set; }

        public static bool ShouldQ, ShouldAA;

        public static GameObject QTarget = Player.Instance;

        static ModeManager()
        {
            Modes = new List<ModeBase>();

            Modes.AddRange(
                new ModeBase[]
                    { new PermaActive(), new Combo(), new LaneClear(), new JungleClear(), new Harass(), new LastHit()});

            Game.OnUpdate += OnUpdate;
            
            Obj_AI_Base.OnSpellCast += OnSpellCast;
            Gapcloser.OnGapcloser += OnGapCloser;
            Interrupter.OnInterruptableSpell += OnInterruptibleSpell;
        }

        private static void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.IsAutoAttack())
                {
                    if (Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None)
                    {
                        if (ShouldAA)
                        {
                            Orbwalker.ResetAutoAttack();
                            if (Player.Instance.IsInAutoAttackRange(args.Target as AttackableUnit))
                            {
                                Player.IssueOrder(GameObjectOrder.AttackTo, args.Target);
                            }
                            ShouldAA = false;
                            return;
                        }
                        if (ShouldQ)
                        {
                            SpellManager.Q.Cast();
                            Orbwalker.ResetAutoAttack();
                            if (Player.Instance.IsInAutoAttackRange(args.Target as AttackableUnit) )
                            {
                                Player.IssueOrder(GameObjectOrder.AttackTo, args.Target);
                            }
                            ShouldAA = true;
                            ShouldQ = false;
                            return;
                        }
                    }
                }
                if (args.Slot == SpellSlot.Q)
                {
                    Orbwalker.ResetAutoAttack();
                }
            }
        }

        private static void OnInterruptibleSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!(args.DangerLevel > DangerLevel.Medium) || !sender.IsEnemy
                || !sender.IsInAutoAttackRange(Player.Instance) || !SpellManager.Q.IsReady()
                || !Config.Modes.Misc.AutoQInterruptible)
            {
                return;
            }
            SpellManager.Q.Cast();
            Player.IssueOrder(GameObjectOrder.AttackUnit, sender);
        }

        private static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!sender.IsEnemy || !sender.IsInAutoAttackRange(Player.Instance) || !SpellManager.Q.IsReady()
                || !Config.Modes.Misc.AutoQGapCloser)
            {
                return;
            }
            SpellManager.Q.Cast();
            Player.IssueOrder(GameObjectOrder.AttackUnit, sender);
        }

        public static void Initialize()
        {
        }

        private static void OnUpdate(EventArgs args)
        {
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