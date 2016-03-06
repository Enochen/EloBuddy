using EloBuddy.SDK;

namespace Reborn_Leona.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            // TODO: Add flee logic here
        }
    }
}
