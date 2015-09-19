using System.Text.RegularExpressions;

namespace FeederBuddy.SRShopAI
{
    class Item
    {
        public string Name;
        public int Id;
        public int[] Into;
        public int[] From;
        public int[] Maps;
        public int Goldbase;
        public bool Ispurchasable;
        public int Goldtotal;
        public int Goldsell;

        public static string GetNameFromBlock(string block)
        {
            return Regex.Matches(block, "(?<=\"name\":\").*?(?=\")")[0].Groups[0].ToString();
        }

        public static int GetIdFromBlock(string block)
        {
            return int.Parse(block.Split(':')[0].Replace("\"", ""));
        }

        public static int[] GetIntoFromBlock(string block)
        {
            var stringItemArray = Regex.Match(block, "(?<=\"into\":\\[).*?(?=])").ToString().Split(',');
            if (stringItemArray[0].Length < 4)
                return null;
            var intItemArray = new int[stringItemArray.Length];
            for (int i = 0; i < stringItemArray.Length; i++)
                intItemArray[i] = int.Parse(stringItemArray[i].Replace("\"", ""));
            return intItemArray;
        }

        public static int[] GetFromFromBlock(string block)
        {
            var stringItemArray = Regex.Match(block, "(?<=\"from\":\\[).*?(?=])").ToString().Split(',');
            if (stringItemArray[0].Length < 4)
                return null;
            var intItemArray = new int[stringItemArray.Length];
            for (int i = 0; i < stringItemArray.Length; i++)
                intItemArray[i] = int.Parse(stringItemArray[i].Replace("\"", ""));
            return intItemArray;
        }

        public static int[] GetMapsFromBlock(string block)
        {
            var stringItemArray = Regex.Match(block, "(?<=\"maps\":\\{).*?(?=})").ToString().Split(',');
            if (stringItemArray[0].Length < 1)
                return null;
            var intItemArray = new int[stringItemArray.Length];
            for (int i = 0; i < stringItemArray.Length; i++)
                intItemArray[i] = int.Parse(stringItemArray[i].Replace("\"", "").Split(':')[0]);
            return intItemArray;
        }

        public static int GetGoldBaseFromBlock(string block)
        {
            var stringPriceArray = Regex.Match(block, "(?<=\"gold\":\\{).*?(?=})").ToString().Split(',');
            return int.Parse(stringPriceArray[0].Split(':')[1].Replace("\"", ""));
        }

        public static bool GetPurchasableBaseFromBlock(string block)
        {
            var stringPriceArray = Regex.Match(block, "(?<=\"gold\":\\{).*?(?=})").ToString().Split(',');
            return bool.Parse(stringPriceArray[1].Split(':')[1].Replace("\"", ""));
        }

        public static int GetGoldTotalFromBlock(string block)
        {
            var stringPriceArray = Regex.Match(block, "(?<=\"gold\":\\{).*?(?=})").ToString().Split(',');
            return int.Parse(stringPriceArray[2].Split(':')[1].Replace("\"", ""));
        }
        public static int GetGoldSellFromBlock(string block)
        {
            var stringPriceArray = Regex.Match(block, "(?<=\"gold\":\\{).*?(?=})").ToString().Split(',');
            return int.Parse(stringPriceArray[3].Split(':')[1].Replace("\"", ""));
        }

        public Item(string inputBlock)
        {
            Name = GetNameFromBlock(inputBlock);
            Id = GetIdFromBlock(inputBlock);
            if (GetIntoFromBlock(inputBlock) != null)
                Into = GetIntoFromBlock(inputBlock);
            if (GetFromFromBlock(inputBlock) != null)
                From = GetFromFromBlock(inputBlock);
            if (GetMapsFromBlock(inputBlock) != null)
                Maps = GetMapsFromBlock(inputBlock);
            Goldbase = GetGoldBaseFromBlock(inputBlock);
            Ispurchasable = GetPurchasableBaseFromBlock(inputBlock);
            Goldtotal = GetGoldTotalFromBlock(inputBlock);
            Goldsell = GetGoldSellFromBlock(inputBlock);
        }

        public override string ToString()
        {
            var info =
            "ITEM NAME: " + Name + "\n" + "ITEM ID: " + Id;
            return info;
        }
    }
}
