using Settings = SpaceAIDS.Config.Modes.Draw;

namespace SpaceAIDS
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    using Color = System.Drawing.Color;

    public static class DamageIndicator
    {
        private const int BarWidth = 104;

        private const int LineThickness = 9;

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
            DrawingColor = Settings.colorHealth;
            HealthbarEnabled = Settings.DrawHealth;
            
            Drawing.OnEndScene += OnEndScene;
        }

        private static void OnEndScene(EventArgs args)
        {
            if (HealthbarEnabled || PercentEnabled)
            {
                foreach (var unit in EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget() && u.IsHPBarRendered))
                {
                    var damage = DamageToUnit(unit);
                    
                    if (damage <= 0)
                    {
                        continue;
                    }

                    if (HealthbarEnabled)
                    {
                        var damagePercentage = ((unit.TotalShieldHealth() - damage) > 0
                                                    ? (unit.TotalShieldHealth() - damage)
                                                    : 0)
                                               / (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                        var currentHealthPercentage = unit.TotalShieldHealth()
                                                      / (unit.MaxHealth + unit.AllShield + unit.AttackShield
                                                         + unit.MagicShield);
                        
                        var startPoint =
                            new Vector2(
                                (int)(unit.HPBarPosition.X + BarOffset.X + damagePercentage * BarWidth),
                                (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                        var endPoint =
                            new Vector2(
                                (int)(unit.HPBarPosition.X + BarOffset.X + currentHealthPercentage * BarWidth) + 1,
                                (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                        
                        Drawing.DrawLine(startPoint, endPoint, LineThickness, DrawingColor);
                    }

                    if (PercentEnabled)
                    {
                        Drawing.DrawText(
                            unit.HPBarPosition,
                            Color.MediumVioletRed,
                            string.Concat(Math.Ceiling((damage / unit.TotalShieldHealth()) * 100), "%"),
                            10);
                    }
                }
            }
        }
    }
}