using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using EloBuddy;
using EloBuddy.SDK;

namespace FeederBuddy.SRShopAI
{
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu.Values;

    class Main
    {
        private static Item lastItem;
        private static int priceAddup;
        private static readonly List<Item> ItemList = new List<Item>();

        public static void ItemSequence(Item item, Queue<Item> shopListQueue)
        {
            if (item.From == null)
                shopListQueue.Enqueue(item);
            else
            {
                foreach (int itemDescendant in item.From)
                    ItemSequence(GetItemById(itemDescendant), shopListQueue);
                shopListQueue.Enqueue(item);
            }
        }
        public static Item GetItemById(int id)
        {
            return ItemList.Single(x => x.Id.Equals(id));
        }
        public static Item GetItemByName(string name)
        {
            return ItemList.FirstOrDefault(x => x.Name.Equals(name));
        }
        public static string Request(string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            Debug.Assert(dataStream != null, "dataStream != null");
            StreamReader reader = new StreamReader(stream: dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return responseFromServer;
        }
        public static string[] List = new[] { "Boots of Speed", "Boots of Mobility", "Aether Wisp", "Zeal", "Zeal", "Zeal", "Zeal" };

        public static Queue<Item> Queue = new Queue<Item>();
        public static bool CanBuy = true;
        public static void Init()
        {
            string itemJson = "https://raw.githubusercontent.com/myo/Experimental/master/item.json";
            string itemsData = Request(itemJson);
            string itemArray = itemsData.Split(new[] { "data" }, StringSplitOptions.None)[1];
            MatchCollection itemIdArray = Regex.Matches(itemArray, "[\"]\\d*[\"][:][{].*?(?=},\"\\d)");
            foreach (Item item in from object iItem in itemIdArray select new Item(iItem.ToString()))
                ItemList.Add(item);
            Console.WriteLine("Auto Buy Activated");
            Loading.OnLoadingComplete += Game_OnGameLoad;
            AIHeroClient.OnSpawn += CustomEvents_OnSpawn;
        }

        static void CustomEvents_OnSpawn(Obj_AI_Base sender)
        {
            if (sender.NetworkId == ObjectManager.Player.NetworkId)
                BuyItems();
        }
        static void Game_OnGameLoad(EventArgs args)
        {
            Queue = ShoppingQueue();
            AlterInventory();

            BuyItems();
        }
        public static Queue<Item> ShoppingQueue()
        {
            var shoppingItems = new Queue<Item>();
            foreach (string indexItem in List)
            {
                var macroItems = new Queue<Item>();
                ItemSequence(GetItemByName(indexItem), macroItems);
                foreach (Item secondIndexItem in macroItems)
                    shoppingItems.Enqueue(secondIndexItem);
            }
            return shoppingItems;
        }
        public static void BuyItems()
        {
            
            if (!Entry.FeedMenu["ItemsActivated"].Cast<CheckBox>().CurrentValue) return;
            while ((Queue.Peek() != null && InventoryFull()) && (Queue.Peek().From == null ||(Queue.Peek().From != null && !Queue.Peek().From.Contains(lastItem.Id))))
            {
                var y = Queue.Dequeue();
                priceAddup += y.Goldbase;
            }
            var x = 0;
                while ( Queue.Peek().Goldbase <= ObjectManager.Player.Gold - x - priceAddup && Queue.Count > 0 &&
                       Shop.CanShop)
                {
                    var y = Queue.Dequeue();
                    Shop.BuyItem((ItemId)y.Id);
                    lastItem = y;
                    priceAddup = 0;
                    x += y.Goldbase;
                }
        }
        public static int FreeSlots()
        {
            return -1 + ObjectManager.Player.InventoryItems.Count();
        }

        public static bool InventoryFull()
        {
            return FreeSlots() == 6;
        }

        public static void AlterInventory()
        {
            var y = 0;
            var z = ObjectManager.Player.InventoryItems.ToList().OrderBy(i => i.Slot).Select(item => item.Id).ToList();
            for(int i = 0; i < z.Count - 2; i++)
            {
                var x = GetItemById((int) z[i]);
                Queue<Item> temp = new Queue<Item>();
                ItemSequence(x, temp);
                y += temp.Count;
            }
            for (int i = 0; i < y; i++)
                Console.WriteLine(Queue.Dequeue());
        }
    }
}
