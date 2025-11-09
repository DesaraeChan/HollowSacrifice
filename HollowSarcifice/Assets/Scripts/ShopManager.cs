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

   [SerializeField] private TMP_Text totalText; //shows running money total
   [SerializeField] private Button sellButton;

    [SerializeField] private ItemSlot[] itemSlots;


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

// public void TrySellItem(ItemSO itemSO, int price)
// {
//     if (itemSO == null)
//     {
//         Debug.LogError("[ShopManager] TrySellItem: itemSO is null.");
//         return;
//     }

//     if (inventoryManager == null)
//     {
//         Debug.LogError("[ShopManager] TrySellItem: inventoryManager is null. Assign it in the Inspector.");
//         return;
//     }

    
//     int finalPrice = itemSO.price;

//     Debug.Log($"The item name is {itemSO.itemName} and price is {finalPrice}");

//     inventoryManager.money += finalPrice;

//     if (inventoryManager.moneyText != null)
//         inventoryManager.moneyText.text = inventoryManager.money.ToString();
//     else
//         Debug.LogWarning("[ShopManager] moneyText is null; assign the TMP/Text field on the InventoryManager.");
// }

 public void RecalculateTotal()
{
    int total = 0;
    bool any = false;

    foreach (var slot in itemSlots)
    {
        if (slot != null && slot.CurrentItem != null && slot.CurrentItem.itemSO != null)
        {
            total += slot.CurrentItem.itemSO.price;
            any = true;
        }
    }

    if (totalText) totalText.text = $"Total: ${total}";
    if (sellButton) sellButton.interactable = any;
}


    // Optional: keep single-item path if you still call it
    public void TrySellItem(ItemSO itemSO, int price)
    {
        if (itemSO == null || inventoryManager == null) return;

        inventoryManager.money += price;
        if (inventoryManager.moneyText)
            inventoryManager.UpdateMoneyUI();
    }

    private bool selling; // guard against double firing

public void SellAllInSlots()
{
    if (selling) return;     // prevent double click/double listeners
    selling = true;

    Debug.Log("[Sell] Button pressed");

    // Recompute from slots so we use a fresh, correct total
    int localTotal = 0;
    var soldItems = new List<DragDrop>();

    foreach (var slot in itemSlots)
    {
        if (slot == null) continue;

        var item = slot.CurrentItem;
        if (item != null && item.itemSO != null)
        {
            Debug.Log($"[Sell] Found item {item.itemSO.itemName} ${item.itemSO.price}");
            localTotal += item.itemSO.price;
            soldItems.Add(item);
        }
    }
     Debug.Log($"[Sell] Computed localTotal = ${localTotal}");

    // Apply money once
    if (localTotal > 0 && inventoryManager != null)
    {
        inventoryManager.money += localTotal;
            inventoryManager.UpdateMoneyUI();
             Debug.Log($"[Sell] Added ${localTotal} to player's money. New balance: ${inventoryManager.money}");
}
else
{
    Debug.LogWarning("[Sell] No valid total or inventoryManager not assigned.");
}
    

    // Clear the slots once, don’t call per-item sell functions here
    foreach (var slot in itemSlots)
    {
        if (slot == null) continue;
        var item = slot.CurrentItem;
        if (item != null)
        {
            slot.ClearIfThis(item);      // remove reference from slot
            item.gameObject.SetActive(false); // or Destroy(item.gameObject); or return to shelf
        }
    }

    // Refresh UI
    RecalculateTotal(); // will show Total: $0 and disable button if you set that logic there
    selling = false;
}
}


//gives access to System namespace (same as using system at the top of script)
//Serializable means that we will be able to see this in the inspector
[System.Serializable] 

public class ShopItems{
    public ItemSO itemSO;
    public int price;
}

