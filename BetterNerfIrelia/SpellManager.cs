using EloBuddy;
using EloBuddy.SDK;

namespace BetterNerfIrelia
{
    using System;

    using EloBuddy.SDK.Enumerations;

    public static class SpellManager
    {
        public static Spell.Targeted Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Skillshot R { get; private set; }
        public static Spell.Targeted Ignite { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 650);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 425);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Linear, 0, 1600, 65);
            Console.WriteLine(Player.Instance.GetSpellSlotFromName("summonerdot"));
            var fireSlot = Player.Instance.GetSpellSlotFromName("summonerdot");
            if (fireSlot != SpellSlot.Unknown)
            {
                Ignite = new Spell.Targeted(fireSlot, 600);
            }
        }

        public static void Initialize()
        {
        }
    }
}
