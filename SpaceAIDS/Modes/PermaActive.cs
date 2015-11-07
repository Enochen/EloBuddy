namespace SpaceAIDS.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            //if (Player.Instance.Spellbook.IsChanneling
            //    || EntityManager.Heroes.AllHeroes.Count(x => x.HasBuff("AlZaharNetherGrasp")) > 0)
            //{
            //    Orbwalker.DisableMovement = true;
            //    Orbwalker.DisableAttacking = true;
            //    Chat.Say("1");
            //}
            //else
            //{
            //    Orbwalker.DisableMovement = false;
            //    Orbwalker.DisableAttacking = false;
            //    Chat.Say("nuu");
            //}
            //Chat.Print("1");
        }
    }
}