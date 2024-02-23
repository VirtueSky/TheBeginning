using System;
using System.Collections.Generic;
using TheBeginning.UserData;
using UnityEngine;
using VirtueSky.Inspector;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "ScriptableObject/ItemConfig")]
public class ItemConfig : ScriptableObject
{
    [SerializeField] private List<ItemData> listItemData;
    public List<ItemData> ListItemData => listItemData;

    public void Initialize()
    {
        UnlockSkinsDefault();
    }

    public void UnlockSkinsDefault()
    {
        foreach (ItemData item in listItemData)
        {
            if (item.buyType == BuyType.Default)
            {
                item.IsUnlocked = true;
            }
        }
    }

    public void UnlockAllSkins()
    {
        foreach (ItemData itemData in listItemData)
        {
            itemData.IsUnlocked = true;
        }
    }

    public ItemData GetItemData(string itemIdentity)
    {
        return listItemData.Find(item => item.Identity == itemIdentity);
    }

    public List<ItemData> GetListItemDataByType(ItemType itemType)
    {
        return listItemData.FindAll(item => item.itemType == itemType);
    }

    public ItemData GetGiftItemData()
    {
        List<ItemData> tempList =
            listItemData.FindAll(item =>
                !item.IsUnlocked && (item.buyType == BuyType.BuyCoin || item.buyType == BuyType.WatchAds));
        return tempList.Count > 0 ? tempList[Random.Range(0, tempList.Count)] : null;
    }
}

public class ItemIdentity
{
    public string Identity => $"{itemType}_{numberID}";

    public string name;
    public ItemType itemType;
    public int numberID;
}

[Serializable]
public class ItemData : ItemIdentity
{
    public BuyType buyType;
     public Sprite shopIcon;
    [ShowIf(nameof(buyType), BuyType.BuyCoin)] public int coinValue;

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