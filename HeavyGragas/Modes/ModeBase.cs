namespace HeavyGragas.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    using SharpDX;

    public abstract class ModeBase
    {
        protected float LastQThrow;

        protected Spell.Skillshot Q
        {
            get
            {
                return SpellManager.Q;
            }
        }

        protected Spell.Active Q2
        {
            get
            {
                return SpellManager.Q2;
            }
        }

        protected Spell.Active W
        {
            get
            {
                return SpellManager.W;
            }
        }

        protected Spell.Skillshot E
        {
            get
            {
                return SpellManager.E;
            }
        }

        protected Spell.Skillshot R
        {
            get
            {
                return SpellManager.R;
            }
        }

        protected Spell.Skillshot Flash
        {
            get
            {
                return SpellManager.Flash;
            }
        }

        protected GameObject QBarrel
        {
            get
            {
                return EventManager.QBarrel;
            }
        }

        protected void CastQ1(Vector3 pos)
        {
            if (EventManager.QBarrel == null && Game.Time - LastQThrow > 1)
            {
                LastQThrow = Game.Time;
                Q.Cast(pos);
            }
        }

        protected bool CastE(Obj_AI_Base target)
        {
            var collision = E.GetPrediction(target).CollisionObjects;
            if (collision != null && collision.Length == 1)
            {
                var distance1 = target.Distance(Player.Instance);
                var distance2 =
                    E.GetPrediction(target)
                        .CollisionObjects.OrderBy(x => x.Distance(Player.Instance))
                        .FirstOrDefault()
                        .Distance(Player.Instance);

                if (distance1 - distance2 > 50)
                {
                    return false;
                }
                E.Cast(E.GetPrediction(target).CastPosition);
            }
            else if (E.GetPrediction(target).HitChance >= HitChance.High)
            {
                E.Cast(target);
            }
            else
            {
                return false;
            }
            Orbwalker.ResetAutoAttack();
            Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            return true;
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}