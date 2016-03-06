namespace HeavyGragas
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;

    using HeavyGragas.Modes;

    public static class EventManager
    {
        public static GameObject QBarrel;

        public static float BarrelTime;

        static EventManager()
        {
            //Barrel Events
            GameObject.OnCreate += OnBarrelCreate;
            GameObject.OnDelete += OnBarrelDelete;

            //Anti-Gapclosers and Interrupters
            Gapcloser.OnGapcloser += OnGapCloser;
            Interrupter.OnInterruptableSpell += OnInterruptibleSpell;

            Dash.OnDash += OnDash;                                                                                    
        }

        private static void OnDash(Obj_AI_Base sender, Dash.DashEventArgs args)
        {
            if (sender.IsEnemy && QBarrel != null)
            {
                var width = SpellManager.Q.Width;
                if (args.StartPos.Distance(QBarrel) < width && args.EndPos.Distance(QBarrel) > width)
                {
                    SpellManager.Q2.Cast();
                }
            }
        }

        public static void Initialize()
        {
        }

        private static void OnBarrelCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name == "Gragas_Base_Q_Ally.troy")
            {
                QBarrel = sender;
                BarrelTime = Game.Time;
            }
        }

        private static void OnBarrelDelete(GameObject sender, EventArgs args)
        {
            if (sender.Name == "Gragas_Base_Q_Ally.troy")
            {
                QBarrel = null;
            }
        }

        private static void OnInterruptibleSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (sender.IsEnemy && SpellManager.E.GetPrediction(sender).HitChance > HitChance.High
                && SpellManager.E.IsReady() && args.DangerLevel >= DangerLevel.High
                && Config.Modes.Misc.AutoEInterruptible)
            {
                SpellManager.E.Cast(sender);
            }
        }

        private static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            if (sender.IsEnemy && SpellManager.E.GetPrediction(sender).HitChance > HitChance.High
                && SpellManager.E.IsReady() && Config.Modes.Misc.AutoEGapCloser)
            {
                SpellManager.E.Cast(sender);
            }
        }
    }
}