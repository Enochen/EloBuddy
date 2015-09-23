namespace dRiven
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    internal class StateManager
    {
        #region Properties

        private static Item RavenousHydra
        {
            get
            {
                return new Item((int)ItemId.Ravenous_Hydra_Melee_Only);
            }
        }

        private static Item Tiamat
        {
            get
            {
                return new Item((int)ItemId.Tiamat_Melee_Only);
            }
        }

        private static Item YoumuusGhostBlade
        {
            get
            {
                return new Item((int)ItemId.Youmuus_Ghostblade);
            }
        }

        #endregion

        #region Public Methods and Operators

        public static bool CanHardEngage(AIHeroClient target)
        {
            var dmg = GetDamage(target);
            return dmg * 2 > target.Health;
        }

        public static void CastW()
        {
            if (HeroManager.Enemies.Any(x => x.IsValidTarget(Program.W.Range)))
            {
                Chat.Say("enemies");
                Program.W.Cast();
            }
        }

        public static void Cleave()
        {
            var cleaveMinions = Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.Combo;

            if (cleaveMinions)
            {
                if (!ObjectManager.Get<Obj_AI_Minion>().Any(x => x.IsValidTarget(RavenousHydra.Range)))
                {
                    return;
                }
                Chat.Say("yep");
                RavenousHydra.Cast();
                Tiamat.Cast();
            }
            else
            {
                if (!HeroManager.Enemies.Any(x => x.IsValidTarget(RavenousHydra.Range)))
                {
                    return;
                }
                RavenousHydra.Cast();
                Tiamat.Cast();
            }
        }

        public static float GetDamage(AIHeroClient hero)
        {
            var dmg = 0f;

            if (Program.Q.IsReady())
            {
                dmg += Player.Instance.GetSpellDamage(hero, SpellSlot.Q) * 3 - Program.QCount;
                dmg += Program.PassiveDmg * 3 - Program.QCount;
                dmg += Program.Player.GetAutoAttackDamage(hero, true) * 3;
            }

            if (Program.E.IsReady())
            {
                dmg += Program.PassiveDmg;
            }

            if (Program.W.IsReady())
            {
                dmg += Player.Instance.GetSpellDamage(hero, SpellSlot.W);
                dmg += Program.PassiveDmg;
            }

            if (RavenousHydra.IsReady())
            {
                dmg += Program.Player.GetItemDamage(hero, ItemId.Ravenous_Hydra_Melee_Only);
            }

            if (Tiamat.IsReady())
            {
                dmg += Program.Player.GetItemDamage(hero, ItemId.Tiamat_Melee_Only);
            }

            dmg += Program.Player.GetAutoAttackDamage(hero, true);

            return dmg;
        }

        public static void OnGameUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.Combo)
            {
            }
            if (Program.HasSpell("summonerdot"))
            {
                if (Program.Fire.IsReady())
                {
                    FireKs();
                }
            }

            if (Environment.TickCount - Program.LastQ >= 3650 && Program.QCount != 0 && !Program.Player.IsRecalling())
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Q, Game.CursorPos);
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Combo(false);
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                WaveClear();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
        }

        public static bool UseSkill(string skill)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    return (Program.GetBool(Program.cMenu, skill + ".c"));
                case Orbwalker.ActiveModes.LaneClear:
                    if (skill == "r")
                    {
                        return false;
                    }
                    return (Program.GetBool(Program.fMenu, skill + ".f"));
                case Orbwalker.ActiveModes.Harass:
                    if (skill == "r")
                    {
                        return false;
                    }
                    return (Program.GetBool(Program.hMenu, skill + ".h"));
            }
            return false;
        }

        #endregion

        #region Methods

        private static void Combo(bool useR = true)
        {
            var target = TargetSelector.GetTarget(Program.EwRange, DamageType.Physical);

            if (!target.IsValidTarget())
            {
                return;
            }

            Program.LastTarget = target;

            if (YoumuusGhostBlade.IsReady())
            {
                YoumuusGhostBlade.Cast();
            }

            if (Program.RActivated && Program.CanU2 && useR)
            {
                UseU2(target);
            }

            if (CanHardEngage(target) && !Program.RActivated && Program.R.IsReady() && target.HealthPercent > 1 && useR
                && UseSkill("r"))
            {
                if (Program.E.IsReady())
                {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E, target.ServerPosition);
                    Program.R.Cast();
                }
                else if (Program.QCount == 2 && Program.Q.IsReady() && Program.E.IsReady())
                {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E, target.ServerPosition);
                    Program.R2.Cast();
                }
                else if (Program.W.IsReady() && HeroManager.Enemies.Any(x => x.IsValidTarget(Program.W.Range)))
                {
                    Program.R.Cast();
                    Program.W.Cast();
                }
                else
                {
                    Program.R.Cast();
                }
            }

            if (target.Distance(Program.Player) < Program.EwRange
                && (!Program.Q.IsReady() || target.Distance(Program.Player) > Program.Q.Range) && Program.E.IsReady()
                && Program.W.IsReady() && UseSkill("e") && UseSkill("w"))
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E, target.Position);
                Cleave();
                CastW();
            }
            else if ((RavenousHydra.IsReady() || Tiamat.IsReady()) && Program.W.IsReady() && UseSkill("w"))
            {
                Cleave();
                CastW();
            }
            else if (Program.W.IsReady())
            {
                Program.W.Cast();
            }
            else if (Program.E.IsReady())
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E, target.Position);
            }
            else if (Program.Q.IsReady() && Environment.TickCount - Program.LastQ >= 2000
                     && Program.Player.Distance(target) < Program.Q.Range)
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Q, target.Position);
            }
        }

        private static void FireKs()
        {
            var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Program.Fire.Range));
            var enemies = targets as AIHeroClient[] ?? targets.ToArray();
            var target =
                enemies.Where(
                    x => Program.Player.GetSummonerSpellDamage(x, DamageLibrary.SummonerSpells.Ignite) > x.Health)
                    .OrderByDescending(x => x.Distance(Program.Player))
                    .FirstOrDefault();

            if (target != null)
            {
                Program.Fire.Cast(target);
            }
        }

        private static void Flee()
        {
            if (YoumuusGhostBlade.IsReady())
            {
                YoumuusGhostBlade.Cast();
            }

            if (Program.E.IsReady() && Program.QCount == 0)
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E, Game.CursorPos);
            }

            if (Program.Q.IsReady())
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Q, Game.CursorPos);
            }
        }

        private static double GetCircleThingDamage(Obj_AI_Base target)
        {
            if (RavenousHydra.IsReady())
            {
                return Program.Player.GetItemDamage(target, ItemId.Ravenous_Hydra_Melee_Only);
            }
            if (Tiamat.IsReady())
            {
                return Program.Player.GetItemDamage(target, ItemId.Tiamat_Melee_Only);
            }

            return 0;
        }

        private static void UseU2(Obj_AI_Base target)
        {
            if (Program.W.IsReady()
                && HeroManager.Enemies.Any(
                    x =>
                    x.IsValidTarget(Program.W.Range)
                    && Program.Player.GetSpellDamage(target, SpellSlot.W)
                    + Program.Player.GetSpellDamage(target, SpellSlot.R) > target.Health))
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.R, target.Position);
                Program.W.Cast();
            }
            else if (Program.QCount == 2 && Program.Q.IsReady() && Program.E.IsReady()
                     && Program.Player.GetSpellDamage(target, SpellSlot.Q)
                     + Program.Player.GetSpellDamage(target, SpellSlot.R) + Program.PassiveDmg
                     + Program.Player.GetAutoAttackDamage(target) > target.Health)
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E, target.Position);
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.R, target.Position);
            }
            else if (GetCircleThingDamage(target) + Program.Player.GetSpellDamage(target, SpellSlot.R)
                     > target.Health)
            {
                Cleave();
                Core.DelayAction(() => Program.R2.Cast(target.Position), 100);
            }
            else if (Program.Fire.IsReady()
                     && Program.Player.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite)
                     + Program.Player.GetSpellDamage(target, SpellSlot.R) > target.Health
                     && Program.Player.Distance(target) > Program.Player.AttackRange)
            {
                Program.Fire.Cast(target);
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.R, target.Position);
            }
            else if (Program.Player.GetSpellDamage(target, SpellSlot.R) > target.Health)
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.R, target.Position);
            }
        }

        private static void WaveClear()
        {
            var minion =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(a => a.IsEnemy && a.Distance(Player.Instance) < Program.W.Range)
                    .OrderBy(a => a.Health)
                    .FirstOrDefault();
            if (minion != null)
            {
                Program.LastTarget = minion;

                if (Program.E.IsReady() && Program.W.IsReady() && Program.GetBool(Program.fMenu, "e.f")
                    && Program.GetBool(Program.fMenu, "w.f") && minion.Distance(Player.Instance) < Program.EwRange)
                {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E, minion.Position);
                    Cleave();
                    Program.W.Cast();
                }
                else if (Program.W.IsReady() && minion.Distance(Player.Instance) < Program.W.Range)
                {
                    Cleave();
                    Program.W.Cast();
                }
            }
        }

        #endregion
    }
}