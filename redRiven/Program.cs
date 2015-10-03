namespace redRiven
{
    using EloBuddy.SDK.Events;

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Riven.OnGameLoad;
        }

        #endregion
    }
}