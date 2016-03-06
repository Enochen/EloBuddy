
namespace HeavyGragas
{
    using System;
    using System.Drawing;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Rendering;

    using SharpDX.Direct3D9;

    using Font = System.Drawing.Font;

    public static class Program
    {
        public const string ChampName = "Gragas";

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != ChampName)
            {
                return;
            }

            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            DrawManager.Initialize();
        }

        
    }
}