using Settings = BetterNerfIrelia.Config.Modes.Draw;

namespace BetterNerfIrelia
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Rendering;

    public static class Program
    {
        public const Champion ChampName = Champion.Irelia;

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != ChampName)
            {
                return;
            }

            Console.WriteLine(Player.Instance.GetSpellSlotFromName("summonerdot"));
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);

            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (Settings.DrawReady ? SpellManager.Q.IsReady() : Settings.DrawQ)
            {
                new Circle { Color = Settings.colorQ, BorderWidth = Settings._widthQ, Radius = SpellManager.Q.Range }
                    .Draw(Player.Instance.Position);
            }

            if (Settings.DrawReady ? SpellManager.W.IsReady() : Settings.DrawW)
            {
                new Circle { Color = Settings.colorW, BorderWidth = Settings._widthW, Radius = SpellManager.W.Range }
                    .Draw(Player.Instance.Position);
            }

            if (Settings.DrawReady ? SpellManager.E.IsReady() : Settings.DrawE)
            {
                new Circle { Color = Settings.colorE, BorderWidth = Settings._widthE, Radius = SpellManager.E.Range }
                    .Draw(Player.Instance.Position);
            }

            if (Settings.DrawReady ? SpellManager.R.IsReady() : Settings.DrawR)
            {
                new Circle { Color = Settings.colorR, BorderWidth = Settings._widthR, Radius = SpellManager.R.Range }
                    .Draw(Player.Instance.Position);
            }
        }
    }
}