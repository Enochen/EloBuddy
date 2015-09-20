namespace SkinsPlus
{
    using System;
    using System.Collections.Generic;

    using EloBuddy;

    internal class Re
    {
        #region Static Fields

        public static readonly Dictionary<Obj_AI_Base, bool> Dead = new Dictionary<Obj_AI_Base, bool>();

        #endregion

        #region Public Methods and Operators

        public static void New(EventArgs args)
        {
            foreach (var hero in Setup.Heroes)
            {
                if (hero.IsDead && !Dead[hero])
                {
                    Dead[hero] = true;
                    continue;
                }
                if (hero.IsDead || !Dead[hero] || !Setup.ChampEnabled[hero.NetworkId])
                {
                    continue;
                }
                hero.SetSkin(hero.ChampionName, Setup.ChampSkins[hero.Name]);
                Dead[hero] = false;
            }
        }

        #endregion
    }
}