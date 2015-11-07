namespace SpaceAIDS
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Targeted R { get; private set; }
        public static Spell.Targeted Fire { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 500, int.MaxValue, 100)
            {MinimumHitChance = HitChance.High};
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, 500, int.MaxValue, 250);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Targeted(SpellSlot.R, 700);
            var ignite = Player.Spells.FirstOrDefault(x => x.SData.Name == "summonerdot");
            if (ignite != null)
            {
                Fire = new Spell.Targeted(ignite.Slot, 600);
            }
            else
            {
                Fire = null;
            }
        }

        public static void Initialize()
        {
        }
    }
}
