namespace Rice
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;

    //By 0x0539
    //From SluttyRyze
    public class Humanizer
    {
        private struct Action
        {

            public float Delay { get; set; }

            public float LastTick { get; set; }
        }

        private static Action general;

        public static void ChangeDelay(float nDelay)
        {
            general.Delay = nDelay;
        }

        public static bool CheckDelay(string actionName)
        {
            if (!(Core.GameTickCount - general.LastTick >= general.Delay)) { return false; }
            
            general.LastTick = Core.GameTickCount;
            return true;
        }
    }
}