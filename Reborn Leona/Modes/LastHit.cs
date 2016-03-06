using Settings = Reborn_Leona.Config.Modes.LastHit;

namespace Reborn_Leona.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)
                   && Player.Instance.ManaPercent > Settings.Mana;
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady())
            {
                ModeManager.ShouldQ = true;
            }
        }
    }
}