namespace BetterNerfIrelia
{
    using EloBuddy;
    using EloBuddy.SDK;

    public static class SpellDamage
    {
        public static float GetTotalDamage(AIHeroClient target, bool ks)
        {
            // Auto attack
            var damage = Player.Instance.GetAutoAttackDamage(target) * 3;
            if (ks)
            {
                damage = 0;
            }

            // Q
            if (SpellManager.Q.IsReady())
            {
                damage += SpellManager.Q.GetRealDamage(target);
            }
            // W
            if (SpellManager.W.IsReady())
            {
                damage += SpellManager.W.GetRealDamage(target) * 3;
                if (ks)
                {
                    damage -= SpellManager.W.GetRealDamage(target) * 2;
                }
            }
            // E
            if (SpellManager.E.IsReady())
            {
                damage += SpellManager.E.GetRealDamage(target);
            }
            // R
            if (SpellManager.R.IsReady())
            {
                var stacks = Player.Instance.GetBuffCount("ireliatranscendentbladesspell");
                if (ks && Player.HasBuff("ireliatranscendentbladesspell"))
                {
                    damage += SpellManager.R.GetRealDamage(target);
                }
                else
                {
                    damage += SpellManager.R.GetRealDamage(target) * (stacks == 0 ? 4 : stacks);
                }
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
            var damageType = DamageType.Magical;
            float damage = 0;

            // Validate spell level
            if (spellLevel == 0)
            {
                return 0;
            }
            //spellLevel--;

            switch (slot)
            {
                case SpellSlot.Q:
                    damageType = DamageType.Physical;
                    damage = -10 + (30 * spellLevel) + (Player.Instance.TotalAttackDamage);
                    break;

                case SpellSlot.W:
                    damageType = DamageType.True;
                    damage = (15 * spellLevel);
                    break;

                case SpellSlot.E:
                    damageType = DamageType.Magical;
                    damage = 40 + (40 * spellLevel) + (0.5f * Player.Instance.TotalMagicalDamage);
                    break;

                case SpellSlot.R:
                    damageType = DamageType.Physical;
                    damage = 40 + (40 * spellLevel) + (0.5f * Player.Instance.TotalMagicalDamage)
                             + (0.6f * Player.Instance.TotalAttackDamage);
                    break;

                default:
                    if (slot == SpellManager.Ignite.Slot)
                    {
                        damage = 50 + 20 * Player.Instance.Level;
                    }
                    break;
            }

            if (damage <= 0)
            {
                return 0;
            }

            return Player.Instance.CalculateDamageOnUnit(target, damageType, damage) - 10;
        }
    }
}