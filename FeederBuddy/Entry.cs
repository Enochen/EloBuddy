namespace FeederBuddy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using FeederBuddy.SRShopAI;

    using SharpDX;

    internal class Entry
    {
        #region Static Fields

        public static bool BotVectorReached;

        public static List<ChampWrapper> ListChamp = new List<ChampWrapper>
                                                         {
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Blitzcrank",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Bard",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "DrMundo",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.R }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Draven",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Evelynn",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Garen",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.Q }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Hecarim",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.E }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Karma",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.E }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Kayle",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Kennen",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.E }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Lulu",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "MasterYi",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.R }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Nunu",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Olaf",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.R }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Orianna",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Poppy",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Quinn",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.R }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Rammus",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.Q }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Rumble",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Ryze",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.R }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Shyvana",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Singed",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.R }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Sivir",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.R }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Skarner",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Sona",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.E }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Teemo",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Trundle",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.W }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Twitch",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.Q }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Udyr",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.E }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Volibear",
                                                                     SpellSlots = new List<SpellSlot> { SpellSlot.Q }
                                                                 },
                                                             new ChampWrapper
                                                                 {
                                                                     Name = "Zilean",
                                                                     SpellSlots =
                                                                         new List<SpellSlot> { SpellSlot.W, SpellSlot.E }
                                                                 }
                                                         };

        public static Menu Menu, FeedMenu, MiscMenu;

        public static bool TopVectorReached;

        private static readonly Vector3 BlueSpawn = new Vector3(416f, 468f, 182f);

        private static readonly Vector3 BotVector3 = new Vector3(12608, 2380, 52);

        private static readonly AIHeroClient player = Player.Instance;

        private static readonly Vector3 PurpleSpawn = new Vector3(14286f, 14382f, 172f);

        private static readonly Vector3 TopVector3 = new Vector3(2122, 12558, 53);

        private static string[] deaths;

        private static int globalRand;

        private static int lastLaugh;

        private static double lastTouchdown;

        private static float realTime;

        private static bool surrenderActive;

        private static int surrenderTime;

        private static double timeDead;

        #endregion

        #region Public Methods and Operators

        public static void OnLoad(EventArgs args)
        {
            try
            {
                deaths = new[]
                             {
                                 "/all XD", "kek", "sorry lag", "/all gg", "help pls", "nooob wtf", "team???",
                                 "/all gg my team sucks", "/all matchmaking sucks", "i can't carry dis", "wtf how?",
                                 "wow rito nerf pls", "/all report enemys for drophacks", "tilidin y u do dis", "kappa",
                                 "amk", "/all einfach mal leben genießen amk"
                             };

                if (player.Gold >= 0)
                {
                    realTime = Game.Time;
                }

                InitializeMenu.Load();
                Main.Init();
                Game.OnUpdate += OnUpdate;
                Game.OnEnd += OnEnd;
                Player.OnIssueOrder += OnIssueOrder;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }

        #endregion

        #region Methods

        private static void Feed()
        {
            var feedingMode = Menu["FeedingMode"].Cast<Slider>().CurrentValue;
            if (feedingMode == 3 && globalRand == -1)
            {
                var rnd = new Random();
                globalRand = rnd.Next(0, 3);
            }

            if (feedingMode != 3)
            {
                globalRand = -1;
            }

            if (player.IsDead)
            {
                globalRand = -1;
            }

            if (globalRand != -1)
            {
                feedingMode = globalRand;
            }

            switch (feedingMode)
            {
                case 0:
                    {
                        if (player.Team == GameObjectTeam.Order)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, PurpleSpawn);
                        }
                        else
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, BlueSpawn);
                        }
                    }
                    break;
                case 1:
                    {
                        if (player.Team == GameObjectTeam.Order)
                        {
                            if (!BotVectorReached)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, BotVector3);
                            }
                            else if (BotVectorReached)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, PurpleSpawn);
                            }
                        }
                        else
                        {
                            if (!BotVectorReached)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, BotVector3);
                            }
                            else if (BotVectorReached)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, BlueSpawn);
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        if (player.Team == GameObjectTeam.Order)
                        {
                            if (!TopVectorReached)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, TopVector3);
                            }
                            else if (TopVectorReached)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, PurpleSpawn);
                            }
                        }
                        else
                        {
                            if (!TopVectorReached)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, TopVector3);
                            }
                            else if (TopVectorReached)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, BlueSpawn);
                            }
                        }
                    }
                    break;
            }

            if (FeedMenu["SpellsActivated"].Cast<CheckBox>().CurrentValue)
            {
                Spells();
            }

            if (FeedMenu["MessagesActivated"].Cast<CheckBox>().CurrentValue)
            {
                Messages();
            }

            if (FeedMenu["LaughActivated"].Cast<CheckBox>().CurrentValue)
            {
                Laughing();
            }
        }

        private static void Laughing()
        {
            if (Environment.TickCount <= lastLaugh + 2500)
            {
                return;
            }

            Chat.Say("/l");
            lastLaugh = Environment.TickCount;
        }

        private static void Messages()
        {
            if (player.IsDead && Game.Time - timeDead > 80)
            {
                var r = new Random();
                Chat.Say(deaths[r.Next(0, 17)]);
                timeDead = Game.Time;
            }

            if (player.Team == GameObjectTeam.Chaos && player.Distance(BlueSpawn) < 600)
            {
                if (Game.Time - lastTouchdown > 80)
                {
                    Chat.Say("/all TOUCHDOWN!");
                    lastTouchdown = Game.Time;
                }
            }

            if (player.Team == GameObjectTeam.Order && player.Distance(PurpleSpawn) < 600)
            {
                if (Game.Time - lastTouchdown > 80)
                {
                    Chat.Say("/all TOUCHDOWN!");
                    lastTouchdown = Game.Time;
                }
            }
        }

        private static void OnEnd(EventArgs args)
        {
            Chat.Say("/all GGWP");
        }

        private static void OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (!FeedMenu["AttacksDisabled"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (sender.IsMe && args.Order == GameObjectOrder.AttackTo)
            {
                args.Process = false;
            }
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Menu["FeedingActivated"].Cast<CheckBox>().CurrentValue)
            {
                Feed();
            }

            if (MiscMenu["SurrenderActivated"].Cast<CheckBox>().CurrentValue)
            {
                Surrender();
            }

            if (Player.Instance.IsDead)
            {
                TopVectorReached = false;
                BotVectorReached = false;
            }
            else
            {
                if (Player.Instance.Distance(BotVector3) <= 400)
                {
                    BotVectorReached = true;
                }

                if (Player.Instance.Distance(TopVector3) <= 400)
                {
                    TopVectorReached = true;
                }
            }
        }

        private static void Spells()
        {
            if (player.Distance(PurpleSpawn) < 600 | player.Distance(BlueSpawn) < 600)
            {
                return;
            }

            var entry = ListChamp.FirstOrDefault(h => h.Name == ObjectManager.Player.ChampionName);

            if (entry == null)
            {
                return;
            }

            var slots = entry.SpellSlots;

            foreach (var slot in slots)
            {
                player.Spellbook.LevelSpell(slot);
                if (player.Spellbook.CanUseSpell(slot) == SpellState.Ready)
                {
                    player.Spellbook.CastSpell(slot, player);
                }
            }
        }

        private static void Surrender()
        {
            if (Game.Time - realTime >= 1200 && !surrenderActive)
            {
                Chat.Say("/ff");
                surrenderActive = true;
                surrenderTime = Environment.TickCount;
            }

            if (surrenderActive)
            {
                if (Environment.TickCount - surrenderTime > 180000)
                {
                    surrenderActive = false;
                }
            }
        }

        #endregion
    }
}