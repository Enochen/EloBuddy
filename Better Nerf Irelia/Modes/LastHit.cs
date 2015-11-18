using EloBuddy;
using EloBuddy.SDK;

namespace BetterNerfIrelia.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            // TODO: Add lasthit logic here
            //if (Q.IsReady())
            //{
            //    var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            //    if (target != null)
            //    {
            //        Q.Cast(target);
            //    }
            //}
        }
    }
}
