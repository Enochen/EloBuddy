namespace Rice.Modes
{
    using EloBuddy.SDK;

    public abstract class ModeBase
    {
        protected Spell.Skillshot Q
        {
            get
            {
                return Config.Modes.Misc.QCollision == 1 ? SpellManager.Q2 : SpellManager.Q1;
            }
        }

        protected Spell.Targeted W
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

        protected Spell.Active R
        {
            get
            {
                return SpellManager.R;
            }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}