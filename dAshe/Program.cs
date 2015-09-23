namespace dAshe
{
    using System;
    using System.Collections.Generic;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using SharpDX;

    internal class Program
    {
        #region Static Fields

        public static Dictionary<SpellSlot, Spell.SpellBase> Spells = new Dictionary<SpellSlot, Spell.SpellBase>
                                                                          {
                                                                              {
                                                                                  SpellSlot.Q,
                                                                                  new Spell.Active(SpellSlot.Q)
                                                                              },
                                                                              {
                                                                                  SpellSlot.W,
                                                                                  new Spell.Skillshot(
                                                                                  SpellSlot.W,
                                                                                  1200,
                                                                                  SkillShotType.Linear,
                                                                                  250,
                                                                                  1500,
                                                                                  20)
                                                                              },
                                                                              {
                                                                                  SpellSlot.R,
                                                                                  new Spell.Skillshot(
                                                                                  SpellSlot.R,
                                                                                  25000,
                                                                                  SkillShotType.Linear,
                                                                                  250,
                                                                                  1600,
                                                                                  130)
                                                                              }
                                                                          };

        private static readonly Dictionary<SpellSlot, CheckBox> comboSS = new Dictionary<SpellSlot, CheckBox>();

        private static readonly Dictionary<SpellSlot, CheckBox> harassSS = new Dictionary<SpellSlot, CheckBox>();

        private static readonly Dictionary<SpellSlot, CheckBox> waveClearSS = new Dictionary<SpellSlot, CheckBox>();

        private static Menu mainMenu, comboMenu, harassMenu, manaMenu;

        private static Slider manaQ, manaW, manaR, manaHarass;

        #endregion

        #region Methods

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(1200f, DamageType.Physical);
            var rtarget = TargetSelector.GetTarget(3000f, DamageType.Magical);
            var useW = (comboSS[SpellSlot.W].CurrentValue && Spells[SpellSlot.W].IsReady()
                        && (Player.Instance.ManaPercent >= manaW.CurrentValue));
            var useR = (comboSS[SpellSlot.R].CurrentValue && Spells[SpellSlot.R].IsReady()
                        && (Player.Instance.ManaPercent >= manaR.CurrentValue)
                        && rtarget.Distance(Player.Instance) > Player.Instance.GetAutoAttackRange(rtarget));
            if (useW)
            {
                Spells[SpellSlot.W].Cast(target);
            }
            if (useR)
            {
                Spells[SpellSlot.R].Cast(rtarget);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
        }

        private static void Game_OnTick(EventArgs args)
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

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(1200f, DamageType.Physical);
            if (target == null || Orbwalker.IsAutoAttacking)
            {
                return;
            }
            if (harassSS[SpellSlot.W].CurrentValue && Spells[SpellSlot.W].IsReady()
                && (Player.Instance.ManaPercent >= manaHarass.CurrentValue))
            {
                Spells[SpellSlot.W].Cast(target);
            }
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadComplete;
        }

        private static void OnLoadComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Ashe")
            {
                return;
            }
            SetupSpellStatus();
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            mainMenu = MainMenu.AddMenu("dAshe", "dAsheMenu");
            mainMenu.AddGroupLabel("dAshe by Darakath - Enjoy the bugs");
            mainMenu.AddSeparator();
            comboMenu = mainMenu.AddSubMenu("Combo");
            comboMenu.AddGroupLabel("Combo Options");
            comboSS[SpellSlot.Q] = comboMenu.Add("comboUseQ", new CheckBox("Use Q"));
            comboSS[SpellSlot.W] = comboMenu.Add("comboUseW", new CheckBox("Use W"));
            comboSS[SpellSlot.R] = comboMenu.Add("comboUseR", new CheckBox("Use R"));
            //mainMenu.AddGroupLabel("Waveclear");
            //waveClearSS[SpellSlot.W] = mainMenu.Add("waveClearUseW", new CheckBox("Use W"));
            harassMenu = mainMenu.AddSubMenu("Harass");
            harassMenu.AddGroupLabel("Harass Options");
            harassSS[SpellSlot.W] = harassMenu.Add("harassUseW", new CheckBox("Use W"));
            manaMenu = mainMenu.AddSubMenu("Mana");
            manaMenu.AddGroupLabel("Mana Options");
            manaQ = manaMenu.Add("manaQ", new Slider("Min Mana % to Q"));
            manaW = manaMenu.Add("manaW", new Slider("Min Mana % to W"));
            manaR = manaMenu.Add("manaR", new Slider("Min Mana % to R"));
            manaHarass = manaMenu.Add("manaHarass", new Slider("Min Mana % to Harass"));
            Game.OnTick += Game_OnTick;
            Chat.Print("dAshe By Darakath loaded.", Color.DarkBlue);
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            var useQ = (comboSS[SpellSlot.Q].CurrentValue && Spells[SpellSlot.Q].IsReady()
                        && (Player.Instance.ManaPercent >= manaQ.CurrentValue));
            if (useQ)
            {
                if (target.Type == GameObjectType.AIHeroClient)
                {
                    foreach (var buff in Player.Instance.Buffs)
                    {
                        if (buff.Name == "asheqcastready" && buff.Count == 5)
                        {
                            Spells[SpellSlot.Q].Cast();
                        }
                    }
                }
            }
        }

        private static void SetupSpellStatus()
        {
            comboSS.Add(SpellSlot.Q, null);
            comboSS.Add(SpellSlot.W, null);
            comboSS.Add(SpellSlot.R, null);
            harassSS.Add(SpellSlot.W, null);
            waveClearSS.Add(SpellSlot.W, null);
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

        #endregion
    }
}