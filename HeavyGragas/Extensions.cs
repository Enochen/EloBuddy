namespace HeavyGragas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    using Color = System.Drawing.Color;

    public static class Extensions
    {
        public static bool HasUndyingBuff(this AIHeroClient target)
        {
            if (
                target.Buffs.Any(
                    b =>
                    b.IsValid()
                    && (b.DisplayName == "Chrono Shift" /* Zilean R */|| b.DisplayName == "JudicatorIntervention"
                        /* Kayle R */|| b.DisplayName == "Undying Rage" /* Tryndamere R */)))
            {
                return true;
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

        public static int SpellTime(this Spell.Skillshot spell, Vector3 pos)
        {
            var distance = Player.Instance.Distance(pos);
            var delay = spell.CastDelay;
            var speed = spell.Speed;

            return (int)(distance / speed * 1000 + delay);
        }

        public static bool IsFacing(this Obj_AI_Base unit, Vector2 pos)
        {
            return unit.Direction.To2D().Perpendicular().AngleBetween(pos - unit.Position.To2D()) < 90;
        }

        public static float DistanceToNearestAlly(this Vector2 position)
        {
            var nearestAlly = EntityManager.Heroes.Allies.OrderBy(x => x.Distance(position)).FirstOrDefault();
            return position.Distance(nearestAlly);
        }

        public static double Limit(this double value, double max)
        {
            return value >= max ? max : value;
        }

        //Modified from http://stackoverflow.com/a/5023279/3224382
        public static bool IsBetween<T>(this T item, T start, T end) where T : IComparable, IComparable<T>
        {
            return Comparer<T>.Default.Compare(item, start) >= 0 && Comparer<T>.Default.Compare(item, end) <= 0;
        }

        public static bool IsUnderAllyTurret(this Vector3 position)
        {
            return EntityManager.Turrets.Allies.Any(turret => turret.IsInRange(position, 900) && !turret.IsDead);
        }

        public static bool CheckWall(this Vector3 start, Vector3 end)
        {
            for (var i = 0; i < start.Distance(end); i += 10)
            {
                new Circle { Color = Color.Green, BorderWidth = Config.Modes.Draw.WidthR, Radius = 1 }.Draw(
                    start.Extend(end, i).To3D());
                var posCollision = start.Extend(end, i).ToNavMeshCell().CollFlags;
                if (posCollision.HasFlag(CollisionFlags.Wall) || posCollision.HasFlag(CollisionFlags.Building))
                {
                    Chat.Print("kek");
                    return true;
                }
            }
            return false;
        }
    }
}