namespace Reborn_Leona
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;

    //By 0x0539
    //From SluttyRyze
    public class Humanizer
    {
        public struct Action
        {
            public float Delay { get; set; }

            public float LastTick { get; set; }
        }

        public static Action General, Spell;

        public static void ChangeDelay(Action action, float nDelay)
        {
            action.Delay = nDelay;
        }

        public static bool CheckDelay(Action action)
        {
            if (!(Core.GameTickCount - action.LastTick >= action.Delay)) { return false; }
            
            action.LastTick = Core.GameTickCount;
            return true;
        }
    }
}