using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class ShopManagerStock : MonoBehaviour
{
    [SerializeField] private List<ShopItemsStock> shopItems;
    [SerializeField] private ShopSlotStock[] shopSlots;
    [SerializeField] private Button BuyButton;

    private void Start()
    {
        PopulateShopItems();
        
    }
    public void PopulateShopItems()
    {
        for (int i = 0; i < shopItems.Count && i < shopSlots.Length; i++)
        {
            ShopItemsStock shopItem = shopItems[i];
            shopSlots[i].Initialize(shopItem.itemSO, shopItem.itemSO.stockPrice);

            //shopSlots[i].Initialize(shopItem.itemSO, shopItem.price);
            shopSlots[i].gameObject.SetActive(true);
        }

        for (int i = shopItems.Count; i < shopSlots.Length; i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }
    }

    public void TryBuyItem(ItemSOStock itemSO, int price)
    {
        if(itemSO != null && MoneyCounter.Instance.money >= price)
        {
            MoneyCounter.Instance.money -= price;
            // needs to add the item to the game
            //needs to update the amount of money

             if (StockInventory.Instance != null && itemSO.shopItem != null)
        {
            StockInventory.Instance.AddStock(itemSO.shopItem, 1);
        }
        }
    }
}
[System.Serializable]
public class ShopItemsStock
{
    public ItemSOStock itemSO;
   // public int price;
}