namespace HeavyGragas
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }

        public static Spell.Active Q2 { get; private set; }

        public static Spell.Active W { get; private set; }

        public static Spell.Skillshot E { get; private set; }

        public static Spell.Skillshot R { get; private set; }

        public static Spell.Skillshot Flash { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Circular, 250, 900, 275);
            Q2 = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 600, SkillShotType.Linear, 0, 900, 200) { AllowedCollisionCount = 0 };
            R = new Spell.Skillshot(SpellSlot.R, 1150, SkillShotType.Circular, 250, 1800, 400);
            var slot = Player.Instance.GetSpellSlotFromName("SummonerFlash");
            if (slot != SpellSlot.Unknown)
            {
                Flash = new Spell.Skillshot(slot, 400, SkillShotType.Linear) { AllowedCollisionCount = int.MaxValue, CastDelay = 0};
            }
        }

        public static void Initialize()
        {
        }
    }
}