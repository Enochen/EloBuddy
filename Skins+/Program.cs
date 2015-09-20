namespace Skins_
{
    using System;
    using System.Collections.Generic;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Program
    {
        #region Static Fields

        private static readonly Dictionary<string, int> ChampSkins = new Dictionary<string, int>();

        private static readonly Dictionary<int, bool> Enabled = new Dictionary<int, bool>();

        private static readonly List<AIHeroClient> HeroList = new List<AIHeroClient>();

        private static readonly Dictionary<Obj_AI_Base, bool> WasDead = new Dictionary<Obj_AI_Base, bool>();

        private static Menu menu, heroSubMenu;

        private static Slider skinSelect;

        #endregion

        #region Methods

        private static void GameLoad(EventArgs argss)
        {
            menu = MainMenu.AddMenu("Skins+", "skins+");

            menu.AddGroupLabel("Skin+ by Darakath");
            menu.AddSeparator();
            menu.AddLabel("Use the Submenus to Change Skins");

            try
            {
                foreach (var hero in HeroManager.AllHeroes)
                {
                    HeroList.Add(hero);

                    WasDead.Add(hero, false);

                    Enabled.Add(hero.NetworkId, false);

                    var currenthero = hero;

                    heroSubMenu = menu.AddSubMenu(hero.ChampionName + " (" + hero.Name + ") ", hero.ChampionName);

                    skinSelect = heroSubMenu.Add("skin." + hero.ChampionName, new Slider("Skin ID", 0, 0, 15));

                    ChampSkins.Add(hero.Name, heroSubMenu["skin." + hero.ChampionName].Cast<Slider>().CurrentValue);

                    Enabled[hero.NetworkId] = true;
                    hero.SetSkin(hero.ChampionName, ChampSkins[hero.Name]);

                    skinSelect.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                        {
                            Enabled[hero.NetworkId] = true;
                            ChampSkins[currenthero.Name] = args.NewValue;
                            currenthero.SetSkin(currenthero.ChampionName, ChampSkins[currenthero.Name]);
                        };
                }
            }
            catch (Exception e)
            {
                Console.Write(e + " " + e.StackTrace);
            }
            Game.OnUpdate += RenewSkins;
        }

        private static void Main()
        {
            Loading.OnLoadingComplete += GameLoad;
            Hacks.ZoomHack = true;
        }

        private static void RenewSkins(EventArgs args)
        {
            foreach (var hero in HeroList)
            {
                if (hero.IsDead && !WasDead[hero])
                {
                    WasDead[hero] = true;
                    continue;
                }
                if (!hero.IsDead && WasDead[hero] && Enabled[hero.NetworkId])
                {
                    hero.SetSkin(hero.ChampionName, ChampSkins[hero.Name]);
                    WasDead[hero] = false;
                }
            }
        }

        #endregion
    }
}