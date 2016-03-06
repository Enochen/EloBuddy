namespace HeavyGragas
{
    using System;
    using System.Collections.Generic;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Utils;

    using HeavyGragas.Modes;

    public static class ModeManager
    {
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        private static List<ModeBase> Modes { get; set; }


        static ModeManager()
        {
            Modes = new List<ModeBase>();

            Modes.AddRange(
                new ModeBase[]
                    {
                        new PermaActive(),  new Insec(), new Combo(),new LaneClear(), new JungleClear(), new Harass(),
                        new LastHit()
                    });

            Game.OnUpdate += OnUpdate;
            
        }

        public static void Initialize()
        {
        }

        private static void OnUpdate(EventArgs args)
        {
            Modes.ForEach(
                mode =>
                    {
                        try
                        {
                            if (!mode.ShouldBeExecuted()) { return; }
                            mode.Execute();
                        }
                        catch (Exception e)
                        {
                            Logger.Log(LogLevel.Error, "Error in mode '{0}'\n{1}", mode.GetType().Name, e);
                        }
                    });
        }
    }
}