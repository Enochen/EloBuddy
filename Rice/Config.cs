// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Rice
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public static class Config
    {
        public static class Modes
        {
            public static class AutoStack
            {
                private static readonly Slider _AutoStackMana;

                private static readonly Slider _MaxStacks;

                private static readonly Slider _StackTimer;

                private static readonly KeyBind _AutoStackQ;

                public static int AutoStackMana
                {
                    get
                    {
                        return _AutoStackMana.CurrentValue;
                    }
                }

                public static bool AutoStackQ
                {
                    get
                    {
                        return _AutoStackQ.CurrentValue;
                    }
                }

                public static int MaxStacks
                {
                    get
                    {
                        return _MaxStacks.CurrentValue;
                    }
                }

                public static int StackTimer
                {
                    get
                    {
                        return _StackTimer.CurrentValue;
                    }
                }

                static AutoStack()
                {
                    MiscMenu.AddGroupLabel("Auto Stack");
                    _AutoStackQ = MiscMenu.Add(
                        "AutoStackQ",
                        new KeyBind("Auto Stack Passive", false, KeyBind.BindTypes.PressToggle, 'Z'));

                    _AutoStackMana = MiscMenu.Add("AutoStackMana", new Slider("Min Mana %", 30));

                    _MaxStacks = MiscMenu.Add("MaxStacks", new Slider("Keep Stacks At", 3, 1, 4));

                    _StackTimer = MiscMenu.Add("StackTimer", new Slider("Stack Every <Second>", 5, 1, 10));
                }

                public static void Initialize()
                {
                }
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;

                private static readonly CheckBox _useW;

                private static readonly CheckBox _useE;

                private static readonly CheckBox _useR;

                private static readonly CheckBox _blockAA;

                public static bool UseQ
                {
                    get
                    {
                        return _useQ.CurrentValue;
                    }
                }

                public static bool UseW
                {
                    get
                    {
                        return _useW.CurrentValue;
                    }
                }

                public static bool UseE
                {
                    get
                    {
                        return _useE.CurrentValue;
                    }
                }

                public static bool UseR
                {
                    get
                    {
                        return _useR.CurrentValue;
                    }
                }

                public static bool blockAA
                {
                    get
                    {
                        return _blockAA.CurrentValue;
                    }
                }

                static Combo()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("Combo");
                    _useQ = ModesMenu.Add("comboQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("comboW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("comboE", new CheckBox("Use E"));
                    _useR = ModesMenu.Add("comboR", new CheckBox("Use R"));
                    _blockAA = ModesMenu.Add("blockAA", new CheckBox("Block AA"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Draw
            {
                private static readonly CheckBox _drawHealth;

                private static readonly CheckBox _drawQ;

                private static readonly CheckBox _drawW;

                private static readonly CheckBox _drawE;

                private static readonly CheckBox _drawR;

                private static readonly CheckBox _drawReady;

                public static bool DrawHealth
                {
                    get
                    {
                        return _drawHealth.CurrentValue;
                    }
                }

                public static bool DrawQ
                {
                    get
                    {
                        return _drawQ.CurrentValue;
                    }
                }

                public static bool DrawW
                {
                    get
                    {
                        return _drawW.CurrentValue;
                    }
                }

                public static bool DrawE
                {
                    get
                    {
                        return _drawE.CurrentValue;
                    }
                }

                public static bool DrawR
                {
                    get
                    {
                        return _drawR.CurrentValue;
                    }
                }

                public static bool DrawReady
                {
                    get
                    {
                        return _drawReady.CurrentValue;
                    }
                }

                public static Color colorHealth
                {
                    get
                    {
                        return DrawMenu.GetColor("colorHealth");
                    }
                }

                public static Color colorQ
                {
                    get
                    {
                        return DrawMenu.GetColor("colorQ");
                    }
                }

                public static Color colorW
                {
                    get
                    {
                        return DrawMenu.GetColor("colorW");
                    }
                }

                public static Color colorE
                {
                    get
                    {
                        return DrawMenu.GetColor("colorE");
                    }
                }

                public static Color colorR
                {
                    get
                    {
                        return DrawMenu.GetColor("colorR");
                    }
                }

                public static float _widthQ
                {
                    get
                    {
                        return DrawMenu.GetWidth("widthQ");
                    }
                }

                public static float _widthW
                {
                    get
                    {
                        return DrawMenu.GetWidth("widthW");
                    }
                }

                public static float _widthE
                {
                    get
                    {
                        return DrawMenu.GetWidth("widthE");
                    }
                }

                public static float _widthR
                {
                    get
                    {
                        return DrawMenu.GetWidth("widthR");
                    }
                }

                static Draw()
                {
                    DrawMenu.AddGroupLabel("Draw");
                    _drawReady = DrawMenu.Add("drawReady", new CheckBox("Draw Only If The Spells Are Ready.", false));
                    _drawHealth = DrawMenu.Add("drawHealth", new CheckBox("Draw Damage in HealthBar"));
                    DrawMenu.AddColorItem("colorHealth");
                    DrawMenu.AddSeparator();
                    //Q
                    _drawQ = DrawMenu.Add("drawQ", new CheckBox("Draw Q"));
                    DrawMenu.AddColorItem("colorQ");
                    DrawMenu.AddWidthItem("widthQ");
                    //W
                    _drawW = DrawMenu.Add("drawW", new CheckBox("Draw W"));
                    DrawMenu.AddColorItem("colorW");
                    DrawMenu.AddWidthItem("widthW");
                    //E
                    _drawE = DrawMenu.Add("drawE", new CheckBox("Draw E"));
                    DrawMenu.AddColorItem("colorE");
                    DrawMenu.AddWidthItem("widthE");
                    //R
                    _drawR = DrawMenu.Add("drawR", new CheckBox("Draw R"));
                    DrawMenu.AddColorItem("colorR");
                    DrawMenu.AddWidthItem("widthR");
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;

                private static readonly CheckBox _useW;

                private static readonly CheckBox _useE;

                private static readonly CheckBox _useR;

                public static bool UseQ
                {
                    get
                    {
                        return _useQ.CurrentValue;
                    }
                }

                public static bool UseW
                {
                    get
                    {
                        return _useW.CurrentValue;
                    }
                }

                public static bool UseE
                {
                    get
                    {
                        return _useE.CurrentValue;
                    }
                }

                public static bool UseR
                {
                    get
                    {
                        return _useR.CurrentValue;
                    }
                }

                static Harass()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("Harass");
                    _useQ = ModesMenu.Add("harassQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("harassW", new CheckBox("Use W", false));
                    _useE = ModesMenu.Add("harassE", new CheckBox("Use E", false));
                    _useR = ModesMenu.Add("harassR", new CheckBox("Use R", false));
                }

                public static void Initialize()
                {
                }
            }

            public static class JungleClear
            {
                private static readonly CheckBox _useQ;

                private static readonly CheckBox _useW;

                private static readonly CheckBox _useE;

                private static readonly CheckBox _useR;

                public static bool UseQ
                {
                    get
                    {
                        return _useQ.CurrentValue;
                    }
                }

                public static bool UseW
                {
                    get
                    {
                        return _useW.CurrentValue;
                    }
                }

                public static bool UseE
                {
                    get
                    {
                        return _useE.CurrentValue;
                    }
                }

                public static bool UseR
                {
                    get
                    {
                        return _useR.CurrentValue;
                    }
                }

                static JungleClear()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("JungleClear");
                    _useQ = ModesMenu.Add("laneQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("laneW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("laneE", new CheckBox("Use E"));
                    _useR = ModesMenu.Add("laneR", new CheckBox("Use R"));
                }

                public static void Initialize()
                {
                }
            }

            public static class KillSteal
            {
                private static readonly CheckBox _useQ;

                private static readonly CheckBox _useW;

                private static readonly CheckBox _useE;

                public static bool UseQ
                {
                    get
                    {
                        return _useQ.CurrentValue;
                    }
                }

                public static bool UseW
                {
                    get
                    {
                        return _useW.CurrentValue;
                    }
                }

                public static bool UseE
                {
                    get
                    {
                        return _useE.CurrentValue;
                    }
                }

                static KillSteal()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("KillSteal");
                    _useQ = ModesMenu.Add("killQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("killW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("killE", new CheckBox("Use E"));
                }

                public static void Initialize()
                {
                }
            }

            public static class LaneClear
            {
                private static readonly CheckBox _useQ;

                private static readonly CheckBox _useW;

                private static readonly CheckBox _useE;

                private static readonly CheckBox _useR;

                public static bool UseQ
                {
                    get
                    {
                        return _useQ.CurrentValue;
                    }
                }

                public static bool UseW
                {
                    get
                    {
                        return _useW.CurrentValue;
                    }
                }

                public static bool UseE
                {
                    get
                    {
                        return _useE.CurrentValue;
                    }
                }

                public static bool UseR
                {
                    get
                    {
                        return _useR.CurrentValue;
                    }
                }

                static LaneClear()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("LaneClear");
                    _useQ = ModesMenu.Add("laneQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("laneW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("laneE", new CheckBox("Use E"));
                    _useR = ModesMenu.Add("laneR", new CheckBox("Use R"));
                }

                public static void Initialize()
                {
                }
            }

            public static class LastHit
            {
                private static readonly CheckBox _useQ;

                private static readonly CheckBox _useW;

                private static readonly CheckBox _useE;

                private static readonly CheckBox _useR;

                public static bool UseQ
                {
                    get
                    {
                        return _useQ.CurrentValue;
                    }
                }

                public static bool UseW
                {
                    get
                    {
                        return _useW.CurrentValue;
                    }
                }

                public static bool UseE
                {
                    get
                    {
                        return _useE.CurrentValue;
                    }
                }

                public static bool UseR
                {
                    get
                    {
                        return _useR.CurrentValue;
                    }
                }

                static LastHit()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("LastHit");
                    _useQ = ModesMenu.Add("lastQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("lastW", new CheckBox("Use W", false));
                    _useE = ModesMenu.Add("lastE", new CheckBox("Use E", false));
                    _useR = ModesMenu.Add("lastR", new CheckBox("Use R", false)); // Default false
                }

                public static void Initialize()
                {
                }
            }

            public static class Misc
            {
                private static readonly Slider _QCollision;

                private static readonly CheckBox _ChangeNames;

                public static List<string> EnemyNames, AllyNames;

                public static int QCollision
                {
                    get
                    {
                        return _QCollision.CurrentValue;
                    }
                }

                public static bool ChangeNames
                {
                    get
                    {
                        return _ChangeNames.CurrentValue;
                    }
                }

                static Misc()
                {
                    MiscMenu.AddGroupLabel("Miscellaneous");
                    _QCollision = MiscMenu.Add("QCollision", new Slider("QCollision: Always Q At Enemy", 0, 0, 1));
                    _ChangeNames = MiscMenu.Add("ChangeNames", new CheckBox("Change Hero Names (Experimental)", false));

                    _QCollision.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                        {
                            if (args.NewValue == 1)
                            {
                                _QCollision.DisplayName = "QCollision: Only Q If No Collision";
                                SpellManager.Q.AllowedCollisionCount = 0;
                                return;
                            }
                            _QCollision.DisplayName = "QCollision: Always Q At Enemy";
                            SpellManager.Q.AllowedCollisionCount = int.MaxValue;
                        };
                    _ChangeNames.OnValueChange +=
                        delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                            {
                                if (args.NewValue == false)
                                {
                                    foreach (var enemy in EntityManager.Heroes.Enemies)
                                    {
                                        enemy.Name = EnemyNames[EntityManager.Heroes.Enemies.OrderBy(x => x.NetworkId).ToList().IndexOf(enemy)];
                                    }
                                    foreach (var ally in EntityManager.Heroes.Allies)
                                    {
                                        ally.Name = AllyNames[EntityManager.Heroes.Enemies.OrderBy(x => x.NetworkId).ToList().IndexOf(ally)];
                                    }
                                }
                                else
                                {
                                    foreach (var enemy in EntityManager.Heroes.Enemies)
                                    {
                                        enemy.Name = "Combo Me Pls";
                                    }
                                    foreach (var ally in EntityManager.Heroes.Allies)
                                    {
                                        ally.Name = "Don't Let Me KS";
                                    }
                                    Player.Instance.Name = "Best Rice EB";
                                }
                            };
                }

                public static void Initialize()
                {
                }
            }

            public static class TearStack
            {
                private static readonly Slider _AutoStackMana;

                private static readonly Slider _MaxStacks;

                private static readonly Slider _StackTimer;

                private static readonly CheckBox _OnlyFountain;

                private static readonly KeyBind _AutoStackQ;

                public static int AutoStackMana
                {
                    get
                    {
                        return _AutoStackMana.CurrentValue;
                    }
                }

                public static bool AutoStackQ
                {
                    get
                    {
                        return _AutoStackQ.CurrentValue;
                    }
                }

                public static bool OnlyFountain
                {
                    get
                    {
                        return _OnlyFountain.CurrentValue;
                    }
                }

                public static int MaxStacks
                {
                    get
                    {
                        return _MaxStacks.CurrentValue;
                    }
                }

                public static int StackTimer
                {
                    get
                    {
                        return _StackTimer.CurrentValue;
                    }
                }

                static TearStack()
                {
                    MiscMenu.AddGroupLabel("Tear Stack");
                    _AutoStackQ = MiscMenu.Add(
                        "AutoTear",
                        new KeyBind("Auto Stack Tear", false, KeyBind.BindTypes.PressToggle, 'T'));

                    _AutoStackMana = MiscMenu.Add("AutoTearMana", new Slider("Min Mana %", 30));

                    _MaxStacks = MiscMenu.Add("MaxStacksTear", new Slider("Keep Stacks At", 3, 1, 4));

                    _StackTimer = MiscMenu.Add("TearStackTimer", new Slider("Stack Every <Second>", 5, 1, 10));

                    _OnlyFountain = MiscMenu.Add("OnlyFountain", new CheckBox("Only Stack in Fountain", false));
                }

                public static void Initialize()
                {
                }
            }

            private static readonly Menu ModesMenu, DrawMenu, MiscMenu;

            static Modes()
            {
                ModesMenu = Menu.AddSubMenu("Modes");

                Combo.Initialize();
                Menu.AddSeparator();
                Harass.Initialize();
                Menu.AddSeparator();
                LaneClear.Initialize();
                Menu.AddSeparator();
                LastHit.Initialize();

                MiscMenu = Menu.AddSubMenu("Misc");
                Misc.Initialize();
                Menu.AddSeparator();
                AutoStack.Initialize();
                Menu.AddSeparator();
                TearStack.Initialize();

                DrawMenu = Menu.AddSubMenu("Draw");
                Draw.Initialize();
            }

            public static void Initialize()
            {
            }
        }

        private const string MenuName = "Rice";

        private static readonly Menu Menu;

        static Config()
        {
            // Initialize the menu
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Rice");
            Menu.AddLabel("10/10 with Darakath", 50);
            Menu.AddLabel("Thank you for your suggestions.", 50);

            // Initialize the modes
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }
    }
}