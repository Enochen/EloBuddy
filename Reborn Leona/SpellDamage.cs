using EloBuddy;
using EloBuddy.SDK;

namespace Reborn_Leona
{
    public static class SpellDamage
    {
        public static float GetTotalDamage(Obj_AI_Base target)
        {
            // Auto attack
            var damage = Player.Instance.GetAutoAttackDamage(target);

            // Q
            if (SpellManager.Q.IsReady())
            {
                damage += SpellManager.Q.GetRealDamage(target);
            }

            // W
            if (SpellManager.W.IsReady())
            {
                damage += SpellManager.W.GetRealDamage(target);
            }

            // E
            if (SpellManager.E.IsReady())
            {
                damage += SpellManager.E.GetRealDamage(target);
            }

            //R
            if (SpellManager.R.IsReady())
            {
                damage += SpellManager.R.GetRealDamage(target);
            }

            return damage;
        }

        public static float GetRealDamage(this Spell.SpellBase spell, Obj_AI_Base target)
        {
            return spell.Slot.GetRealDamage(target);
        }

        public static float GetRealDamage(this SpellSlot slot, Obj_AI_Base target)
        {
            // Helpers
            var spellLevel = Player.Instance.Spellbook.GetSpell(slot).Level;
            const DamageType DamageType = DamageType.Magical;
            double damage = 0;

            // Validate spell level
            if (spellLevel == 0)
            {
                return 0;
            }

            switch (slot)
            {
                case SpellSlot.Q:

                    damage = (10 + (30 * spellLevel)) + (.30 * Player.Instance.TotalMagicalDamage);
                    break;

                case SpellSlot.W:

                    damage = (10 + (50 * spellLevel)) + (.40 * Player.Instance.TotalMagicalDamage);
                    break;

                case SpellSlot.E:

                    damage = (20 + (40 * spellLevel)) + (.40 * Player.Instance.TotalMagicalDamage);
                    break;

                case SpellSlot.R:

                    damage = (50 + (100 * spellLevel)) + (.80 * Player.Instance.TotalMagicalDamage);
                    break;
            }

            if (damage <= 0)
            {
                return 0;
            }

            return Player.Instance.CalculateDamageOnUnit(target, DamageType, (float)damage) - 10;
        }
    }
}