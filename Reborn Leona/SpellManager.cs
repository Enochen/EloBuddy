namespace Reborn_Leona
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }

        public static Spell.Active W { get; private set; }

        public static Spell.Skillshot E { get; private set; }

        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 905, SkillShotType.Linear, 250, 2000, 70);
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Circular, 1000, int.MaxValue, 300);
        }

        public static void Initialize()
        {
        }
    }
}