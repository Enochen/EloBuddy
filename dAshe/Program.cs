using System;
using System.Drawing;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace dAshe
{
    using SharpDX;

    class Program
	{

		private static Menu mainMenu;
        private static Slider manaQ, manaW, manaR;
		private static Dictionary<SpellSlot, CheckBox> comboSpellStatus = new Dictionary<SpellSlot, CheckBox>();
		private static Dictionary<SpellSlot, CheckBox> waveClearSpellStatus = new Dictionary<SpellSlot, CheckBox>();
        public static Dictionary<SpellSlot, Spell.SpellBase> spells = new Dictionary<SpellSlot, Spell.SpellBase>()
        {
            {SpellSlot.Q, new Spell.Active(SpellSlot.Q)},
            {SpellSlot.W, new Spell.Skillshot(SpellSlot.W, 1200, SkillShotType.Linear, 250, 1500, 20)},
            {SpellSlot.R, new Spell.Skillshot(SpellSlot.R, 25000, SkillShotType.Linear, 250, 1600, 130)}
        };
        public static object W = spells[SpellSlot.W];
        public static object R = spells[SpellSlot.R];

        static void Main(string[] args)
		{
			Loading.OnLoadingComplete += onLoadComplete;
		}

		private static void onLoadComplete(EventArgs args)
		{
			if (Player.Instance.ChampionName != "Ashe")
				return;
            SetupSpellStatus();
			Drawing.OnDraw += Drawing_OnDraw;
			Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            mainMenu = MainMenu.AddMenu("dAshe", "dAsheMenu");
            mainMenu.AddGroupLabel("dAshe by Darakath - Enjoy the bugs");
            mainMenu.AddGroupLabel("Combo");
			comboSpellStatus[SpellSlot.Q] = mainMenu.Add("comboUseQ", new CheckBox("Use Q"));
			comboSpellStatus[SpellSlot.W] = mainMenu.Add("comboUseW", new CheckBox("Use W"));
			comboSpellStatus[SpellSlot.R] = mainMenu.Add("comboUseR", new CheckBox("Use R"));
			mainMenu.AddGroupLabel("Farming");
		    waveClearSpellStatus[SpellSlot.W] = mainMenu.Add("waveClearUseW", new CheckBox("Use W"));
            mainMenu.AddGroupLabel("Mana Options");
		    manaQ = mainMenu.Add("manaQ", new Slider("Min Mana % to Q"));
            manaW = mainMenu.Add("manaW", new Slider("Min Mana % to W"));
            manaR = mainMenu.Add("manaR", new Slider("Min Mana % to R"));
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
			}
		}

		private static void WaveClear()
		{
			
		}
            
		private static void Combo()
		{
            var target = TargetSelector.GetTarget(1200f, DamageType.Physical);
            var rtarget = TargetSelector.GetTarget(3000f, DamageType.Physical);
            var useW = (comboSpellStatus[SpellSlot.W].CurrentValue && spells[SpellSlot.W].IsReady() && (Player.Instance.ManaPercent >= manaW.CurrentValue));
		    var useR = (comboSpellStatus[SpellSlot.R].CurrentValue && spells[SpellSlot.R].IsReady() && (Player.Instance.ManaPercent >= manaR.CurrentValue) && rtarget.Distance(Player.Instance) > Player.Instance.GetAutoAttackRange(rtarget));
            if (useW)
				spells[SpellSlot.W].Cast(target);
		    if (useR)
                spells[SpellSlot.R].Cast(rtarget);

        }

		private static void SetupSpellStatus()
		{
			comboSpellStatus.Add(SpellSlot.Q, null);
			comboSpellStatus.Add(SpellSlot.W, null);
			comboSpellStatus.Add(SpellSlot.R, null);
			waveClearSpellStatus.Add(SpellSlot.Q, null);
			waveClearSpellStatus.Add(SpellSlot.W, null);

		}

		private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
		{
            bool useQ = (comboSpellStatus[SpellSlot.Q].CurrentValue && spells[SpellSlot.Q].IsReady() && (Player.Instance.ManaPercent >= manaQ.CurrentValue));
		    if (useQ)
		    {
		        if (target.Type == GameObjectType.AIHeroClient)
		        {
		            foreach (BuffInstance buff in Player.Instance.Buffs)
		            {
		                if (buff.Name == "asheqcastready" && buff.Count == 5)
		                {
		                    spells[SpellSlot.Q].Cast();
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
