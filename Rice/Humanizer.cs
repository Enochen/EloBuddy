namespace Rice
{
    using System;

    using EloBuddy;

    //By 0x0539
    //From SluttyRyze
    public class Humanizer
    {
        private struct Action
        {

            public float Delay { get; set; }

            public float LastTick { get; set; }
        }

        private static Action General;

        public static void ChangeDelay(float nDelay)
        {
            General.Delay = nDelay;
        }

        public static bool CheckDelay(string actionName)
        {

            if (!(Environment.TickCount - General.LastTick >= General.Delay)) { return false; }
            
            General.LastTick = Environment.TickCount;

            Chat.Print(Environment.TickCount + " - " + General.LastTick);
            return true;
        }
    }
}