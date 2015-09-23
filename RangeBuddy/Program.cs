namespace RangeBuddy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class Program
    {
        #region Constants
        private const float ExpRange = 1350f;

        private static float trtRange = 875f + ObjectManager.Player.BoundingRadius;

        #endregion

        #region Static Fields

        private static Menu rMenu, eMenu, aMenu, tMenu, sMenu;

        private static readonly Dictionary<int, Obj_AI_Turret> TurretCache = new Dictionary<int, Obj_AI_Turret>();

        private static readonly Dictionary<int, AttackableUnit> TurretTarget = new Dictionary<int, AttackableUnit>();

        #endregion

        #region Methods

        private static void CacheTurrets()
        {
            foreach (var obj in ObjectManager.Get<Obj_AI_Turret>())
            {
                if (TurretCache.ContainsKey(obj.NetworkId))
                {
                    continue;
                }
                TurretCache.Add(obj.NetworkId, obj);
                TurretTarget.Add(obj.NetworkId, null);
            }
        }

        private static void DrawAttack()
        {
            var drawSelf = aMenu["a.s"].Cast<CheckBox>().CurrentValue;
            var drawAlly = aMenu["a.a"].Cast<CheckBox>().CurrentValue;
            var drawEnemy = aMenu["a.e"].Cast<CheckBox>().CurrentValue;
            var thickness = rMenu["cT"].Cast<Slider>().CurrentValue;

            if (!drawAlly && !drawEnemy && !drawSelf)
            {
                return;
            }
            foreach (var hero in
                HeroManager.AllHeroes.Where(hero => !hero.IsDead && hero.IsVisible)
                    .Where(
                        hero =>
                        (hero.IsAlly && drawAlly || hero.IsMe && drawSelf || hero.IsEnemy && drawEnemy)
                        && !(hero.IsMe && !drawSelf)))
            {
                var radius = hero.BoundingRadius + hero.AttackRange
                             + (hero.IsAlly
                                    ? HeroManager.Enemies.Select(e => e.BoundingRadius).DefaultIfEmpty(0).Average()
                                    : ObjectManager.Player.BoundingRadius);
                if (hero.VisibleOnScreen)
                {
                    new Circle(Color.Gold, radius, thickness).Draw(hero.Position);
                }
            }
        }

        private static void DrawExperience()
        {
            var drawSelf = eMenu["e.s"].Cast<CheckBox>().CurrentValue;
            var drawAlly = eMenu["e.a"].Cast<CheckBox>().CurrentValue;
            var drawEnemy = eMenu["e.e"].Cast<CheckBox>().CurrentValue;
            var thickness = rMenu["cT"].Cast<Slider>().CurrentValue;

            if (!drawAlly && !drawEnemy && !drawSelf)
            {
                return;
            }
            foreach (
                var hero in
                    HeroManager.AllHeroes.Where(hero => !hero.IsDead && hero.IsVisible)
                        .Where(
                            hero =>
                            (hero.IsAlly && drawAlly || hero.IsMe && drawSelf || hero.IsEnemy && drawEnemy)
                            && !(hero.IsMe && !drawSelf) && hero.VisibleOnScreen))
            {
                new Circle(Color.Gray, ExpRange, thickness).Draw(hero.Position);
                //Drawing.DrawCircle(hero.Position, ExpRange, System.Drawing.Color.Gray);
            }
        }
        

        private static void DrawTurret()
        {
            var drawAlly = tMenu["t.a"].Cast<CheckBox>().CurrentValue;
            var drawEnemy = tMenu["t.e"].Cast<CheckBox>().CurrentValue;
            var thickness = rMenu["cT"].Cast<Slider>().CurrentValue;
            var dangerColor = System.Drawing.Color.Red;
            var safeColor = System.Drawing.Color.Red;
            if (!drawAlly && !drawEnemy)
            {
                return;
            }
            foreach (var entry in TurretCache)
            {
                var turret = entry.Value;
                const int CirclePadding = 20;
                if (turret != null && turret.IsValid && !turret.IsDead)
                {
                    var distToTurret = ObjectManager.Player.ServerPosition.Distance(turret.Position);
                    if (distToTurret < trtRange + 1500)
                    {
                        var tTarget = TurretTarget[turret.NetworkId];
                        if (tTarget.IsValidTarget(float.MaxValue))
                        {
                            Drawing.DrawCircle(
                                tTarget.Position,
                                tTarget.BoundingRadius + CirclePadding,
                                turret.IsAttackingPlayer ? dangerColor : safeColor);
                        }
                        if (tTarget != null && (tTarget.IsMe || (turret.IsAlly && tTarget is AIHeroClient)))
                        {
                            Drawing.DrawCircle(turret.Position, trtRange, dangerColor);
                        }
                        else
                        {
                            
                            var alpha = distToTurret > trtRange ? (((trtRange + 500) - distToTurret) / 2) : 250;
                            /*Drawing.DrawCircle(
                                turret.Position,
                                trtRange,
                                System.Drawing.Color.FromArgb(alpha, 238, 232, 170));*/
                            var color = new Color(238, 232, 170, alpha);
                            new Circle(color, trtRange, thickness).Draw(turret.Position);
                            
                        }
                    }
                }
                else
                {
                    TurretCache.Remove(entry.Key);
                    continue;
                }
            }
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        private static void OnDrawingDraw(EventArgs args)
        {
            DrawExperience();
            DrawTurret();
            DrawAttack();
        }

        private static void OnLoad(EventArgs args)
        {
            rMenu = MainMenu.AddMenu("RangeBuddy", "rMenu");
            rMenu.Add("cT", new Slider("Circle Thickness", 2, 1, 10));

            eMenu = rMenu.AddSubMenu("Experience");
            eMenu.Add("e.s", new CheckBox("Draw Self", false));
            eMenu.Add("e.a", new CheckBox("Draw Ally", false));
            eMenu.Add("e.e", new CheckBox("Draw Enemy", false));

            aMenu = rMenu.AddSubMenu("Auto Attack");
            aMenu.Add("a.s", new CheckBox("Draw Self", false));
            aMenu.Add("a.a", new CheckBox("Draw Ally", false));
            aMenu.Add("a.e", new CheckBox("Draw Enemy", false));

            //tMenu = rMenu.AddSubMenu("Turret");
            //tMenu.Add("t.a", new CheckBox("Draw Ally", false));
            //tMenu.Add("t.e", new CheckBox("Draw Enemy", false));

            CacheTurrets();
            Drawing.OnDraw += OnDrawingDraw;
        }

        #endregion
    }
}