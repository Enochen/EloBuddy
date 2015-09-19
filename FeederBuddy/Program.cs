using System;
using EloBuddy;

namespace FeederBuddy
{
    using EloBuddy.SDK.Events;

    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Loading.OnLoadingComplete += Entry.OnLoad;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }

        private static void E(EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}