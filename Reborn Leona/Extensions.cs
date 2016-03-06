namespace Reborn_Leona
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    public static class Extensions
    {
        public static bool HasUndyingBuff(this AIHeroClient target)
        {
            // Various buffs
            if (
                target.Buffs.Any(
                    b =>
                    b.IsValid()
                    && (b.DisplayName == "Chrono Shift" /* Zilean R */|| b.DisplayName == "JudicatorIntervention"
                        /* Kayle R */|| b.DisplayName == "Undying Rage" /* Tryndamere R */)))
            {
                return true;
            }

            // Poppy R
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
            // Various spellshields
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

        public static KeyValuePair<Vector2, int> GetBestRPos(this Vector2 targetPosition)
        {
            var rPos = new List<Vector2>();
            for (var i = 0; i < SpellManager.R.Range; i += 100)
            {
                for (var j = 0; j < SpellManager.R.Range; j += 100)
                {
                    rPos.Add(new Vector2(targetPosition.X + i, targetPosition.Y + j));
                }
            }

            var posHits = rPos.ToDictionary(pos => pos, GetRHits);

            var bestPos = posHits.First(pos => pos.Value == posHits.Values.Max()).Key;
            var hits = posHits.First(pos => pos.Key == bestPos).Value;

            return new Dictionary<Vector2, int> { { bestPos, hits } }.First();
        }

        public static KeyValuePair<Vector2, int> GetBestRPos(this Vector2 targetPosition, GameObject targetEnemy)
        {
            var rPos = new List<Vector2>();
            for (var i = 0; i < SpellManager.R.Range; i += 100)
            {
                for (var j = 0; j < SpellManager.R.Range; j += 100)
                {
                    rPos.Add(new Vector2(targetPosition.X + i, targetPosition.Y + j));
                }
            }

            var posHits = rPos.ToDictionary(pos => pos, GetRHits);

            var bestPos = posHits.First(pos => pos.Value == posHits.Values.Max()).Key;
            var hits = posHits.First(pos => pos.Key == bestPos).Value;

            return new Dictionary<Vector2, int> { { bestPos, hits } }.First();
        }

        public static int GetRHits(this Vector2 castPosition)
        {
            var positions =
                EntityManager.Heroes.Enemies.Where(hero => !hero.IsDead && hero.IsValidTarget(SpellManager.R.Range))
                    .Select(hero => Prediction.Position.PredictUnitPosition(hero, 1000).To3D())
                    .ToList();

            return positions.Count(enemyPos => castPosition.Distance(enemyPos) <= SpellManager.R.Radius);
        }

        public static int GetRHits(this Vector2 castPosition, GameObject targetEnemy)
        {
            var positions =
                EntityManager.Heroes.Enemies.Where(hero => !hero.IsDead && hero.IsValidTarget(SpellManager.R.Range))
                    .Select(hero => Prediction.Position.PredictUnitPosition(hero, 1000).To3D())
                    .ToList();

            return targetEnemy.Distance(castPosition) > SpellManager.R.Range
                       ? 0
                       : positions.Count(enemyPos => castPosition.Distance(enemyPos) <= SpellManager.R.Radius);
        }
    }
}