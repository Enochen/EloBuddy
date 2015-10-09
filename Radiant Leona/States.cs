namespace Leona
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    internal class States
    {
        private static readonly Spell.Active Q = Leona.Q;

        private static readonly Spell.Active W = Leona.W;

        private static readonly Spell.Skillshot E = Leona.E;

        private static readonly Spell.Skillshot R = Leona.R;

        private static readonly Spell.Skillshot R2 = Leona.R2;

        private static readonly AIHeroClient Player = ObjectManager.Player;

        public static void DoCombo(bool useQ, bool useW, bool useE, bool useR, int rEnemies)
        {
            var target = TargetSelector.GetTarget(Leona.E.Range, DamageType.Magical);

            if (target == null || !target.IsValidTarget() || target.IsZombie)
            {
                return;
            }

            if (useQ && Q.IsReady() && Player.IsInAutoAttackRange(target))
            {
                Q.Cast();
            }

            if (R.IsReady() && useR
                && (target.CountEnemiesInRange(R.Radius) >= rEnemies || Player.CountEnemiesInRange(R.Range) >= rEnemies)
                && (R.GetPrediction(target).HitChance > HitChance.High
                    || R2.GetPrediction(target).HitChance > HitChance.Medium))
            {
                R.Cast(target);
            }

            if (W.IsReady() && useW && Player.CountEnemiesInRange(W.Range) > 0)
            {
                W.Cast();
            }
            
            if (E.IsReady() && useE)
            {
                E.Cast(target);
                W.Cast();
                
            }
        }
    }
}