namespace Rice
{
    using System;
    using System.Drawing;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    using Color = System.Drawing.Color;

    public static class Program
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public const string ChampName = "Ryze";

        public static Text StackingStatus = new Text("Rice", new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold));

        public static void Main(string[] args)
        {
            Game.OnLoad += OnLoad;
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoad(EventArgs args)
        {
            Config.Modes.Misc.EnemyNames =
                EntityManager.Heroes.Enemies.OrderBy(x => x.NetworkId).Select(x => x.Name).ToList();
            Config.Modes.Misc.AllyNames =
                EntityManager.Heroes.Allies.OrderBy(x => x.NetworkId).Select(x => x.Name).ToList();
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != ChampName)
            {
                return;
            }
            if (Config.Modes.Misc.ChangeNames)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies)
                {
                    enemy.Name = "Combo Me Pls";
                }
                foreach (var ally in EntityManager.Heroes.Allies)
                {
                    ally.Name = "Don't Let Me KS";
                }
                Player.Instance.Name = "Best Rice EB";
            }
            StackingStatus.TextValue = Config.Modes.AutoStack.AutoStackQ
                                           ? "Passive Stacking On"
                                           : "Passive Stacking Off";
            StackingStatus.Color = Config.Modes.AutoStack.AutoStackQ ? Color.LimeGreen : Color.Red;
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);

            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (Config.Modes.Draw.DrawStackStatus)
            {
                StackingStatus.Position = Player.Instance.Position.WorldToScreen()
                                          - new Vector2((float)(StackingStatus.Bounding.Width / 2.0), -38);
                StackingStatus.Draw();
            }

            if (Config.Modes.Draw.DrawReady ? SpellManager.Q1.IsReady() : Config.Modes.Draw.DrawQ)
            {
                new Circle { Color = Config.Modes.Draw.colorQ, BorderWidth = Config.Modes.Draw._widthQ, Radius = SpellManager.Q1.Range }
                    .Draw(Player.Instance.Position);
            }

            if (Config.Modes.Draw.DrawReady ? SpellManager.W.IsReady() : Config.Modes.Draw.DrawW)
            {
                new Circle { Color = Config.Modes.Draw.colorW, BorderWidth = Config.Modes.Draw._widthW, Radius = SpellManager.W.Range }
                    .Draw(Player.Instance.Position);
            }

            if (Config.Modes.Draw.DrawReady ? SpellManager.E.IsReady() : Config.Modes.Draw.DrawE)
            {
                new Circle { Color = Config.Modes.Draw.colorE, BorderWidth = Config.Modes.Draw._widthE, Radius = SpellManager.E.Range }
                    .Draw(Player.Instance.Position);
            }

            if (Config.Modes.Draw.DrawReady ? SpellManager.R.IsReady() : Config.Modes.Draw.DrawR)
            {
                new Circle { Color = Config.Modes.Draw.colorR, BorderWidth = Config.Modes.Draw._widthR, Radius = SpellManager.R.Range }
                    .Draw(Player.Instance.Position);
            }
        }
    }
}