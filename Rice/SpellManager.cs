using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Rice
{
    using System;

    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Targeted W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 100)
                    { AllowedCollisionCount = int.MaxValue };
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R);
        }

        public static void Initialize()
        {
        }
    }

}
