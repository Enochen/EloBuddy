namespace SpaceAIDS
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;

    public static class SpellDamage
    {
        public static float GetTotalDamage(AIHeroClient target)
        {
            var damage = Player.Instance.GetAutoAttackDamage(target);

            if (SpellManager.Q.IsReady())
            {
                damage += SpellManager.Q.GetRealDamage(target);
            }

            if (SpellManager.W.IsReady())
            {
                damage += SpellManager.W.GetRealDamage(target);
            }

            if (SpellManager.E.IsReady())
            {
                damage += SpellManager.E.GetRealDamage(target);
            }

            if (SpellManager.R.IsReady())
            {
                damage += SpellManager.R.GetRealDamage(target);
            }

            if ((SpellManager.Fire != null) && SpellManager.Fire.IsReady())
            {
                damage += SpellManager.Fire.GetRealDamage(target);
            }
            return damage;
        }

        public static float GetRealDamage(this Spell.SpellBase spell, Obj_AI_Base target)
        {
            return spell.Slot.GetRealDamage(target);
        }

        public static float GetRealDamage(this SpellSlot slot, Obj_AI_Base target)
        {
            var spellLevel = Player.Instance.Spellbook.GetSpell(slot).Level;
            var playerLevel = Player.Instance.Level;
            const DamageType DamageType = DamageType.Magical;
            float damage = 0;

            if (spellLevel == 0)
            {
                return 0;
            }
            spellLevel--;

            switch (slot)
            {
                case SpellSlot.Q:
                    damage = (spellLevel * 55) + 25 + 0.8f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.W:
                    damage = (float)((3 + spellLevel + Math.Floor(Player.Instance.TotalMagicalDamage/100)) * 0.01f * target.MaxHealth * 2);
                    break;

                case SpellSlot.E:
                    damage = (spellLevel * 60) + 20 + 0.8f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.R:
                    damage = (spellLevel * 150) + 100 + 1.3f * Player.Instance.TotalMagicalDamage;
                    break;

                default:
                    if (slot == SpellManager.Fire.Slot)
                    {
                        damage = (20 * playerLevel) + 50;
                    }
                    break;
            }

            if (damage <= 0)
            {
                return 0;
            }

            return Player.Instance.CalculateDamageOnUnit(target, DamageType, damage) - 10;
        }
    }
}