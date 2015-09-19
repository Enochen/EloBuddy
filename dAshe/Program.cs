using System;
using SharpDX;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace dAshe
{
    using System.Linq;

    class Program
	{

		private static Menu mainMenu;
        private static Slider manaQ, manaW, manaR, manaFarm, manaHarass;
		private static Dictionary<SpellSlot, CheckBox> comboSS = new Dictionary<SpellSlot, CheckBox>();
		private static Dictionary<SpellSlot, CheckBox> waveClearSS = new Dictionary<SpellSlot, CheckBox>();
        private static Dictionary<SpellSlot, CheckBox> harassSS = new Dictionary<SpellSlot, CheckBox>();
        public static Dictionary<SpellSlot, Spell.SpellBase> Spells = new Dictionary<SpellSlot, Spell.SpellBase>()
        {
            {SpellSlot.Q, new Spell.Active(SpellSlot.Q)},
            {SpellSlot.W, new Spell.Skillshot(SpellSlot.W, 1200, SkillShotType.Linear, 250, 1500, 20)},
            {SpellSlot.R, new Spell.Skillshot(SpellSlot.R, 25000, SkillShotType.Linear, 250, 1600, 130)}
        };

        static void Main(string[] args)
		{
			Loading.OnLoadingComplete += OnLoadComplete;
		}

		private static void OnLoadComplete(EventArgs args)
		{
			if (Player.Instance.ChampionName != "Ashe")
				return;
            SetupSpellStatus();
			Drawing.OnDraw += Drawing_OnDraw;
			Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            mainMenu = MainMenu.AddMenu("dAshe", "dAsheMenu");
            mainMenu.AddGroupLabel("dAshe by Darakath - Enjoy the bugs");
            mainMenu.AddSeparator();
            mainMenu.AddGroupLabel("Combo");
			comboSS[SpellSlot.Q] = mainMenu.Add("comboUseQ", new CheckBox("Use Q"));
			comboSS[SpellSlot.W] = mainMenu.Add("comboUseW", new CheckBox("Use W"));
			comboSS[SpellSlot.R] = mainMenu.Add("comboUseR", new CheckBox("Use R"));
            mainMenu.AddSeparator();
			//mainMenu.AddGroupLabel("Waveclear");
		    //waveClearSS[SpellSlot.W] = mainMenu.Add("waveClearUseW", new CheckBox("Use W"));
            mainMenu.AddGroupLabel("Harass");
            harassSS[SpellSlot.W] = mainMenu.Add("harassUseW", new CheckBox("Use W"));
            mainMenu.AddSeparator();
            mainMenu.AddGroupLabel("Mana Options");
		    manaQ = mainMenu.Add("manaQ", new Slider("Min Mana % to Q"));
            manaW = mainMenu.Add("manaW", new Slider("Min Mana % to W"));
		    manaR = mainMenu.Add("manaR", new Slider("Min Mana % to R"));
            manaHarass = mainMenu.Add("manaHarass", new Slider("Min Mana % to Harass"));
            Game.OnTick += Game_OnTick;
            Chat.Print("dAshe By Darakath loaded.", Color.DarkBlue);
		}

		static void Game_OnTick(EventArgs args)
		{
			switch (Orbwalker.ActiveModesFlags)
			{
				case Orbwalker.ActiveModes.Combo:
					Combo();
					break;
				case Orbwalker.ActiveModes.LaneClear:
					WaveClear();
					break;
                case Orbwalker.ActiveModes.Harass:
			        Harass();
			        break;
			}
		}

		private static void WaveClear()
		{
		    /*if (waveClearSS[SpellSlot.W].CurrentValue && Spells[SpellSlot.W].IsReady()
		        && (Player.Instance.ManaPercent >= manaFarm.CurrentValue))
		    {
                var creep = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(c => c.IsEnemy && c.Health <= Player.Instance.GetAutoAttackDamage(c));
                if (creep == null) return;
                Spells[SpellSlot.W].Cast(creep);
            }*/
        }
            
		private static void Combo()
		{
            var target = TargetSelector.GetTarget(1200f, DamageType.Physical);
            var rtarget = TargetSelector.GetTarget(3000f, DamageType.Magical);
            var useW = (comboSS[SpellSlot.W].CurrentValue && Spells[SpellSlot.W].IsReady() && (Player.Instance.ManaPercent >= manaW.CurrentValue));
		    var useR = (comboSS[SpellSlot.R].CurrentValue && Spells[SpellSlot.R].IsReady() && (Player.Instance.ManaPercent >= manaR.CurrentValue) && rtarget.Distance(Player.Instance) > Player.Instance.GetAutoAttackRange(rtarget));
            if (useW)
				Spells[SpellSlot.W].Cast(target);
		    if (useR)
                Spells[SpellSlot.R].Cast(rtarget);

        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(1200f, DamageType.Physical);
            if(target == null || Orbwalker.IsAutoAttacking) return;
            if(harassSS[SpellSlot.W].CurrentValue && Spells[SpellSlot.W].IsReady() && (Player.Instance.ManaPercent >= manaHarass.CurrentValue))
                Spells[SpellSlot.W].Cast(target);
        }

		private static void SetupSpellStatus()
		{
			comboSS.Add(SpellSlot.Q, null);
			comboSS.Add(SpellSlot.W, null);
			comboSS.Add(SpellSlot.R, null);
			harassSS.Add(SpellSlot.W, null);
			waveClearSS.Add(SpellSlot.W, null);

		}

		private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
		{
            bool useQ = (comboSS[SpellSlot.Q].CurrentValue && Spells[SpellSlot.Q].IsReady() && (Player.Instance.ManaPercent >= manaQ.CurrentValue));
		    if (useQ)
		    {
		        if (target.Type == GameObjectType.AIHeroClient)
		        {
		            foreach (BuffInstance buff in Player.Instance.Buffs)
		            {
		                if (buff.Name == "asheqcastready" && buff.Count == 5)
		                {
		                    Spells[SpellSlot.Q].Cast();
		                }
		            }
		        }
		    }
            
			
		}

		private static void Drawing_OnDraw(EventArgs args)
		{
			
		}

	}
}
