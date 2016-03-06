using System.Linq;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Color = System.Drawing.Color;

namespace Reborn_Leona
{
    public static class ColorMenu
    {
        private static readonly Color[] Colors =
        {
            Color.Blue, Color.Red, Color.Yellow, Color.Green, Color.Purple, Color.Black, Color.Brown, Color.Gray,
            Color.Pink, Color.BlueViolet, Color.Aqua, Color.MidnightBlue, Color.Gold, Color.LimeGreen, Color.Violet,
            Color.WhiteSmoke, Color.DarkRed, Color.Violet, Color.IndianRed, Color.MediumVioletRed, Color.Orange,
            Color.OrangeRed
        };

        private static readonly string[] ColorsName =
        {
            "Blue", "Red", "Yellow", "Green", "Purple", "Black", "Brown", "Gray", "Pink", "BlueViolet", "Aqua",
            "MidnightBlue", "Gold", "LimeGreen", "Violet", "WhiteSmoke", "DarkRed", "Violet", "IndianRed",
            "Medium Violet Red", "Orange", "Orange Red"
        };

        public static void AddColorItem(this Menu menu, string uniqueId, int defaultColour = 0)
        {
            var a = menu.Add(uniqueId, new Slider("Color Picker: ", defaultColour, 0, Colors.Count() - 1));
            a.DisplayName = "Color Picker: " + ColorsName[a.CurrentValue];
            a.OnValueChange += delegate { a.DisplayName = "Colour Picker: " + ColorsName[a.CurrentValue]; };
        }

        public static Color GetColor(this Menu m, string id)
        {
            var number = m[id].Cast<Slider>();
            if (number != null)
            {
                return Colors[number.CurrentValue];
            }
            return Color.White;
        }
    }
}