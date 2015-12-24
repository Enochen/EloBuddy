using EloBuddy;
using EloBuddy.SDK;
using Settings = Rice.Config.Modes.Harass;

namespace Rice.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && Player.Instance.ManaPercent > Settings.Mana;
        }

        public override void Execute()
        {
            if (!Settings.UseQ || !Q.IsReady()) { return; }
            var target = TargetSelector.GetTarget(this.Q.Range, DamageType.Physical);
            if (target != null)
            {
                this.Q.PredCast(target);
            }
        }
    }
}
