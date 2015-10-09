namespace DevBuddy2._0
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public static class Program
    {
        private static List<GameObject> gameObjects = new List<GameObject>();

        private static List<GameObject> mouseObjects = new List<GameObject>();

        private static Menu cMenu, Menu;

        private static int lastTick;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static Color GetMenuColor()
        {
            return Color.FromArgb(
                cMenu["colorR"].Cast<Slider>().CurrentValue,
                cMenu["colorG"].Cast<Slider>().CurrentValue,
                cMenu["colorB"].Cast<Slider>().CurrentValue);
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            Hacks.AntiAFK = true;
            Bootstrap.Init(null);

            Menu = MainMenu.AddMenu("DevBuddy2", "DevBuddy2.0");
            Menu.AddGroupLabel("ヽ༼ຈل͜ຈ༽ﾉ RAISE YOUR DONGERS ヽ༼ຈل͜ຈ༽ﾉ ");
            Menu.AddLabel("PORTED FROM YOU KNOW WHERE");
            Menu.AddLabel("Ported by and copied from vOID github.com/voidbuddy");
            Menu.AddSeparator();
            Menu.AddLabel("Edited by Darakath");
            Menu.AddLabel("https://www.elobuddy.net/user/1112-darakath/");

            cMenu = Menu.AddSubMenu("Configuration", "config");
            cMenu.AddGroupLabel("Config");
            cMenu.Add("mouseRange", new Slider("Range from Mouse to GameObject", 400, 100, 1000));

            cMenu.AddLabel("Text Color");
            cMenu.Add("colorR", new Slider("R", 255, 0 , 255));
            cMenu.Add("colorG", new Slider("G", 255, 0, 255));
            cMenu.Add("colorB", new Slider("B", 255, 0, 255));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += OnDraw;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Environment.TickCount - lastTick > 150)
            {
                gameObjects = ObjectManager.Get<GameObject>().ToList();
                mouseObjects =
                    gameObjects.Where(
                        o =>
                        o.Position.Distance(Game.CursorPos) < cMenu["mouseRange"].Cast<Slider>().CurrentValue
                        && !(o is Obj_Turret) && o.Name != "missile" && !(o is Obj_LampBulb) && !(o is Obj_SpellMissile)
                        && !(o is GrassObject) && !(o is DrawFX) && !(o is LevelPropSpawnerPoint)
                        && !(o is Obj_GeneralParticleEmitter) && !o.Name.Contains("MoveTo")).ToList();
                lastTick = Environment.TickCount;
            }
        }

        private static void OnDraw(EventArgs args)
        {
            foreach (var obj in mouseObjects)
            {
                if (!obj.IsValid)
                {
                    return;
                }
                var x = Drawing.WorldToScreen(obj.Position).X;
                var y = Drawing.WorldToScreen(obj.Position).Y;
                Drawing.DrawText(x, y + 10, GetMenuColor(), obj.Type.ToString());
                Drawing.DrawText(x, y + 20, GetMenuColor(), "NetworkID: " + obj.NetworkId);
                Drawing.DrawText(x, y + 30, GetMenuColor(), obj.Position.ToString());
                if (obj is Obj_AI_Base)
                {
                    var aiobj = obj as Obj_AI_Base;
                    Drawing.DrawText(
                        x,
                        y + 40,
                        GetMenuColor(),
                        "Health: " + aiobj.Health + "/" + aiobj.MaxHealth + "(" + aiobj.HealthPercent + "%)");
                }
                if (obj is AIHeroClient)
                {
                    var hero = obj as AIHeroClient;
                    Drawing.DrawText(x, y + 50, GetMenuColor(), "Spells:");
                    Drawing.DrawText(x, y + 60, GetMenuColor(), "(Q): " + hero.Spellbook.Spells[0].Name);
                    Drawing.DrawText(x, y + 70, GetMenuColor(), "(W): " + hero.Spellbook.Spells[1].Name);
                    Drawing.DrawText(x, y + 80, GetMenuColor(), "(E): " + hero.Spellbook.Spells[2].Name);
                    Drawing.DrawText(x, y + 90, GetMenuColor(), "(R): " + hero.Spellbook.Spells[3].Name);
                    Drawing.DrawText(x, y + 100, GetMenuColor(), "(D): " + hero.Spellbook.Spells[4].Name);
                    Drawing.DrawText(x, y + 110, GetMenuColor(), "(F): " + hero.Spellbook.Spells[5].Name);
                    var buffs = hero.Buffs;
                    if (buffs.Any())
                    {
                        Drawing.DrawText(x, y + 120, GetMenuColor(), "Buffs:");
                    }
                    for (var i = 0; i < buffs.Count() * 10; i += 10)
                    {
                        Drawing.DrawText(x, (y + 130 + i), GetMenuColor(), buffs[i / 10].Count + "x " + buffs[i / 10].Name);
                    }
                }
                if (obj is Obj_SpellMissile)
                {
                    var missile = obj as Obj_SpellMissile;
                    Drawing.DrawText(x, y + 40, GetMenuColor(), "Missile Speed: " + missile.SData.MissileSpeed);
                    Drawing.DrawText(x, y + 50, GetMenuColor(), "Cast Range: " + missile.SData.CastRange);
                }

                if (obj is MissileClient && obj.Name != "missile")
                {
                    var missile = obj as MissileClient;
                    Drawing.DrawText(x, y + 40, GetMenuColor(), "Missile Speed: " + missile.SData.MissileSpeed);
                    Drawing.DrawText(x, y + 50, GetMenuColor(), "Cast Range: " + missile.SData.CastRange);
                }
            }
        }
    }
}