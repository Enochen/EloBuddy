using System.Linq;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Reborn_Leona
{
    public static class WidthMenu
    {
        private static readonly float[] Values =
        {
            0.01f,0.50f,1f,1.50f,2f,2.50f,3f
        };

        private static readonly string[] ValuesName =
        {
            "Tiny","Small", "Normal","Larger Than Normal", "Large", "Larger","Largest"
        };

        public static void AddWidthItem(this Menu menu, string uniqueId, int defaultWidth = 2)
        {
            var a = menu.Add(uniqueId, new Slider("Width: ", defaultWidth, 0, Values.Count() - 1));
            a.DisplayName = "Width: " + ValuesName[a.CurrentValue];
            a.OnValueChange += delegate { a.DisplayName = "Width: " + ValuesName[a.CurrentValue]; };
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