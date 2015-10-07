namespace Leona
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class Leona
    {
        public static Menu Menu, CMenu, HMenu, WMenu, JMenu, MMenu, DMenu, OMenu;

        public static AIHeroClient Player = ObjectManager.Player;

        public static Spell.Skillshot E, R, R2;

        public static Spell.Active Q, W;

        public static void CreateMenu()
        {
            Menu = MainMenu.AddMenu("Radiant Leona", "Menu");
            Menu.AddGroupLabel("Developed by Darakath");

            CMenu = Menu.AddSubMenu("Combo", "cMenu");
            CMenu.Add("Q", new CheckBox("Use Q"));
            CMenu.AddSeparator();
            CMenu.Add("W", new CheckBox("Use W"));
            CMenu.AddSeparator();
            CMenu.Add("E", new CheckBox("Use E"));
            CMenu.AddSeparator();
            CMenu.Add("R", new CheckBox("Use R"));
            CMenu.AddSeparator();
            CMenu.Add("RE", new Slider("Use R Enemies", 2, 1, 5));

            HMenu = Menu.AddSubMenu("Harass", "hMenu");
            HMenu.Add("Q", new CheckBox("Use Q"));
            HMenu.AddSeparator();
            HMenu.Add("E", new CheckBox("Use E"));

            MMenu = Menu.AddSubMenu("Mana", "mMenu");
            MMenu.Add("Q", new Slider("Mana for Q"));
            MMenu.Add("W", new Slider("Mana for W"));
            MMenu.Add("E", new Slider("Mana for E"));
            MMenu.Add("R", new Slider("Mana for R"));
            MMenu.Add("H", new Slider("Mana for Harass"));

            DMenu = Menu.AddSubMenu("Drawings", "dMenu");
            DMenu.Add("Q", new CheckBox("Draw Q Range"));
            DMenu.Add("W", new CheckBox("Draw W Range"));
            DMenu.Add("E", new CheckBox("Draw E Range"));
            DMenu.Add("R", new CheckBox("Draw R Range"));

            OMenu = Menu.AddSubMenu("Other", "oMenu");
            OMenu.Add("G", new CheckBox("Auto QAA on Gapcloser"));
            OMenu.Add("C", new CheckBox("Auto AA Cancel With Q on Champions"));
        }

        public static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Leona")
            {
                return;
            }
            CreateMenu();
            Q = new Spell.Active(SpellSlot.Q, 155);
            W = new Spell.Active(SpellSlot.W, 275);
            E = new Spell.Skillshot(SpellSlot.E, 875, SkillShotType.Linear, 250, 2000, 70);
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Circular, 1000, int.MaxValue, 250);
            R2 = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Circular, 1000, int.MaxValue, 100);
            E.MinimumHitChance = HitChance.High;
            Game.OnUpdate += OnUpdate;
            Gapcloser.OnGapcloser += OnGapcloser;
            Drawing.OnEndScene += OnEndScene;
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
            Chat.Print(
                "<font color='#FFD700'>Radiant</font> <font color='#DEB887'>Leona</font> <font color='#FFF8DC'>Loaded!</font>");
        }

        public static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !(args.Target is AIHeroClient) || !Q.IsReady() || !Player.CanAttack || !GetOption(OMenu, "C"))
            {
                return;
            }
            Q.Cast();
            Orbwalker.ResetAutoAttack();
            EloBuddy.Player.IssueOrder(GameObjectOrder.AttackTo, args.Target);
        }

        public static void OnGapcloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!Q.IsReady() || sender.Type != GameObjectType.AIHeroClient || !Player.IsInAutoAttackRange(sender)
                //Player.Distance(args.End) > Player.GetAutoAttackRange()
                || sender.IsAlly || !GetOption(OMenu, "G"))
            {
                return;
            }
            Q.Cast();
            Orbwalker.ResetAutoAttack();
            EloBuddy.Player.IssueOrder(GameObjectOrder.AttackTo, sender);
        }

        public static void OnEndScene(EventArgs args)
        {
            var qRange = new Circle(new ColorBGRA(138, 43, 226, 255), Q.Range);
            var wRange = new Circle(new ColorBGRA(138, 43, 226, 255), W.Range);
            var eRange = new Circle(new ColorBGRA(138, 43, 226, 255), E.Range);
            var rRange = new Circle(new ColorBGRA(138, 43, 226, 255), R.Range);

            if (GetOption(DMenu, "Q") && Q.IsReady())
            {
                qRange.Draw(Player.Position);
            }
            if (GetOption(DMenu, "W") && W.IsReady())
            {
                wRange.Draw(Player.Position);
            }
            if (GetOption(DMenu, "E") && E.IsReady())
            {
                eRange.Draw(Player.Position);
            }
            if (GetOption(DMenu, "R") && R.IsReady())
            {
                rRange.Draw(Player.Position);
            }
        }

        public static dynamic GetOption(Menu menu, string option)
        {
            var item = menu[option];
            if (item is CheckBox)
            {
                return item.Cast<CheckBox>().CurrentValue;
            }
            if (item is Slider)
            {
                return item.Cast<Slider>().CurrentValue;
            }
            if (item is KeyBind)
            {
                return item.Cast<KeyBind>().CurrentValue;
            }
            return null;
        }

        public static void OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                States.DoCombo(
                    GetOption(CMenu, "Q"),
                    GetOption(CMenu, "W"),
                    GetOption(CMenu, "E"),
                    GetOption(CMenu, "R"),
                    GetOption(CMenu, "RE"));
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                States.DoCombo(
                    GetOption(HMenu, "Q"), 
                    false,
                    GetOption(HMenu, "E"),
                    false,
                    6);
            }
        }

        public static bool WActive()
        {
            return Player.HasBuff("rivenwindslashready");
        }
    }
}