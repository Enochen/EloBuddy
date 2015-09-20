namespace SkinsPlus
{
    using EloBuddy.SDK.Events;

    internal class Program
    {
        #region Methods
        private static void Main()
        {
            Loading.OnLoadingComplete += Setup.OnGameLoad;
        }
        #endregion
    }
}