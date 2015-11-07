namespace SpaceAIDS.Modes
{
    using EloBuddy.SDK;

    public abstract class ModeBase
    {
        protected Spell.Skillshot Q
        {
            get
            {
                return SpellManager.Q;
            }
        }

        protected Spell.Skillshot W
        {
            get
            {
                return SpellManager.W;
            }
        }

        protected Spell.Targeted E
        {
            get
            {
                return SpellManager.E;
            }
        }

        protected Spell.Targeted R
        {
            get
            {
                return SpellManager.R;
            }
        }

        protected Spell.Targeted Fire
        {
            get
            {
                return SpellManager.Fire;
            }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}