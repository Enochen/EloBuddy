using System;
using EloBuddy;
using EloBuddy.SDK;

namespace dRiven
{
    class MOrbwalker
    {
        public static void Init()
        {
            Orbwalker.DisableAttacking = true;
            Orbwalker.DisableMovement = true;

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
                Orbwalk();
            }
            else
            {
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;

            }
        }

        private static long lastAaTick;

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                lastAaTick = Environment.TickCount;
            }
        }

        public static bool CanAttack()
        {
            if (lastAaTick <= Environment.TickCount)
                return Environment.TickCount + Game.Ping / 2 + 25 >= lastAaTick + ObjectManager.Player.AttackDelay * 1000;
            return false;
        }

        public static bool CanMove()
        {
            if (lastAaTick <= Environment.TickCount)
                return Environment.TickCount + Game.Ping / 2 >= lastAaTick + ObjectManager.Player.AttackCastDelay * 1000 + 25;
            return false;
        }

        public static void Orbwalk()
        {
            if (CanAttack())
            {
                var target = TargetSelector.GetTarget(ObjectManager.Player.GetAutoAttackRange(), DamageType.Physical);
                if (target != null)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    lastAaTick = Environment.TickCount;
                    return;
                }
            }
            if (CanMove())
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
        }
    }
}