using Settings = HeavyGragas.Config.Modes.Draw;

namespace HeavyGragas
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    using Color = System.Drawing.Color;

    public static class DamageIndicator
    {
        private const int BarWidth = 104;

        public delegate float DamageToUnitDelegate(AIHeroClient hero);

        private static DamageToUnitDelegate DamageToUnit { get; set; }

        private static readonly Vector2 BarOffset = new Vector2(-9, 11);

        private static Color drawingColor;

        public static Color DrawingColor
        {
            get
            {
                return drawingColor;
            }
            set
            {
                drawingColor = Color.FromArgb(170, value);
            }
        }

        public static bool HealthbarEnabled { get; set; }

        public static bool PercentEnabled { get; set; }

        public static void Initialize(DamageToUnitDelegate damageToUnit)
        {
            DamageToUnit = damageToUnit;
            HealthbarEnabled = Settings.DrawHealth;

            Drawing.OnEndScene += OnEndScene;
        }

        private static void OnEndScene(EventArgs args)
        {
            if (!HealthbarEnabled && !PercentEnabled) { return; }

            foreach (var unit in EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget() && u.IsHPBarRendered))
            {
                var damage = DamageToUnit(unit);

                if (damage <= 0)
                {
                    continue;
                }

                if (HealthbarEnabled)
                {
                    var pos = new Vector2(unit.HPBarPosition.X + 2, unit.HPBarPosition.Y + 9);
                    var fullbar = BarWidth * (unit.HealthPercent / 100);
                    damage = BarWidth
                             * (unit.TotalShieldHealth() - damage > 0 ? unit.TotalShieldHealth() - damage : 0)
                             / (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                    Line.DrawLine(
                        DrawingColor,
                        9f,
                        new Vector2(pos.X, pos.Y),
                        new Vector2(pos.X + (damage > fullbar ? fullbar : damage), pos.Y));
                }

                if (PercentEnabled)
                {
                    Drawing.DrawText(
                        unit.HPBarPosition,
                        Color.MediumVioletRed,
                        string.Concat(Math.Ceiling(damage / unit.TotalShieldHealth() * 100), "%"),
                        10);
                }
            }
        }
    }
}