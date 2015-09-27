namespace redRiven
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Constants;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Riven
    {
        #region Constants

        public const string R1Name = "RivenFengShuiEngine";

        public const string R2Name = "rivenizunablade";

        #endregion

        #region Static Fields

        public static GameObject mainTarget;

        public static Menu Menu, CMenu, HMenu, WMenu, JMenu, MMenu;

        public static AIHeroClient Player = ObjectManager.Player;

        public static Spell.Skillshot Q, E, R2;

        public static int QStacks, LastQ;

        public static Spell.Active W, R;

        public static bool WaitQ;

        #endregion

        #region Public Methods and Operators

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
            CMenu.Add("r1", new Slider("Use R1 Enemies", 2, 0, HeroManager.Enemies.Count));

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
            Obj_AI_Base.OnSpellCast += OnSpellCast;
            Chat.Print("redRiven Loaded");
        }

        public static void OnSpellCast(GameObject sender, GameObjectProcessSpellCastEventArgs args)
        {
            var mTar = args.Target;
            var spell = args.SData;
            if (!sender.IsMe)
            {
                return;
            }
            if (spell.Name.Contains("RivenTriCleave"))
            {
                LastQ = Environment.TickCount;
                WaitQ = false;
                QStacks++;
                if (QStacks > 2)
                {
                    QStacks = 0;
                }
                Orbwalker.ResetAutoAttack();
                if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None))
                {
                    EloBuddy.Player.IssueOrder(GameObjectOrder.AttackUnit, mainTarget);
                }
                
            }

            if (spell.IsAutoAttack() && sender.IsMe && Q.IsReady()
                && (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                    || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)))
            {
                //Player.Spellbook.CastSpell(SpellSlot.Q, mainTarget.Position);
                SetupQ(mTar);
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

        #endregion
    }
}