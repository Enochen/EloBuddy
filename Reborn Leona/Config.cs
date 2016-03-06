// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Reborn_Leona
{
    using System.Drawing;

    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public static class Config
    {
        public static class Modes
        {
            public static class Combo
            {
                private static readonly CheckBox _useQ;

                private static readonly CheckBox _useW;

                private static readonly CheckBox _useE;

                private static readonly CheckBox _useR;

                private static readonly Slider _minMana;

                public static int Mana
                {
                    get
                    {
                        return _minMana.CurrentValue;
                    }
                }

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

                static Combo()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("Combo");
                    _useQ = ModesMenu.Add("comboQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("comboW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("comboE", new CheckBox("Use E"));
                    _useR = ModesMenu.Add("comboR", new CheckBox("Use R"));
                    _minMana = ModesMenu.Add("comboMana", new Slider("Minimum Mana %"));
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

                private static readonly CheckBox _drawStackStatus;

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

                public static bool DrawStackStatus
                {
                    get
                    {
                        return _drawStackStatus.CurrentValue;
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
                    _drawStackStatus = DrawMenu.Add("drawStackStatus", new CheckBox("Draw Stacking Status Text"));
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

                private static readonly Slider _minMana;

                public static int Mana
                {
                    get
                    {
                        return _minMana.CurrentValue;
                    }
                }

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

                static Harass()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("Harass");
                    _useQ = ModesMenu.Add("harassQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("harassW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("harassE", new CheckBox("Use E"));
                    _minMana = ModesMenu.Add("harassMana", new Slider("Minimum Mana"));
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

                private static readonly Slider _minMana;

                public static int Mana
                {
                    get
                    {
                        return _minMana.CurrentValue;
                    }
                }

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

                static JungleClear()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("JungleClear");
                    _useQ = ModesMenu.Add("jungleQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("jungleW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("jungleE", new CheckBox("Use E", false));
                    _minMana = ModesMenu.Add("jungleMana", new Slider("Minimum Mana"));
                }

                public static void Initialize()
                {
                }
            }

            public static class KillSteal
            {
                private static readonly CheckBox _useQ;

                private static readonly CheckBox _useE;

                private static readonly CheckBox _useR;

                public static bool UseQ
                {
                    get
                    {
                        return _useQ.CurrentValue;
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

                static KillSteal()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("KillSteal");
                    _useQ = ModesMenu.Add("killQ", new CheckBox("Use Q"));
                    _useE = ModesMenu.Add("killE", new CheckBox("Use E", false));
                    _useR = ModesMenu.Add("killR", new CheckBox("Use R", false));
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

                private static readonly Slider _minMana;

                public static int Mana
                {
                    get
                    {
                        return _minMana.CurrentValue;
                    }
                }

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
                    ModesMenu.AddGroupLabel("LaneClear");
                    _useQ = ModesMenu.Add("laneQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("laneW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("laneE", new CheckBox("Use E", false));
                    _useR = ModesMenu.Add("laneR", new CheckBox("Use R", false));
                    _minMana = ModesMenu.Add("laneMana", new Slider("Minimum Mana"));
                }

                public static void Initialize()
                {
                }
            }

            public static class LastHit
            {
                private static readonly CheckBox _useQ;

                private static readonly Slider _minMana;

                public static int Mana
                {
                    get
                    {
                        return _minMana.CurrentValue;
                    }
                }

                public static bool UseQ
                {
                    get
                    {
                        return _useQ.CurrentValue;
                    }
                }

                static LastHit()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("LastHit");
                    _useQ = ModesMenu.Add("lastQ", new CheckBox("Use Q"));
                    _minMana = ModesMenu.Add("lastMana", new Slider("Minimum Mana"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Misc
            {
                private static readonly CheckBox _AutoQInterruptible;
                private static readonly CheckBox _AutoQGapCloser;
                private static readonly CheckBox _AutoRObjectives;
                private static readonly Slider _REnemiesHit;

                public static bool AutoQInterruptible
                {
                    get
                    {
                        return _AutoQInterruptible.CurrentValue;
                    }
                }

                public static bool AutoQGapCloser
                {
                    get
                    {
                        return _AutoQGapCloser.CurrentValue;
                    }
                }
                public static bool AutoRObjectives
                {
                    get
                    {
                        return _AutoRObjectives.CurrentValue;
                    }
                }
                public static int REnemiesHit
                {
                    get
                    {
                        return _REnemiesHit.CurrentValue;
                    }
                }

                static Misc()
                {
                    MiscMenu.AddGroupLabel("Miscellaneous");

                    _AutoQGapCloser = MiscMenu.Add("AutoQGapCloser", new CheckBox("Auto Q-Stun On Gapcloser"));
                    _AutoQInterruptible = MiscMenu.Add("AutoQInterruptible", new CheckBox("Auto Q-Stun On Interruptible"));
                    _AutoRObjectives = MiscMenu.Add("AutoRObjectives", new CheckBox("Auto Ult Steal Objectives"));
                    _REnemiesHit = MiscMenu.Add("REnemiesHit", new Slider("Min Enemies Hit With Ult", 1, 1, EntityManager.Heroes.Enemies.Count));
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
                DrawMenu = Menu.AddSubMenu("Draw");
                Draw.Initialize();
            }

            public static void Initialize()
            {
            }
        }

        private const string MenuName = "Reborn Leona";

        private static readonly Menu Menu;

        static Config()
        {
            // Initialize the menu
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Reborn Leona");
            Menu.AddLabel("Rewrite of Radiant Leona - 10/10", 50);
            Menu.AddLabel("By Darakath", 50);

            // Initialize the modes
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }
    }
}