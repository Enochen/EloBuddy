namespace Leona
{
    using EloBuddy.SDK.Events;

    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Leona.OnGameLoad;
        }
    }
}
