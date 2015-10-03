namespace redRiven
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Constants;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    using Color = System.Drawing.Color;

    internal class Riven
    {
        public const string R1Name = "RivenFengShuiEngine";

        public const string R2Name = "rivenizunablade";

        public static GameObject mainTarget;

        public static Menu Menu, CMenu, HMenu, WMenu, JMenu, MMenu;

        public static AIHeroClient Player = ObjectManager.Player;

        public static Spell.Skillshot Q, E, R2;

        public static int QStacks, LastQ;

        public static Spell.Active W, R;

        public static bool WaitQ;

        public static void Cancel()
        {
            EloBuddy.Player.DoEmote(Emote.Dance);
            Orbwalker.ResetAutoAttack();
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None))
            {
                EloBuddy.Player.IssueOrder(GameObjectOrder.AttackUnit, mainTarget);
            }
        }

        public static bool CanR2()
        {
            return Player.HasBuff("rivenwindslashready");
        }

        public static void CreateMenu()
        {
            Menu = MainMenu.AddMenu("redRiven", "Menu");
            Menu.AddGroupLabel("redRiven By Darakath");
            Menu.AddSeparator();
            Menu.AddLabel("A reworked version of dRiven.");

            CMenu = Menu.AddSubMenu("Combo", "cMenu");
            CMenu.Add("q", new CheckBox("Use Q"));
            CMenu.AddSeparator();
            CMenu.Add("w", new CheckBox("Use W"));
            CMenu.AddSeparator();
            CMenu.Add("e", new CheckBox("Use E"));
            CMenu.AddSeparator();
            CMenu.Add("r", new CheckBox("Use R"));
            CMenu.AddSeparator();
            CMenu.Add("r2", new Slider("Use R2 Health Percent", 10));
            CMenu.AddSeparator();
            CMenu.Add("r1", new Slider("Use R1 Enemies", 2, 0, 5));

            HMenu = Menu.AddSubMenu("Harass", "hMenu");
            HMenu.Add("q", new CheckBox("Use Q"));
            HMenu.AddSeparator();
            HMenu.Add("w", new CheckBox("Use W"));
            HMenu.AddSeparator();
            HMenu.Add("e", new CheckBox("Use E"));

            WMenu = Menu.AddSubMenu("WaveClear", "wMenu");
            WMenu.Add("q", new CheckBox("Use Q"));
            WMenu.AddSeparator();
            WMenu.Add("w", new CheckBox("Use W"));
            WMenu.AddSeparator();
            WMenu.Add("e", new CheckBox("Use E"));

            JMenu = Menu.AddSubMenu("JungleClear", "jMenu");
            JMenu.Add("q", new CheckBox("Use Q"));
            JMenu.AddSeparator();
            JMenu.Add("w", new CheckBox("Use W"));
            JMenu.AddSeparator();
            JMenu.Add("e", new CheckBox("Use E"));

            MMenu = Menu.AddSubMenu("Misc", "mMenu");
            MMenu.Add("q", new CheckBox("Keep Q"));
            MMenu.Add("dr", new CheckBox("Draw R Range"));
            MMenu.Add("hp", new CheckBox("Draw HP Indicator"));
        }

        public static void OnDraw(EventArgs args)
        {
            if (!Combo.GetOption(MMenu, "hp"))
            {
                return;
            }
            foreach (var unit in HeroManager.Enemies.Where(u => u.IsValidTarget() && u.IsHPBarRendered))
            {
                var offset = new Vector2(0, 10);
                var damage = Combo.CalcDmg(unit, true, false);
                if (damage > 0)
                {
                    var dmgPercent = ((unit.Health - damage) > 0 ? (unit.Health - damage) : 0) / unit.MaxHealth;
                    var healthPercent = unit.Health / unit.MaxHealth;
                    var start = new Vector2(
                        (int)(unit.HPBarPosition.X + offset.X + dmgPercent * 104),
                        (int)(unit.HPBarPosition.Y + offset.Y) - 5);
                    var end = new Vector2(
                        (int)(unit.HPBarPosition.X + offset.X + healthPercent * 104) + 1,
                        (int)(unit.HPBarPosition.Y + offset.Y) - 5);
                    Drawing.DrawLine(start, end, 9, Color.Gold);
                }
            }
            if (!Combo.GetOption(MMenu, "dr"))
            {
                return;
            }
            var r2Range = new Circle(new ColorBGRA(138, 43, 226, 255), R2.Range);
            r2Range.Draw(Player.Position);
        }

        public static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Riven")
            {
                return;
            }
            CreateMenu();
            Q = new Spell.Skillshot(SpellSlot.Q, 260, SkillShotType.Cone, 500, 1400, 100);
            W = new Spell.Active(SpellSlot.W, 125);
            E = new Spell.Skillshot(SpellSlot.E, 325, SkillShotType.Linear);
            R = new Spell.Active(SpellSlot.R);
            R2 = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 125)
                     { MinimumHitChance = HitChance.Medium };
            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnPlayAnimation += OnPlayAnimation;
            Obj_AI_Base.OnSpellCast += OnSpellCast;
            Drawing.OnEndScene += OnDraw;
            Chat.Print("redRiven by Darakath Loaded");
        }

        public static void OnSpellCast(GameObject sender, GameObjectProcessSpellCastEventArgs args)
        {
            var mTar = args.Target;
            var spell = args.SData;
            if (!sender.IsMe)
            {
                return;
            }

            if (spell.IsAutoAttack() && sender.IsMe && (Q.IsReady(50))
                && (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                    || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)))
            {
                SetupQ(mTar);
                Orbwalker.ResetAutoAttack();
            }
        }

        public static void OnUpdate(EventArgs args)
        {
            Combo.UseQ();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo.DoCombo();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Combo.DoCombo(false);
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                     || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Combo.Clear();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
            }
        }

        public static bool RState()
        {
            return Player.HasBuff("RivenFengShuiEngine");
        }

        public static void SetupQ(GameObject tar)
        {
            mainTarget = tar;
            WaitQ = true;
        }

        private static void OnPlayAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.None)
            {
                return;
            }
            var t = 0;
            switch (args.Animation)
            {
                case "Spell1a":
                    LastQ = Environment.TickCount;
                    WaitQ = false;
                    t = 291;
                    QStacks++;
                    break;
                case "Spell1b":
                    LastQ = Environment.TickCount;
                    WaitQ = false;
                    t = 291;
                    QStacks++;
                    break;
                case "Spell1c":
                    LastQ = Environment.TickCount;
                    WaitQ = false;
                    t = 393;
                    QStacks++;
                    break;
                case "Spell2":
                    t = 170;
                    break;
                case "Spell4a":
                    t = 0;
                    break;
                case "Spell4b":
                    t = 150;
                    break;
            }
            if (QStacks > 2)
            {
                QStacks = 0;
            }
            if (t == 0)
            {
                return;
            }
            Core.DelayAction(Cancel, t - Game.Ping);
        }
    }
}