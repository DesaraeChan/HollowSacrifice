using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopItems> shopItems;
    [SerializeField] private ShopSlot[] shopSlots;
    [SerializeField] private Button BuyButton;

    private void Start()
    {
        PopulateShopItems();
        
    }
    public void PopulateShopItems()
    {
        for (int i = 0; i < shopItems.Count && i < shopSlots.Length; i++)
        {
            ShopItems shopItem = shopItems[i];
            shopSlots[i].Initialize(shopItem.itemSO, shopItem.price);
            shopSlots[i].gameObject.SetActive(true);
        }

        for (int i = shopItems.Count; i < shopSlots.Length; i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }
    }

    public void TryBuyItem(ItemSO itemSO, int price)
    {
        if(itemSO != null && MoneyCounter.Instance.money >= price)
        {
            MoneyCounter.Instance.money -= price;
            // needs to add the item to the game
            //needs to update the amount of money
        }
    }
}
[System.Serializable]
public class ShopItems
{
    public ItemSO itemSO;
    public int price;
}