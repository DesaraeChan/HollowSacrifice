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

    [Header("Rules")]
    [SerializeField] private ItemCategory[] NoSellCategory = {ItemCategory.Bowl, ItemCategory.Solzae, ItemCategory.Glass };// set special non sellable item herer (bowl, glass)

// true only if NOT one of the non-sellable categories
private bool IsSellable(ItemSO so)
{
    if (so == null) return false;
    foreach (var cat in NoSellCategory)
        if (so.category == cat) return false;
    return true;
}


    private CharacterManager owner;
    public void Initialize(CharacterManager npcOwner)
{
    owner = npcOwner;

    
}



   private void Start(){
    bool any = false;

     if (sellButton) sellButton.interactable = any;

     //owner = FindObjectsOfType<CharacterManager>(); 

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



//  public void RecalculateTotal()
// {
//     int total = 0;
//     bool any = false;

//     foreach (var slot in itemSlots)
//     {
//         if (slot != null && slot.CurrentItem != null && slot.CurrentItem.itemSO != null)
//         {
//             if(IsSellable(so)){
//                 total += slot.CurrentItem.itemSO.price;
//                 any = true;

//             }
            
//         }
// //     }
//  if (totalText) totalText.text = $"Total: ${total}";
//     if (sellButton) sellButton.interactable = any;
// }

public void RecalculateTotal()
{
    int total = 0;
    bool anySellable = false;

    foreach (var slot in itemSlots)
    {
        if (slot == null) continue;

        var item = slot.CurrentItem;
        if (item == null || item.itemSO == null) continue;

        var so = item.itemSO; 
        if (IsSellable(so))   // ignore bowls
        {
            total += slot.CurrentItem.itemSO.price;
            anySellable = true;
        }
    }

    if (totalText) totalText.text = $"Total: ${total}";
    if (sellButton) sellButton.interactable = anySellable;
}


   


    // Optional: keep single-item path if you still call it
    public void TrySellItem(ItemSO itemSO, int price)
    {
        if (itemSO == null || inventoryManager == null) return;
        if (!IsSellable(itemSO)) return;     

        inventoryManager.money += price;
        if (inventoryManager.moneyText)
            inventoryManager.UpdateMoneyUI();
    }

    private bool selling; // guard against double firing

    public void SellAllInSlots()
{
    if (selling) return;
    selling = true;

    int localTotal = 0;
    int repSum = 0;
    string chosenNode = null;

    var soldItems = new List<DragDrop>();

    // 1) Gather sold items and total price
    foreach (var slot in itemSlots)
    {
        if (slot == null) continue;

        var item = slot.CurrentItem;
        if (item == null || item.itemSO == null) continue;
        if (!IsSellable(item.itemSO)) return;     

        localTotal += item.itemSO.price;
        soldItems.Add(item);
        Debug.Log($"[Sell] Found item {item.itemSO.itemName} ${item.itemSO.price}");
    }

    Debug.Log($"[Sell] Computed localTotal = ${localTotal}");


    // 2) Apply money once
    if (localTotal > 0 && inventoryManager != null)
    {
        inventoryManager.money += localTotal;
        inventoryManager.UpdateMoneyUI();
        Debug.Log($"[Sell] Added ${localTotal}. New balance: ${inventoryManager.money}");
    }
    else
    {
        Debug.LogWarning("[Sell] No valid total or inventoryManager not assigned.");
    }

    // 3) Compute reputation impact per item (no reflection)
    var profile = owner != null ? owner.CurrentNPC : null;
    if (profile != null)
    {
        foreach (var sold in soldItems)
        {
            var so = sold.itemSO;                 // per-item SO
            if (so == null) continue;

            if (profile.TryGetPref(so.category, out var pref))
            {
                repSum += pref.repDelta;

                // optional: last matching item decides node (tweak if you want a different rule)
                if (!string.IsNullOrEmpty(pref.nextNode))
                    chosenNode = pref.nextNode;

                Debug.Log($"[Sell] Pref hit for {so.itemName} ({so.category}): repΔ={pref.repDelta}, nextNode='{pref.nextNode}'");
            }
        }
    }

    // 4) Tell the CharacterManager the result (this applies rep and picks the node)
    if (owner != null)
    {
        owner.OnItemsSoldResult(repSum, chosenNode);
    }

    // 5) Clear slots / hide items
    foreach (var slot in itemSlots)
    {
        if (slot == null) continue;
        var item = slot.CurrentItem;
        if (item != null)
        {
            slot.ClearIfThis(item);
            item.gameObject.SetActive(false);
        }
    }

    // 6) Refresh UI (disables button if total == 0)
    RecalculateTotal();

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

