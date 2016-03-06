namespace HeavyGragas
{
    using EloBuddy;
    using EloBuddy.SDK;

    public static class SpellDamage
    {
        public static float GetTotalDamage(Obj_AI_Base target)
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

            return damage;
        }

        public static float GetRealDamage(this Spell.SpellBase spell, Obj_AI_Base target)
        {
            return spell.Slot.GetRealDamage(target);
        }

        public static float GetRealDamage(this SpellSlot slot, Obj_AI_Base target)
        {
            var spellLevel = Player.Instance.Spellbook.GetSpell(slot).Level;
            const DamageType DamageType = DamageType.Magical;
            double damage = 0;

            if (spellLevel == 0)
            {
                return 0;
            }

            switch (slot)
            {
                case SpellSlot.Q:
                    damage = 40 + 40 * spellLevel + .60 * Player.Instance.TotalMagicalDamage;
                    if (target.IsMinion) damage *= .7;
                    if (EventManager.QBarrel != null) damage *= (1 + (Game.Time - EventManager.BarrelTime) * .25).Limit(1.5);
                    break;

                case SpellSlot.W:
                    damage = -10 + 30 * spellLevel + .30 * Player.Instance.TotalMagicalDamage
                             + .08 * target.MaxHealth;
                    break;

                case SpellSlot.E:
                    damage = 30 + 50 * spellLevel + .60 * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.R:
                    damage = 100 + 100 * spellLevel + .70 * Player.Instance.TotalMagicalDamage;
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