using Settings = HeavyGragas.Config.Modes.Insec;

namespace HeavyGragas.Modes
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    public sealed class Insec : ModeBase
    {
        private static Vector3 barrelPos;

        private bool shouldFlash;

        private bool isDashingE
        {
            get
            {
                return Player.Instance.HasBuff("GragasE");
            }
        }

        private AIHeroClient Target
        {
            get
            {
                return TargetSelector.SelectedTarget;
            }
        }

        public Insec()
        {
            Obj_AI_Base.OnSpellCast += OnSpellCast;
            Obj_AI_Base.OnBuffLose += OnBuffLose;
        }

        private void OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (sender.IsMe && ShouldBeExecuted() && args.Buff.Name == "GragasE" && QBarrel != null)
            {
                CastR();
            }
        }

        public void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && ShouldBeExecuted())
            {
                switch (args.Slot)
                {
                    case SpellSlot.E:
                        if (Target.IsValidTarget(E.Range))
                        {
                            //CastQ1(barrelPos);
                        }
                        else
                        {
                            shouldFlash = true;
                        }
                        break;
                    case SpellSlot.R:
                        break;
                }
            }
        }


        public override bool ShouldBeExecuted()
        {
            return Settings.UseInsec && Target != null;
        }
        

        public override void Execute()
        {
            Orbwalker.DisableAttacking = true;

            var targetPos = Target.Position;
            var nearAllyPos =
                CircleCircleIntersection(Player.Instance.Position.To2D(), targetPos.To2D(), Q.Range, 600)
                    .OrderBy(x => x.DistanceToNearestAlly())
                    .FirstOrDefault()
                    .To3D();

            if (Target.IsValidTarget(Flash.Range + 50) && Flash.IsReady() && isDashingE && QBarrel != null)
            {
                shouldFlash = false;
                Player.Instance.Spellbook.CastSpell(Flash.Slot, Target.Position);
            }

            if ((nearAllyPos.CountAlliesInRange(700) > 0 || nearAllyPos.IsUnderAllyTurret()) && nearAllyPos.IsValid(true))
            {
                barrelPos = nearAllyPos;
            }
            else
            {
                barrelPos = targetPos.Extend(Player.Instance.Position, 600).To3D();
            }

            while (barrelPos.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall) || barrelPos.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Building))
            {
                barrelPos = barrelPos.Extend(Target.Position, 50).To3D();
            }

            if (Target.HasBuffOfType(BuffType.Stun) || Target.HasBuffOfType(BuffType.Knockback))// && Player.Instance.IsInAutoAttackRange(Target))
            {
                CastR();
            }
            else if (Target.IsValidTarget(E.Range + Flash.Range) && E.IsReady())
            {
                if (Target.IsValidTarget(E.Range))
                {
                    if (QBarrel == null)
                    {
                        if (CastE(Target) && Target.IsValidTarget(E.Range))
                        {
                            CastQ1(barrelPos);
                        }
                    }
                    else if(Game.Time - LastQThrow < 1)
                    {
                        CastE(Target);
                        CastR();
                    }
                }
                else if (Flash.IsReady() && E.GetPrediction(Target).CollisionObjects.Length == 0)
                {
                    if (QBarrel != null && QBarrel.Distance(Target) < 600)
                    {
                        Player.Instance.Spellbook.CastSpell(SpellSlot.E, Target.Position);
                        shouldFlash = true;
                        Core.DelayAction(() => { shouldFlash = false; }, 2000);
                    }
                }
            }
            Orbwalker.DisableAttacking = false;
        }

        private void CastR()
        {
            if (EventManager.QBarrel != null)
            {
                var targetPos = Target.Position;//.Extend(Player.Instance.Position, -50);

                var castPos = barrelPos.Extend(targetPos, targetPos.Distance(QBarrel) + 200).To3D();
                //var castDistance = castPos.Distance(barrelPos);
                //var targetDistance = Target.Distance(barrelPos);

                if (R.IsReady() && targetPos.Distance(QBarrel).IsBetween(610, 870) &&
                    !targetPos.CheckWall(targetPos.Extend(castPos, 550).To3DWorld()) &&
                    //castDistance > targetDistance &&
                    castPos.CountEnemiesInRange(400) > 0)
                {
                    R.Cast(castPos);
                }
            }
        }

        //From B$.Common
        public static Vector2[] CircleCircleIntersection(Vector2 center1, Vector2 center2, float radius1, float radius2)
        {
            var distance = center1.Distance(center2);

            if (distance > radius1 + radius2 || (distance <= Math.Abs(radius1 - radius2)))
            {
                return new Vector2[] { };
            }

            var a = (radius1 * radius1 - radius2 * radius2 + distance * distance) / (2 * distance);
            var h = (float)Math.Sqrt(radius1 * radius1 - a * a);
            var direction = (center2 - center1).Normalized();
            var pa = center1 + a * direction;
            var intersection1 = pa + h * direction.Perpendicular();
            var intersection2 = pa - h * direction.Perpendicular();
            return new[] { intersection1, intersection2 };
        }
    }
}