namespace redRiven
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;

    class Program
    {
        
        
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Riven.OnGameLoad;
        }
        
    }
}
