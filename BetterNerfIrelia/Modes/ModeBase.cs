using EloBuddy.SDK;

namespace BetterNerfIrelia.Modes
{
    public abstract class ModeBase
    {
        protected Spell.Targeted Q
        {
            get { return SpellManager.Q; }
        }
        protected Spell.Active W
        {
            get { return SpellManager.W; }
        }
        protected Spell.Targeted E
        {
            get { return SpellManager.E; }
        }
        protected Spell.Skillshot R
        {
            get { return SpellManager.R; }
        }
        protected Spell.Targeted Ignite
        {
            get { return SpellManager.Ignite; }
        }
        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
