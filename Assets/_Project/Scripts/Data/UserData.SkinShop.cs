namespace TheBeginning.UserData
{
    public partial struct UserData
    {
        public static bool IsItemEquipped(string itemIdentity)
        {
            return Get($"{Constant.EQUIP_ITEM}_{IdItemUnlocked}", false);
        }

        public static void SetItemEquipped(string itemIdentity, bool isEquipped = true)
        {
            Set($"{Constant.EQUIP_ITEM}_{IdItemUnlocked}", isEquipped);
        }

        public static string IdItemUnlocked = "";

        public static bool IsItemUnlocked
        {
            get => Get($"{Constant.UNLOCK_ITEM}_{IdItemUnlocked}", false);
            set => Set($"{Constant.UNLOCK_ITEM}_{IdItemUnlocked}", value);
        }

        public static int PercentWinGift
        {
            get => Get(Constant.PERCENT_WIN_GIFT, 0);
            set => Set(Constant.PERCENT_WIN_GIFT, value);
        }
    }
}