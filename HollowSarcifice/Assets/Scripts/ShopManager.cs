using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{

    //List to store items in shop and can change during runtime
   [SerializeField] private List<ShopItems> shopItems; 

// Array, easy to use for groups taht will stay the same size, can't change item number
   [SerializeField] private DragDrop[] shopSlots; 

   [SerializeField] private InventoryManager inventoryManager;


   private void Start(){

//this adds the text to the shop items

    PopulateShopItems();
   }

   public void PopulateShopItems(){
    for (int i=0; i< shopItems.Count && i < shopSlots.Length; i++){
        ShopItems shopItem = shopItems[i];
        //each item knows its own price here
        shopSlots[i].InitializeItem(shopItem.itemSO, shopItem.itemSO.price);
        shopSlots[i].gameObject.SetActive(true);


    }

    for (int i = shopItems.Count; i < shopSlots.Length; i++){
        shopSlots[i].gameObject.SetActive(false);
    }
   }

public void TrySellItem(ItemSO itemSO, int price)
{
    if (itemSO == null)
    {
        Debug.LogError("[ShopManager] TrySellItem: itemSO is null.");
        return;
    }

    if (inventoryManager == null)
    {
        Debug.LogError("[ShopManager] TrySellItem: inventoryManager is null. Assign it in the Inspector.");
        return;
    }

    
    int finalPrice = itemSO.price;

    Debug.Log($"The item name is {itemSO.itemName} and price is {finalPrice}");

    inventoryManager.money += finalPrice;

    if (inventoryManager.moneyText != null)
        inventoryManager.moneyText.text = inventoryManager.money.ToString();
    else
        Debug.LogWarning("[ShopManager] moneyText is null; assign the TMP/Text field on the InventoryManager.");
}

}

//gives access to System namespace (same as using system at the top of script)
//Serializable means that we will be able to see this in the inspector
[System.Serializable] 

public class ShopItems{
    public ItemSO itemSO;
    public int price;
}
