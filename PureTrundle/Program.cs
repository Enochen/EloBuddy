namespace PureTrundle
{
    using EloBuddy.SDK.Events;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Trundle.OnGameLoad;
        }
    }
}