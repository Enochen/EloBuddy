using EloBuddy.SDK;

namespace BetterNerfIrelia.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            // TODO: Add jungleclear logic here
        }
    }
}
