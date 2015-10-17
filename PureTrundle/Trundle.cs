namespace PureTrundle
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Constants;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    using Color = System.Drawing.Color;

    internal class Trundle
    {
        public static Spell.Targeted R;

        public static Menu Menu, CMenu, HMenu, WMenu, MMenu, DMenu, OMenu;

        public static AIHeroClient Player = ObjectManager.Player;

        public static Spell.Skillshot W, E;

        public static Spell.Active Chomp;

        public static void CreateMenu()
        {
            Menu = MainMenu.AddMenu("PureTrundle", "Menu");
            Menu.AddGroupLabel("PureTrundle By Darakath");

            CMenu = Menu.AddSubMenu("Combo", "cMenu");
            CMenu.Add("W", new CheckBox("Use W"));
            CMenu.AddSeparator();
            CMenu.Add("E", new CheckBox("Use E"));
            CMenu.AddSeparator();
            CMenu.Add("R", new CheckBox("Use R"));
            CMenu.AddSeparator();
            CMenu.Add("BC", new CheckBox("Use Botrk/Cutlass"));
            CMenu.AddSeparator();
            CMenu.Add("TH", new CheckBox("Use Tiamat/Hydra"));

            HMenu = Menu.AddSubMenu("Harass", "hMenu");
            HMenu.Add("W", new CheckBox("Use W"));
            HMenu.AddSeparator();
            HMenu.Add("E", new CheckBox("Use E"));
            HMenu.AddSeparator();
            HMenu.Add("TH", new CheckBox("Use Tiamat/Hydra"));

            WMenu = Menu.AddSubMenu("Wave/Jungle Clear", "wMenu");
            WMenu.Add("WW", new CheckBox("Use W for WaveClear"));
            WMenu.AddSeparator();
            WMenu.Add("WJ", new CheckBox("Use W for JungleClear"));
            WMenu.AddSeparator();
            WMenu.Add("TH", new CheckBox("Use Tiamat/Hydra"));

            MMenu = Menu.AddSubMenu("Mana", "mMenu");
            MMenu.Add("Q", new Slider("Mana for Q"));
            MMenu.Add("W", new Slider("Mana for W"));
            MMenu.Add("E", new Slider("Mana for E"));
            MMenu.Add("R", new Slider("Mana for R"));
            Menu.AddSeparator();
            MMenu.Add("WC", new Slider("Mana for WaveClear"));
            MMenu.Add("JF", new Slider("Mana for JungleFarm"));
            MMenu.Add("H", new Slider("Mana for Harass"));

            DMenu = Menu.AddSubMenu("Drawings", "dMenu");
            DMenu.Add("W", new CheckBox("Draw W Range", false));
            DMenu.Add("E", new CheckBox("Draw E Range", false));
            DMenu.Add("R", new CheckBox("Draw R Range"));
            DMenu.Add("RD", new CheckBox("HP Bar Indicator (Q+R+AA+Items)"));

            OMenu = Menu.AddSubMenu("Other", "oMenu");
            OMenu.Add("GC", new CheckBox("Auto Anti-GapCloser", false));
        }

        public static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Trundle")
            {
                return; //Poor folks :^)
            }
            CreateMenu();
            Chomp = new Spell.Active(SpellSlot.Q, (uint)Player.GetAutoAttackRange());
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 0, int.MaxValue, 1000);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Circular, 250, int.MaxValue, 225);
            R = new Spell.Targeted(SpellSlot.R, 700);
            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnSpellCast += OnSpellCast;
            Gapcloser.OnGapcloser += OnGapcloser;
            //Interrupter.OnInterruptableSpell += OnInterruptableSpell;
            Drawing.OnEndScene += OnEndScene;

            //Chat.Print("PureTrundle v" + version + " Loaded");
        }

        public static void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spell = args.SData;
            if (!sender.IsMe)
            {
                return;
            }
            if (spell.IsAutoAttack() && sender.IsMe && Chomp.IsReady() && !args.Target.IsDead
                && States.CalcDmg((Obj_AI_Base)args.Target, true)
                && ((Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                     && Player.ManaPercent > GetOption(MMenu, "Q"))
                    || (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && States.UseQwc)
                    || (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) && States.UseQjf)
                    || (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)
                        && Player.ManaPercent > GetOption(MMenu, "H"))))
            {
                Chomp.Cast();
                Orbwalker.ResetAutoAttack();
            }
            if ((!GetOption(WMenu, "TH")
                 || (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                     && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)))
                && (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || (!GetOption(CMenu, "TH")))
                && (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) || (!GetOption(HMenu, "TH"))))
            {
                return;
            }

            if (Item.HasItem(States.Hydra) && Item.CanUseItem(States.Hydra))
            {
                Item.UseItem(States.Hydra);
            }
            else if (Item.HasItem(States.Tiamat) && Item.CanUseItem(States.Tiamat))
            {
                Item.UseItem(States.Tiamat);
            }
        }

        public static void OnGapcloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!E.IsReady() || sender.Type != GameObjectType.AIHeroClient || args.End.Distance(Player.Position) > E.Range || sender.IsAlly || !GetOption(OMenu,"GC") || E.GetPrediction(sender).HitChance != HitChance.Dashing)
            {
                return;
            }
            //if (args.End.Distance(Player.Position) > args.Start.Distance(Player.Position))
            {
                E.Cast(args.End);
            }
            /*else if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo && GetOption(CMenu, "E"))
                {
                    E.Cast((Vector3)args.End.Extend(args.Start, ObjectManager.Player.BoundingRadius + (float)E.Width));
                }
                else
                {
                    E.Cast((Vector3)Player.Position.Extend(args.End, ObjectManager.Player.BoundingRadius + (float)E.Width));
                }*/
        }

        public static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (sender.IsEnemy && E.IsReady() && sender.Distance(Player) < E.Range && sender.Type == GameObjectType.AIHeroClient && args.DangerLevel == DangerLevel.High)
            {
                E.Cast(
                    (Vector3)
                    Player.Position.Extend(sender.Position, sender.Distance(ObjectManager.Player) + (float)E.Width / 2));
            }
        }

        public static void OnEndScene(EventArgs args)
        {
            var wRange = new Circle(new ColorBGRA(138, 43, 226, 255), W.Range);

            var eRange = new Circle(new ColorBGRA(138, 43, 226, 255), E.Range);

            var rRange = new Circle(new ColorBGRA(138, 43, 226, 255), R.Range);

            if (GetOption(DMenu, "RD"))
            {
                foreach (var unit in EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget() && u.IsHPBarRendered))
                {
                    var offset = new Vector2(-10, 14);
                    var damage = States.CalcDmg(unit, false);
                    if (!(damage > 0))
                    {
                        continue;
                    }
                    var dmgPercent = ((unit.Health - damage) > 0 ? (unit.Health - damage) : 0) / unit.MaxHealth;
                    var healthPercent = unit.Health / unit.MaxHealth;
                    var start = new Vector2(
                        (int)(unit.HPBarPosition.X + offset.X + dmgPercent * 104),
                        (int)(unit.HPBarPosition.Y + offset.Y) - 5);
                    var end = new Vector2(
                        (int)(unit.HPBarPosition.X + offset.X + healthPercent * 104) + 1,
                        (int)(unit.HPBarPosition.Y + offset.Y) - 5);
                    Drawing.DrawLine(start, end, 9, Color.BlueViolet);
                }
            }
            if (GetOption(DMenu, "W"))
            {
                wRange.Draw(Player.Position);
                //Drawing.DrawCircle(Player.Position, W.Range, Color.BlueViolet);
            }
            if (GetOption(DMenu, "E"))
            {
                eRange.Draw(Player.Position);
                //Drawing.DrawCircle(Player.Position, E.Range, Color.BlueViolet);
            }
            if (GetOption(DMenu, "R"))
            {
                rRange.Draw(Player.Position);
                //Drawing.DrawCircle(Player.Position, R.Range, Color.BlueViolet);
            }
        }

        public static dynamic GetOption(Menu menu, string option)
        {
            if (menu[option] is CheckBox)
            {
                return menu[option].Cast<CheckBox>().CurrentValue;
            }
            if (menu[option] is Slider)
            {
                return menu[option].Cast<Slider>().CurrentValue;
            }
            if (menu[option] is KeyBind)
            {
                return menu[option].Cast<KeyBind>().CurrentValue;
            }
            return null;
        }

        public static void OnUpdate(EventArgs args)
        {
            //States.PillarBlock();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                States.DoCombo(GetOption(CMenu, "W"), GetOption(CMenu, "E"), GetOption(CMenu, "R"), false);
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                States.DoCombo(GetOption(HMenu, "W"), GetOption(HMenu, "E"), false, true);
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                     || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                States.Clear();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
            }
        }
    }
}