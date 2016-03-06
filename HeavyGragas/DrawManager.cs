using Settings = HeavyGragas.Config.Modes.Draw;

namespace HeavyGragas
{
    using System;
    using System.Drawing;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Rendering;

    using SharpDX.Direct3D9;

    public static class DrawManager
    {
        private static readonly Text BarrelText, InsecText;

        static DrawManager()
        {
            BarrelText = new Text(
                "",
                new FontDescription
                    { FaceName = "Calibri", OutputPrecision = FontPrecision.Default, Quality = FontQuality.ClearType });

            InsecText = new Text(
                "",
                new FontDescription
                    { FaceName = "Calibri", OutputPrecision = FontPrecision.Default, Quality = FontQuality.ClearType });

            Drawing.OnDraw += OnDraw;
        }

        public static void Initialize()
        {
        }

        private static void OnDraw(EventArgs args)
        {
            if (Settings.DrawReady ? SpellManager.Q.IsReady() : Settings.DrawQ)
            {
                new Circle { Color = Color.Green, BorderWidth = Settings.WidthQ, Radius = SpellManager.Q.Range }.Draw(
                    Player.Instance.Position);
            }

            if (Settings.DrawReady ? SpellManager.W.IsReady() : Settings.DrawW)
            {
                new Circle { Color = Color.Green, BorderWidth = Settings.WidthW, Radius = SpellManager.W.Range }.Draw(
                    Player.Instance.Position);
            }

            if (Settings.DrawReady ? SpellManager.E.IsReady() : Settings.DrawE)
            {
                new Circle { Color = Color.Green, BorderWidth = Settings.WidthE, Radius = SpellManager.E.Range }.Draw(
                    Player.Instance.Position);
            }
            
            if (Settings.DrawReady ? SpellManager.E.IsReady() && SpellManager.Flash.IsReady() : Settings.DrawE)
            {
                new Circle
                    {
                        Color = Color.Yellow, BorderWidth = Settings.WidthE,
                        Radius = SpellManager.E.Range + SpellManager.Flash.Range
                    }.Draw(Player.Instance.Position);
            }

            if (Settings.DrawReady ? SpellManager.R.IsReady() : Settings.DrawR)
            {
                new Circle { Color = Color.Green, BorderWidth = Settings.WidthR, Radius = SpellManager.R.Range }.Draw(
                    Player.Instance.Position);
            }

            if (EventManager.QBarrel != null)
            {
                var barrel = EventManager.QBarrel;
                var percent = (int)((Game.Time - EventManager.BarrelTime) * 25);
                var q = SpellManager.Q;

                if (percent > 100)
                {
                    percent = 100;
                }

                if (Settings.DrawBarrelText)
                {
                    BarrelText.TextValue = 100 - percent + "%";
                    BarrelText.Position = Drawing.WorldToScreen(barrel.Position);
                    BarrelText.Draw();
                }

                if (Settings.DrawBarrelCircle)
                {
                    new Circle
                        {
                            Color = Color.MediumPurple, BorderWidth = Settings.WidthCircle,
                            Radius = q.Width - percent * q.Width / 100
                        }.Draw(barrel.Position);
                }
            }

            var insecTarget = TargetSelector.SelectedTarget;
            if (insecTarget != null)
            {
                InsecText.TextValue = "Cannot Insec";
                InsecText.Color = Color.OrangeRed;

                if (SpellManager.E.IsReady() && SpellManager.R.IsReady())
                {
                    var hitchance = SpellManager.E.GetPrediction(insecTarget).HitChance;
                    if (hitchance >= HitChance.Low)
                    {
                        InsecText.TextValue = "HitChance: " + hitchance;
                        InsecText.Color = Color.LimeGreen;
                    }
                    else if ((EventManager.QBarrel != null) && SpellManager.Flash.IsReady()
                             && insecTarget.IsValidTarget(SpellManager.E.Range + SpellManager.Flash.Range))
                    {
                        InsecText.TextValue = "Can Insec with Flash";
                        InsecText.Color = Config.Modes.Insec.UseFlash ? Color.LimeGreen : Color.OrangeRed;
                    }
                }

                InsecText.Position = Player.Instance.Position.WorldToScreen();
                InsecText.Draw();
                
                //new Circle { Color = Color.Red, BorderWidth = Settings.WidthCircle, Radius = 600 }.Draw(
                //    TargetSelector.SelectedTarget.Position);
                //var polygons = new List<Geometry.Polygon>
                //                   {
                //                       new Geometry.Polygon.Ring(insecTarget.Position.To2D(), 50, 625),
                //                       new Geometry.Polygon.Ring(Player.Instance.Position.To2D(), 50, 625)
                //                   };
                //var insecBarrelPath = Geometry.ClipPolygons(polygons);
                //foreach (var pathList in insecBarrelPath)
                //{
                //    var removePaths = pathList.Where(path => new Vector2(path.X, path.Y).ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall)).ToList();
                //    insecBarrelPath.Remove(removePaths);
                //}
                //foreach (var polygon in insecBarrelPath.ToPolygons())
                //{
                //    polygon.Draw(Color.Red);
                //}
                //var pos = Insec.CircleCircleIntersection(Player.Instance.Position.To2D(), insecTarget.Position.To2D(), SpellManager.Q.Range, 600).OrderBy(x => x.DistanceToNearestAlly()).FirstOrDefault().To3D();
                //new Circle { BorderWidth = 10, Color = Color.Yellow, Radius = 100}.Draw(pos);
            }
        }

        private static bool CanInsec(AIHeroClient target)
        {
            return target.Position.Distance(EventManager.QBarrel).IsBetween(610, 870);
        }
    }
}