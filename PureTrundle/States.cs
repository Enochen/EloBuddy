namespace PureTrundle
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;

    internal class States
    {
        public static bool UseQwc;

        public static bool UseQjf;

        public const int Botrk = (int)ItemId.Blade_of_the_Ruined_King;

        public const int Cutlass = (int)ItemId.Bilgewater_Cutlass;

        public const int Hydra = (int)ItemId.Ravenous_Hydra_Melee_Only;

        public const int Tiamat = (int)ItemId.Tiamat_Melee_Only;

        public static dynamic CalcDmg(Obj_AI_Base target, bool qaa)
        {
            double result = 0;
            double[] rDmg = { 0, .20, .275, .35 };
            var rawDmg = target.MaxHealth * rDmg[Trundle.Player.Spellbook.GetSpell(SpellSlot.R).Level];

            result += Trundle.Player.CalculateDamageOnUnit(
                target,
                DamageType.Physical,
                Trundle.Player.GetSpellDamage(target, SpellSlot.Q));

            if (qaa)
            {
                return target.Health - result > (Trundle.Player.TotalAttackDamage) * 0.3;
            }

            if (Item.HasItem(Botrk) && Item.CanUseItem(Botrk))
            {
                result += Trundle.Player.CalculateDamageOnUnit(
                    target,
                    DamageType.Physical,
                    Trundle.Player.GetItemDamage(target, (ItemId)Botrk));
            }

            if (Item.HasItem(Cutlass) && Item.CanUseItem(Cutlass))
            {
                result += Trundle.Player.CalculateDamageOnUnit(
                    target,
                    DamageType.Physical,
                    Trundle.Player.GetItemDamage(target, (ItemId)Cutlass));
            }

            if (Item.HasItem(Hydra) && Item.CanUseItem(Hydra))
            {
                result += Trundle.Player.CalculateDamageOnUnit(
                    target,
                    DamageType.Physical,
                    Trundle.Player.GetItemDamage(target, (ItemId)Hydra));
            }
            else if (Item.HasItem(Tiamat) && Item.CanUseItem(Tiamat))
            {
                result += Trundle.Player.CalculateDamageOnUnit(
                    target,
                    DamageType.Physical,
                    Trundle.Player.GetItemDamage(target, (ItemId)Tiamat));
            }

            if (Trundle.R.IsReady())
            {
                result += Trundle.Player.CalculateDamageOnUnit(target, DamageType.Magical, (float)rawDmg);
            }

            result += Trundle.Player.GetAutoAttackDamage(target);

            return result;
        }

        public static void Clear()
        {
            var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(
                EntityManager.UnitTeam.Enemy,
                Trundle.Player.Position,
                Trundle.Player.AttackRange);

            var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Trundle.Player.Position, Trundle.Player.AttackRange);

            UseQwc = Trundle.Player.ManaPercent > Trundle.GetOption(Trundle.MMenu, "WC");
            UseQjf = Trundle.Player.ManaPercent > Trundle.GetOption(Trundle.MMenu, "JF");
            if (Trundle.W.IsReady()
                && (minion.Any() && Trundle.GetOption(Trundle.WMenu, "WW")
                    || monster.Any() && Trundle.GetOption(Trundle.WMenu, "WJ")))
            {
                Trundle.W.Cast(Trundle.Player);
            }
        }

        public static void DoCombo(bool useW, bool useE, bool useR, bool harass)
        {
            var target = TargetSelector.GetTarget(Trundle.E.Range - Trundle.E.Radius / 2, DamageType.Physical);

            if (target == null || !target.IsValidTarget() || target.IsZombie)
            {
                return;
            }
            if (target.Distance(Trundle.Player) <= 550 && Trundle.GetOption(Trundle.CMenu, "TH"))
            {
                if (Item.HasItem(Botrk) && Item.CanUseItem(Botrk))
                {
                    Item.UseItem(Botrk, target);
                }
                if (Item.HasItem(Cutlass) && Item.CanUseItem(Cutlass))
                {
                    Item.UseItem(Cutlass, target);
                }
            }

            if (Trundle.R.IsReady() && useR)
            {
                Trundle.R.Cast(target);
            }

            if (Trundle.W.IsReady() && useW)
            {
                Trundle.W.Cast(target);
            }

            if (Trundle.W.IsReady() && useE)
            {
                Trundle.W.Cast(target);
            }
        }

        public static void PillarBlock()
        {
            foreach (
                var target in
                    EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget() && u.Distance(Trundle.Player) < Trundle.E.Range && u != null && u.IsDashing()))
            {
                if (Trundle.E.GetPrediction(target).HitChance == HitChance.Dashing)
                {
                    Trundle.E.Cast(target);
                }
            }
        }
    }
}