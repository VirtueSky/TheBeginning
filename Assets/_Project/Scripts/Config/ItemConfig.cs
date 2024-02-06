using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "ScriptableObject/ItemConfig")]
public class ItemConfig : ScriptableObject
{
    public List<ItemData> itemDatas;

    public void Initialize()
    {
        //if (IAPManager.Instance.IsPurchased(Constant.IAP_VIP) || IAPManager.Instance.IsPurchased(Constant.IAP_ALL_SKINS)) UnlockAllSkins();
        UnlockDefaultSkins();
    }

    public void UnlockDefaultSkins()
    {
        foreach (ItemData item in itemDatas)
        {
            if (item.BuyType == BuyType.Default)
            {
                item.ClaimItem();
            }
        }
    }

    public void UnlockAllSkins()
    {
        foreach (ItemData itemData in itemDatas)
        {
            itemData.IsUnlocked = true;
        }
    }

    public ItemData GetItemData(string itemIdentity)
    {
        return itemDatas.Find(item => item.Identity == itemIdentity);
    }

    public List<ItemData> GetListItemDataByType(ItemType itemType)
    {
        return itemDatas.FindAll(item => item.Type == itemType);
    }

    public ItemData GetGiftItemData()
    {
        List<ItemData> tempList =
            itemDatas.FindAll(item =>
                !item.IsUnlocked && (item.BuyType == BuyType.BuyCoin || item.BuyType == BuyType.WatchAds));
        return tempList.Count > 0 ? tempList[Random.Range(0, tempList.Count)] : null;
    }
}

public class ItemIdentity
{
    public string Identity => $"{Type}_{NumberID}";

    public string Name;
    public ItemType Type;
    public int NumberID;
}

[Serializable]
public class ItemData : ItemIdentity
{
    public BuyType BuyType;
    public Sprite ShopIcon;
    [ShowIf("BuyType", BuyType.BuyCoin)] public int CoinValue;

    public void ClaimItem()
    {
        IsUnlocked = true;
        EquipItem();
    }

    public void EquipItem()
    {
        UserData.SetItemEquipped(Identity);
    }

    public bool IsUnlocked
    {
        get
        {
            UserData.IdItemUnlocked = Identity;
            return UserData.IsItemUnlocked;
        }

        set
        {
            //FirebaseManager.OnClaimItemSkin(Identity);
            UserData.IdItemUnlocked = Identity;
            UserData.IsItemUnlocked = value;
        }
    }
}

public enum BuyType
{
    Default,
    BuyCoin,
    DailyReward,
    WatchAds,
    Event,
}

public enum ItemType
{
    PlayerSkin,
    WeaponSkin,
}