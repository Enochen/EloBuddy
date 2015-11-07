using Settings = SpaceAIDS.Config.Modes.Utils;

namespace SpaceAIDS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Utils;

    using SharpDX;

    using SpaceAIDS.Modes;

    public static class ModeManager
    {
        private static readonly List<ModeBase> Modes;

        public static bool UsingR;

        private static float rTime;

        static ModeManager()
        {
            Modes = new List<ModeBase>();
            rTime = 0;

            Modes.AddRange(
                new ModeBase[]
                    {
                        new PermaActive(), new Combo(), new Harass()
                        //new LaneClear(),
                        //new JungleClear(),
                        //new LastHit(),
                        //new Flee()
                    });

            Game.OnTick += OnTick;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
            Player.OnIssueOrder += OnIssueOrder;
            Spellbook.OnCastSpell += OnCastSpell;
        }

        private static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot != SpellSlot.R)
            {
                //Player.IssueOrder(GameObjectOrder.MoveTo, (Vector3)Player.Instance.Position.Extend(Game.CursorPos2D, 1));
                return;
            }
            if (SpellManager.R.IsReady() && args.Target.IsValid)
            {
                Core.DelayAction(delegate { rTime = Game.Time; }, 50);
            }
        }

        private static void OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
        }

        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (sender.IsValidTarget() && args.DangerLevel > DangerLevel.Medium && Settings.UseQOnInterruptable)
            {
                SpellManager.Q.Cast(sender);
            }
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            if (sender.IsValidTarget() && Settings.UseQOnGapcloser && sender.IsTargetable)
            {
                SpellManager.Q.Cast(args.End);
            }
        }

        public static void Initialize()
        {
        }

        private static void OnTick(EventArgs args)
        {
            Modes.ForEach(
                mode =>
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

            if ((Player.Instance.Spellbook.IsChanneling || Game.Time - rTime < 0.5)
                && EntityManager.Heroes.AllHeroes.Count(x => x.HasBuff("AlZaharNetherGrasp")) > 0)
            {
                UsingR = true;
                Orbwalker.DisableMovement = true;
                Orbwalker.DisableAttacking = true;
            }
            else
            {
                UsingR = false;
                Orbwalker.DisableMovement = false;
                Orbwalker.DisableAttacking = false;
            }
        }
    }
}