namespace FeederBuddy
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public class InitializeMenu
    {
        #region Static Fields

        private static Menu fMenu, feMenu, miscMenu;

        #endregion

        #region Public Methods and Operators

        public static void Load()
        {
            fMenu = MainMenu.AddMenu("FeederBuddy", "feederbuddy");

            fMenu.AddGroupLabel("FeederBuddy - Darakath");
            fMenu.AddLabel("A lot of code is taken from blacky's BlackFeeder :^)");
            fMenu.AddSeparator();
            fMenu.Add("FeedingActivated", new CheckBox("Feeding Activated"));
            fMenu.Add("FeedingMode", new Slider("FeedMode: (0-Mid, 1-Bot, 2-Top, 3-Random)", 0, 0, 3));

            feMenu = fMenu.AddSubMenu("Feeding", "feedingmenu");
            feMenu.AddLabel("Feeding Options");
            feMenu.Add("SpellsActivated", new CheckBox("Spells Activated"));
            feMenu.Add("MessagesActivated", new CheckBox("Messages Activated"));
            feMenu.Add("LaughActivated", new CheckBox("Laugh Activated"));
            feMenu.Add("ItemsActivated", new CheckBox("Items Activated"));
            feMenu.Add("AttacksDisabled", new CheckBox("Disable auto attacks"));

            miscMenu = fMenu.AddSubMenu("Misc", "MiscMenu");
            miscMenu.AddLabel("Miscellaneous Options");
            miscMenu.Add("SurrenderActivated", new CheckBox("Auto Surrender Activated"));

            Entry.Menu = fMenu;
            Entry.FeedMenu = feMenu;
            Entry.MiscMenu = miscMenu;
        }

        #endregion
    }
}