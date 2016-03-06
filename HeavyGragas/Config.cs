namespace HeavyGragas
{
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

                private static readonly Slider _rEnemies;

                private static readonly Slider MinMana;

                public static int REnemies
                {
                    get
                    {
                        return _rEnemies.CurrentValue;
                    }
                }

                public static int Mana
                {
                    get
                    {
                        return MinMana.CurrentValue;
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
                    ModesMenu.AddGroupLabel("Combo");
                    _useQ = ModesMenu.Add("comboQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("comboW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("comboE", new CheckBox("Use E"));
                    _useR = ModesMenu.Add("comboR", new CheckBox("Use R"));
                    MinMana = ModesMenu.Add("comboMana", new Slider("Minimum Mana %"));
                    _rEnemies = ModesMenu.Add(
                        "comboREnemies",
                        new Slider("Cast R If Can Hit", 3, 1, EntityManager.Heroes.Enemies.Count));
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

                private static readonly CheckBox _drawBarrelCircle;

                private static readonly CheckBox _drawBarrelText;

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

                public static bool DrawBarrelCircle
                {
                    get
                    {
                        return _drawBarrelCircle.CurrentValue;
                    }
                }

                public static bool DrawBarrelText
                {
                    get
                    {
                        return _drawBarrelText.CurrentValue;
                    }
                }

                public static float WidthQ
                {
                    get
                    {
                        return DrawMenu.GetWidth("widthQ");
                    }
                }

                public static float WidthW
                {
                    get
                    {
                        return DrawMenu.GetWidth("widthW");
                    }
                }

                public static float WidthE
                {
                    get
                    {
                        return DrawMenu.GetWidth("widthE");
                    }
                }

                public static float WidthR
                {
                    get
                    {
                        return DrawMenu.GetWidth("widthR");
                    }
                }

                public static float WidthCircle
                {
                    get
                    {
                        return DrawMenu.GetWidth("widthCircle");
                    }
                }

                static Draw()
                {
                    DrawMenu.AddGroupLabel("Draw");
                    _drawReady = DrawMenu.Add("drawReady", new CheckBox("Draw Only If The Spells Are Ready.", false));
                    _drawHealth = DrawMenu.Add("drawHealth", new CheckBox("Draw Damage in HealthBar"));
                    _drawBarrelText = DrawMenu.Add("drawBarrelText", new CheckBox("Draw Barrel Text"));
                    _drawBarrelCircle = DrawMenu.Add("drawBarrelCircle", new CheckBox("Draw Barrel Circle"));
                    DrawMenu.AddWidthItem("Barrel Circle Width: ", "widthCircle");
                    DrawMenu.AddSeparator();

                    _drawQ = DrawMenu.Add("drawQ", new CheckBox("Draw Q"));
                    DrawMenu.AddWidthItem("Q Range Width: ", "widthQ");

                    _drawW = DrawMenu.Add("drawW", new CheckBox("Draw W"));
                    DrawMenu.AddWidthItem("W Range Width: ", "widthW");

                    _drawE = DrawMenu.Add("drawE", new CheckBox("Draw E"));
                    DrawMenu.AddWidthItem("E Range Width: ", "widthE");

                    _drawR = DrawMenu.Add("drawR", new CheckBox("Draw R"));
                    DrawMenu.AddWidthItem("R Range Width: ", "widthR");
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

                private static readonly Slider MinMana;

                public static int Mana
                {
                    get
                    {
                        return MinMana.CurrentValue;
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
                    ModesMenu.AddGroupLabel("Harass");
                    _useQ = ModesMenu.Add("harassQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("harassW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("harassE", new CheckBox("Use E"));
                    MinMana = ModesMenu.Add("harassMana", new Slider("Minimum Mana"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Insec
            {
                private static readonly KeyBind _useInsec;

                private static readonly CheckBox _useFlash;

                public static bool UseInsec
                {
                    get
                    {
                        return _useInsec.CurrentValue;
                    }
                }

                public static bool UseFlash
                {
                    get
                    {
                        return _useFlash.CurrentValue;
                    }
                }

                static Insec()
                {
                    ModesMenu.AddGroupLabel("Insec");
                    _useInsec = ModesMenu.Add(
                        "Insec",
                        new KeyBind("Insec", false, KeyBind.BindTypes.HoldActive, 'C'));

                    _useFlash = ModesMenu.Add("insecFlash", new CheckBox("Use Flash if Barrel is already placed", false));
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

                private static readonly Slider MinMana;

                public static int Mana
                {
                    get
                    {
                        return MinMana.CurrentValue;
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
                    ModesMenu.AddGroupLabel("JungleClear");
                    _useQ = ModesMenu.Add("jungleQ", new CheckBox("Use Q"));
                    _useW = ModesMenu.Add("jungleW", new CheckBox("Use W"));
                    _useE = ModesMenu.Add("jungleE", new CheckBox("Use E", false));
                    MinMana = ModesMenu.Add("jungleMana", new Slider("Minimum Mana"));
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

                private static readonly Slider MinMana;

                public static int Mana
                {
                    get
                    {
                        return MinMana.CurrentValue;
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
                    MinMana = ModesMenu.Add("laneMana", new Slider("Minimum Mana"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Misc
            {
                private static readonly CheckBox _AutoEInterruptible;

                private static readonly CheckBox _AutoEGapCloser;

                private static readonly CheckBox _AutoQ2Enemy;

                public static bool AutoEInterruptible
                {
                    get
                    {
                        return _AutoEInterruptible.CurrentValue;
                    }
                }

                public static bool AutoEGapCloser
                {
                    get
                    {
                        return _AutoEGapCloser.CurrentValue;
                    }
                }

                public static bool AutoQ2Enemy
                {
                    get
                    {
                        return _AutoQ2Enemy.CurrentValue;
                    }
                }

                static Misc()
                {
                    MiscMenu.AddGroupLabel("Miscellaneous");

                    _AutoEGapCloser = MiscMenu.Add("AutoEGapCloser", new CheckBox("Auto Q-Stun On Gapcloser"));
                    _AutoEInterruptible = MiscMenu.Add(
                        "AutoEInterruptible",
                        new CheckBox("Auto Q-Stun On Interruptible"));

                    _AutoQ2Enemy = MiscMenu.Add("AutoQ2Enemy", new CheckBox("Auto Q2 Before Enemy Walks Out"));
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
                Insec.Initialize();
                Menu.AddSeparator();
                Harass.Initialize();
                Menu.AddSeparator();
                LaneClear.Initialize();

                MiscMenu = Menu.AddSubMenu("Misc");
                Misc.Initialize();
                DrawMenu = Menu.AddSubMenu("Draw");
                Draw.Initialize();
            }

            public static void Initialize()
            {
            }
        }

        private const string MenuName = "HeavyGragas";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("HeavyGragas");
            Menu.AddLabel("5/7 - IGN", 50);
            Menu.AddLabel("By Darakath", 50);

            Modes.Initialize();
        }

        public static void Initialize()
        {
        }
    }
}