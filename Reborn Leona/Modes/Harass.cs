using EloBuddy;
using EloBuddy.SDK;
using Settings = Reborn_Leona.Config.Modes.Harass;

namespace Reborn_Leona.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && Player.Instance.ManaPercent > Config.Modes.Harass.Mana;
        }

        public override void Execute()
        {
            
        }
    }
}
