namespace SpaceAIDS
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public static class Extensions
    {
        public static bool HasUndyingBuff(this AIHeroClient target)
        {
            if (
                target.Buffs.Any(
                    b =>
                    b.IsValid()
                    && (b.DisplayName == "Chrono Shift" || b.DisplayName == "JudicatorIntervention" /* Kayle R */
                        || b.DisplayName == "Undying Rage")))
            {
                return true;
            }

            if (target.ChampionName == "Poppy")
            {
                if (
                    EntityManager.Heroes.Allies.Any(
                        o =>
                        !o.IsMe
                        && o.Buffs.Any(
                            b =>
                            b.Caster.NetworkId == target.NetworkId && b.IsValid() && b.DisplayName == "PoppyDITarget")))
                {
                    return true;
                }
            }

            return target.IsInvulnerable;
        }

        public static bool HasSpellShield(this AIHeroClient target)
        {
            return target.HasBuffOfType(BuffType.SpellShield) || target.HasBuffOfType(BuffType.SpellImmunity);
        }

        public static float TotalShieldHealth(this Obj_AI_Base target)
        {
            return target.Health + target.AllShield + target.AttackShield + target.MagicShield;
        }

        public static int GetStunDuration(this Obj_AI_Base target)
        {
            return
                (int)
                (target.Buffs.Where(
                    b =>
                    b.IsActive && Game.Time < b.EndTime
                    && (b.Type == BuffType.Charm || b.Type == BuffType.Knockback || b.Type == BuffType.Stun
                        || b.Type == BuffType.Suppression || b.Type == BuffType.Snare))
                     .Aggregate(0f, (current, buff) => Math.Max(current, buff.EndTime)) - Game.Time) * 1000;
        }
    }
}