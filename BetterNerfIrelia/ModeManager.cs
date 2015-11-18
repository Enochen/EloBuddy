namespace BetterNerfIrelia
{
    using System;
    using System.Collections.Generic;

    using BetterNerfIrelia.Modes;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Utils;

    public static class ModeManager
    {
        private static readonly List<ModeBase> Modes;

        private static int kills, lastEmote;

        static ModeManager()
        {
            Modes = new List<ModeBase>();

            Modes.AddRange(
                new ModeBase[]
                    {
                        new KillSteal(),
                        new Combo(),
                        new Harass(),
                        new LaneClear(),
                      //new JungleClear(),
                      //new LastHit(),
                      //new Flee()
                    });

            Game.OnTick += OnTick;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender.IsEnemy && sender.IsValidTarget(SpellManager.E.Range) && SpellManager.E.IsInRange(sender) && SpellManager.E.IsReady() && Config.Modes.Misc.UseEGC)
            {
                SpellManager.E.Cast(sender);
            }
        }

        public static void Initialize()
        {
        }

        private static void OnTick(EventArgs args)
        {
            //if (Player.Instance.ChampionsKilled > kills)
            //{
            //    kills = Player.Instance.ChampionsKilled;
            //    if (Environment.TickCount - lastEmote > 10000)
            //    {
            //        lastEmote = Environment.TickCount;
            //        Player.DoMasteryBadge();
            //    }
            //}

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
        }
    }
}