namespace dRiven
{
    using System;
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    using MainMenu = EloBuddy.SDK.Menu.MainMenu;

    internal class Program
    {
        public static int QCount
        {
            get
            {
                var first = Player.Buffs.FirstOrDefault(x => x.Name == "RivenTriCleave");
                return Player.HasBuff("RivenTriCleave") ? first.Count : 0;
            }
        }

        public static Menu menu, cMenu, hMenu, fMenu, mMenu;
        
        public static float EwRange
        {
            get
            {
                return E.Range + W.Range / 2 + Player.BoundingRadius;
            }
        }

        public static Spell.Skillshot Q, E, R2;

        public static Spell.Active W, R;

        
        public static Spell.Targeted Fire;

        public static int LastQ { get; internal set; }

        public static bool RActivated
        {
            get
            {
                return Player.HasBuff("RivenFengShuiEngine");
            }
        }

        public static bool CanU2
        {
            get
            {
                return Player.HasBuff("rivenwindslashready");
            }
        }

        public static float PassiveDmg
        {
            get
            {
                double[] dmgMult = { 0.2, 0.25, 0.3, 0.35, 0.4, 0.45, 0.5 };
                var dmg = (Player.FlatPhysicalDamageMod + Player.BaseAttackDamage) * dmgMult[Player.Level / 3];
                return (float)dmg;
            }
        }

        public static AIHeroClient Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        public static Obj_AI_Base LastTarget { get; set; }
        public static bool CanQ { get; set; }


        public static bool HasSpell(string s)
        {
            return EloBuddy.Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        public static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Riven")
            {
                return;
            }

            SetUpMenu();

            Q = new Spell.Skillshot(SpellSlot.Q, 260, SkillShotType.Cone, 500, 1400, 100);
            W = new Spell.Active(SpellSlot.W, 125);
            E = new Spell.Skillshot(SpellSlot.E, 325, SkillShotType.Linear);
            R = new Spell.Active(SpellSlot.R);
            R2 = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 125);

            if (HasSpell("summonerdot"))
            {
                Fire = new Spell.Targeted(Player.GetSpellSlotFromName("summonerdot"), 600);
            }

            Obj_AI_Base.OnPlayAnimation += OnPlayAnimation;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;

            Game.OnUpdate += StateManager.OnGameUpdate;
            Drawing.OnDraw += DrawingOnOnDraw;

            Chat.Print("dRiven by Darakath Loaded", Color.BlanchedAlmond);
            
        }

        private static void DrawingOnOnDraw(EventArgs args)
        {
            var drawR = (R.IsLearned && R2.IsReady() && CanU2);
            if (drawR)
            {
                Circle.Draw(Color.MediumAquamarine, R.Range, Player.Position);
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name == "RivenFengShuiEngine")
            {
                Core.DelayAction(
                    delegate
                    {
                        if (CanU2)
                        {
                            var bestTarget =
                                HeroManager.Enemies
                                    .Where(x => x.IsValidTarget(R.Range))
                                    .OrderBy(x => x.Health)
                                    .FirstOrDefault();

                            if (bestTarget != null)
                            {
                                R2.Cast(bestTarget);
                            }
                        }
                    }, 15000 - Game.Ping / 2 - R2.CastDelay * 1000
                    );
                    
            }
        }

        public static bool GetBool(Menu Menu, string id)
        {
            return Menu[id].Cast<CheckBox>().CurrentValue;
        }

        private static void SetUpMenu()
        {
            menu = MainMenu.AddMenu("dRiven", "menu");
            menu.AddGroupLabel("dRiven By Darakath");
            menu.AddLabel("Get it? Nvm.");

            cMenu = menu.AddSubMenu("Combo", "cMenu");
            cMenu.Add("q.c", new CheckBox("Use Q"));
            cMenu.Add("w.c", new CheckBox("Use W"));
            cMenu.Add("e.c", new CheckBox("Use E"));
            cMenu.Add("r.c", new CheckBox("Use R"));

            hMenu = menu.AddSubMenu("Harass", "hMenu");
            hMenu.Add("q.h", new CheckBox("Use Q"));
            hMenu.Add("w.h", new CheckBox("Use W"));
            hMenu.Add("e.h", new CheckBox("Use E"));

            fMenu = menu.AddSubMenu("Farm", "fMenu");
            fMenu.Add("q.f", new CheckBox("Use Q"));
            fMenu.Add("w.f", new CheckBox("Use W"));
            fMenu.Add("e.f", new CheckBox("Use E"));

            mMenu = menu.AddSubMenu("Misc", "mMenu");
            mMenu.Add("m.q", new CheckBox("Auto use Q"));
            mMenu.Add("m.r", new CheckBox("Auto use R"));
        }

        private static void OnPlayAnimation(GameObject sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.Animation.Contains("Spell1"))
            {
                LastQ = Environment.TickCount;
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    Core.DelayAction(delegate { EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos); }, 100);
                    Core.DelayAction(Orbwalker.ResetAutoAttack, (int)(100 + Player.AttackDelay * 100));
                }
            }

            if (args.Animation.Contains("Attack") &&
                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)))
            {
                float aaDelay;
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                        aaDelay = Player.AttackDelay * 300 + Game.Ping / 2f;
                }
                else
                {
                    aaDelay = Player.AttackDelay * 300 + Game.Ping / 2f;
                }
                Core.DelayAction(
                    () =>
                        {
                            if ((GetBool(cMenu, "q.c")
                                 && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                                 || (GetBool(hMenu, "q.h")
                                     && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)
                                     || (GetBool(fMenu, "q.f")
                                         && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)))))

                            {
                                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Q, LastTarget.Position); ;
                            }
                        
            }, (int)(aaDelay));

            }
        }
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnGameLoad;
        }
    }
}