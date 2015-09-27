﻿namespace redRiven
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Combo
    {
        #region Static Fields

        private static readonly Spell.Skillshot e = Riven.E;

        private static readonly Spell.Skillshot q = Riven.Q;

        private static readonly Spell.Active r = Riven.R;

        private static readonly Spell.Skillshot r2 = Riven.R2;

        private static readonly Spell.Active w = Riven.W;

        #endregion

        #region Public Methods and Operators

        public static double CalcDmg(Obj_AI_Base target, bool useR)
        {
            if (target != null)
            {
                double dmg = 0;
                double[] passivedmg = { 0.2, 0.25, 0.3, 0.35, 0.4, 0.45, 0.5 };
                //if (Item.HasItem()) dmg = dmg + Player.GetAutoAttackDamage(target) * 0.7;
                if (w.IsReady() && GetOption(Riven.CMenu, "w"))
                {
                    dmg = dmg + Riven.Player.GetSpellDamage(target, SpellSlot.W);
                }
                if (q.IsReady())
                {
                    dmg = dmg + Riven.Player.GetSpellDamage(target, SpellSlot.Q) * (4 - Riven.QStacks)
                          + Riven.Player.GetAutoAttackDamage(target) * (4 - Riven.QStacks)
                          * (1 + passivedmg[Riven.Player.Level / 3]);
                }
                dmg = dmg + Riven.Player.GetAutoAttackDamage(target) * (1 + passivedmg[Riven.Player.Level / 3]);
                if (r2.IsReady())
                {
                    double health = 0;
                    if (Riven.CanR2())
                    {
                        health = target.Health - (dmg * 1.2);
                    }
                    else if (!Riven.CanR2())
                    {
                        health = target.Health - dmg;
                    }
                    var missinghealth = (target.MaxHealth - health) / target.MaxHealth > 0.75
                                            ? 0.75
                                            : (target.MaxHealth - health) / target.MaxHealth;
                    var pluspercent = missinghealth * (8 / 3);
                    var rawdmg = new double[] { 80, 120, 160 }[r.Level - 1] + 0.6 * Riven.Player.FlatPhysicalDamageMod;
                    return Riven.Player.CalculateDamageOnUnit(
                        target,
                        DamageType.Physical,
                        (float)(rawdmg * (1 + pluspercent)));
                }
                return dmg;
            }
            return 0;
        }

        public static void Clear()
        {
            var minion =
                EntityManager.GetLaneMinions(EntityManager.UnitTeam.Enemy, Riven.Player.Position.To2D(), w.Range)
                    .OrderByDescending(x => 1 - x.Distance(Riven.Player.Position))
                    .FirstOrDefault();
            var monster =
                EntityManager.GetJungleMonsters(Riven.Player.Position.To2D(), w.Range)
                    .OrderByDescending(x => 1 - x.Distance(Riven.Player.Position))
                    .FirstOrDefault();

            if (minion != null && Riven.Player.Distance(minion) <= w.Range && w.IsReady() && Orbwalker.CanMove
                && GetOption(Riven.WMenu, "w"))
            {
                w.Cast();
            }
            if (monster != null && Riven.Player.Distance(monster) <= w.Range && w.IsReady() && Orbwalker.CanMove
                && GetOption(Riven.JMenu, "w"))
            {
                w.Cast();
            }
            if (minion != null && Riven.Player.Distance(minion) <= w.Range && e.IsReady() && Orbwalker.CanMove
                && GetOption(Riven.WMenu, "e"))
            {
                Riven.Player.Spellbook.CastSpell(SpellSlot.E, minion.Position);
            }
            if (monster != null && Riven.Player.Distance(monster) <= w.Range && e.IsReady() && Orbwalker.CanMove
                && GetOption(Riven.JMenu, "e"))
            {
                Riven.Player.Spellbook.CastSpell(SpellSlot.E, monster.Position);
            }
        }

        public static void DoCombo(bool useR = true)
        {
            if (q.IsReady() && Orbwalker.CanMove && !Riven.Player.IsDashing()
                && (GetOption(Riven.CMenu, "q") && useR || GetOption(Riven.HMenu, "q") && !useR))
            {
                var target = RivenTarget();
                if (!Riven.Player.IsDashing() && Environment.TickCount - Riven.LastQ >= 1000 && target.IsValidTarget())
                {
                    if (Riven.Player.AttackRange + Riven.Player.Distance(Riven.Player.BBox.Minimum) + 151
                        >= Riven.Player.Distance(target.Position) && !Riven.Player.IsInAutoAttackRange(target))
                    {
                        Riven.Player.Spellbook.CastSpell(SpellSlot.Q, target.Position);
                    }
                }
            }
            if (w.IsReady() && Orbwalker.CanMove
                && (GetOption(Riven.CMenu, "w") && useR || GetOption(Riven.HMenu, "w") && !useR))
            {
                var targets =
                    HeroManager.Enemies.Where(
                        x => x.IsValidTarget() && !x.IsZombie && Riven.Player.Distance(x) <= w.Range);
                if (targets.Any() && Riven.QStacks == 0)
                {
                    w.Cast();
                }
            }
            if (e.IsReady() && Orbwalker.CanMove
                && (GetOption(Riven.CMenu, "e") && useR || GetOption(Riven.HMenu, "e") && !useR))
            {
                var target = TargetSelector.GetTarget(325 + Riven.Player.AttackRange + 70, DamageType.Physical);
                if (target.IsValidTarget() && !target.IsZombie && Riven.QStacks == 0)
                {
                    Riven.Player.Spellbook.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (r.IsReady() && useR && !Riven.CanR2() && GetOption(Riven.CMenu, "r"))
            {
                var targetR = TargetSelector.GetTarget(200 + Riven.Player.BoundingRadius + 70, DamageType.Physical);
                if (targetR.IsValidTarget() && !targetR.IsZombie && CalcDmg(targetR, false) < targetR.Health)
                {
                    r.Cast();
                }
                if (targetR.IsValidTarget() && !targetR.IsZombie
                    && Riven.Player.CountEnemiesInRange(800) >= GetOption(Riven.CMenu, "r2"))
                {
                    r.Cast();
                }
            }

            if (r2.IsReady() && useR && Riven.CanR2() && GetOption(Riven.CMenu, "r"))
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(r.Range) && !x.IsZombie && !x.IsMinion);
                foreach (var target in targets)
                {
                    if (target.Health < CalcDmg(target, true))
                    {
                        Riven.Player.Spellbook.CastSpell(SpellSlot.R, target.Position);
                    }
                    if (target.Health / target.MaxHealth <= GetOption(Riven.CMenu, "r1"))
                    {
                        Riven.Player.Spellbook.CastSpell(SpellSlot.R, target.Position);
                    }
                }
            }
        }

        public static dynamic GetOption(Menu menu, string option)
        {
            if (menu[option] is CheckBox)
            {
                return menu[option].Cast<CheckBox>().CurrentValue;
            }
            if (menu[option] is Slider)
            {
                return menu[option].Cast<Slider>().CurrentValue;
            }
            return null;
        }

        public static AIHeroClient RivenTarget()
        {
            var cursorTarget =
                HeroManager.Enemies.Where(
                    x => x.Distance(Game.CursorPos) <= 375 && x.Distance(Riven.Player.ServerPosition) <= 1200)
                    .OrderBy(x => x.Distance(Game.CursorPos))
                    .FirstOrDefault(x => x.IsEnemy);

            return cursorTarget;
        }

        public static void UseQ()
        {
            if (Riven.WaitQ)
            {
                Riven.Player.Spellbook.CastSpell(SpellSlot.Q, Game.CursorPos);
            }
            if (q.IsReady() && !Riven.Player.IsRecalling && !Riven.Player.Spellbook.IsChanneling && !Riven.Player.IsDead
                && Riven.QStacks != 0 && Environment.TickCount - Riven.LastQ >= 3650 && GetOption(Riven.MMenu, "q"))
            {
                Riven.Player.Spellbook.CastSpell(SpellSlot.Q, Game.CursorPos);
            }
            else
            {
                Riven.QStacks = 0;
            }
        }

        #endregion
    }
}