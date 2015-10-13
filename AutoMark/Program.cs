namespace AutoMark
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;

    internal class Program
    {
        private static Spell.Skillshot mark;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (Game.MapId != GameMapId.HowlingAbyss)
            {
                return;
            }
            var s1 = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1);
            var s2 = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2);
            if (s1.Name.ToLower() == "summonersnowball")
            {
                mark = new Spell.Skillshot(SpellSlot.Summoner1, 1600, SkillShotType.Linear, 0, 1300, 60);
            }
            else if (s2.Name.ToLower() == "summonersnowball")
            {
                mark = new Spell.Skillshot(SpellSlot.Summoner2, 1600, SkillShotType.Linear, 0, 1300, 60);
            }
            mark.MinimumHitChance = HitChance.High;
            Game.OnTick += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            if (TargetSelector.GetTarget(EntityManager.Heroes.Enemies, DamageType.True).Distance(ObjectManager.Player) > mark.Range
                || mark.IsOnCooldown || Player.Instance.HasBuff("snowballfollowupself"))
            {
                return;
            }
            mark.Cast(TargetSelector.GetTarget(EntityManager.Heroes.Enemies,DamageType.True));
        }
    }
}