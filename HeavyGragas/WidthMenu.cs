namespace HeavyGragas
{
    using System.Linq;

    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public static class WidthMenu
    {
        private static readonly float[] Values = { 0.01f, 1f, 2f, 3f, 4f, 5f, 6f };

        private static readonly string[] ValuesName =
            {
                "Tiny", "Small", "Normal", "Larger Than Normal", "Large",
                "Larger", "Largest"
            };

        public static void AddWidthItem(this Menu menu, string displayText, string uniqueId, int defaultWidth = 2)
        {
            var a = menu.Add(uniqueId, new Slider(displayText, defaultWidth, 0, Values.Count() - 1));
            a.DisplayName = displayText + ValuesName[a.CurrentValue];
            a.OnValueChange += delegate { a.DisplayName = displayText + ValuesName[a.CurrentValue]; };
        }

        public static float GetWidth(this Menu m, string id)
        {
            var number = m[id].Cast<Slider>();
            if (number != null)
            {
                return Values[number.CurrentValue];
            }
            return 1f;
        }
    }
}