// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace SpaceAIDS
{
    using System.Drawing;

    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public static class Config
    {
        public static class Modes
        {
            public static class Combo
            {
                private static readonly CheckBox useQ;

                private static readonly CheckBox useW;

                private static readonly CheckBox useE;

                private static readonly CheckBox useR;

                private static readonly CheckBox useF;

                public static bool UseQ
                {
                    get
                    {
                        return useQ.CurrentValue;
                    }
                }

                public static bool UseW
                {
                    get
                    {
                        return useW.CurrentValue;
                    }
                }

                public static bool UseE
                {
                    get
                    {
                        return useE.CurrentValue;
                    }
                }

                public static bool UseR
                {
                    get
                    {
                        return useR.CurrentValue;
                    }
                }

                public static bool UseIgnite
                {
                    get
                    {
                        return useF.CurrentValue;
                    }
                }

                static Combo()
                {
                    ModesMenu.AddGroupLabel("Combo");
                    useQ = ModesMenu.Add("comboQ", new CheckBox("Use Q"));
                    useW = ModesMenu.Add("comboW", new CheckBox("Use W"));
                    useE = ModesMenu.Add("comboE", new CheckBox("Use E"));
                    useR = ModesMenu.Add("comboR", new CheckBox("Use R", false));
                    useF = ModesMenu.Add("comboF", new CheckBox("Use Ignite"));
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

                private static readonly CheckBox _drawF;

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

                public static bool DrawF
                {
                    get
                    {
                        return _drawF.CurrentValue;
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

                public static Color colorF
                {
                    get
                    {
                        return DrawMenu.GetColor("colorF");
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

                public static float _widthF
                {
                    get
                    {
                        return DrawMenu.GetWidth("widthF");
                    }
                }

                static Draw()
                {
                    DrawMenu.AddGroupLabel("Draw");
                    _drawReady = Menu.Add("drawReady", new CheckBox("Draw Only If The Spells Are Ready.", false));
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
                    //Ignite
                    _drawF = DrawMenu.Add("drawF", new CheckBox("Draw Ignite"));
                    DrawMenu.AddColorItem("colorF");
                    DrawMenu.AddWidthItem("widthF");
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
                    _useW = ModesMenu.Add("harassW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("harassE", new CheckBox("Use E"));
                    _useR = ModesMenu.Add("harassR", new CheckBox("Use R", false));
                }

                public static void Initialize()
                {
                }
            }
            public static class Utils
            {
                private static readonly CheckBox _qGapcloser;

                private static readonly CheckBox _qInterruptable;

                public static bool UseQOnGapcloser
                {
                    get
                    {
                        return _qGapcloser.CurrentValue;
                    }
                }

                public static bool UseQOnInterruptable
                {
                    get
                    {
                        return _qInterruptable.CurrentValue;
                    }
                }

                static Utils()
                {
                    // Initialize the menu values
                    ModesMenu.AddGroupLabel("Harass");
                    _qGapcloser = ModesMenu.Add("qGapcloser", new CheckBox("Use Q on Gapcloser"));
                    _qInterruptable = ModesMenu.Add("qInterruptable", new CheckBox("Use Q on Interruptable"));
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
                    _useW = ModesMenu.Add("laneW", new CheckBox("Use W", false));
                    _useE = ModesMenu.Add("laneE", new CheckBox("Use E"));
                    _useR = ModesMenu.Add("laneR", new CheckBox("Use R", false)); // Default false
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
                    _useW = ModesMenu.Add("lastW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("lastE", new CheckBox("Use E"));
                    _useR = ModesMenu.Add("lastR", new CheckBox("Use R", false)); // Default false
                }

                public static void Initialize()
                {
                }
            }

            private static readonly Menu ModesMenu, DrawMenu;

            static Modes()
            {
                ModesMenu = Menu.AddSubMenu("Modes");

                Combo.Initialize();
                Menu.AddSeparator();
                Harass.Initialize();
                Menu.AddSeparator();
                Utils.Initialize();
                //LaneClear.Initialize();
                //Menu.AddSeparator();
                //LastHit.Initialize();

                DrawMenu = Menu.AddSubMenu("Draw");
                Draw.Initialize();
            }

            public static void Initialize()
            {
            }
        }

        private const string MenuName = "SpaceAIDS";

        private static readonly Menu Menu;

        static Config()
        {
            // Initialize the menu
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("SpaceAIDS");
            Menu.AddLabel("By Darakath", 50);

            // Initialize the modes
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }
    }
}