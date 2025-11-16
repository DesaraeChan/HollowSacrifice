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
    [SerializeField] private Animator buttonanim;
    [SerializeField] private GameObject arrow;


    [Header("Special Visuals")]
    [SerializeField] private ItemSO soupItemSO;
    [SerializeField] private GameObject soupObjectToHide;
        [SerializeField] private GameObject soupObject2ToHide;

    [SerializeField] private ItemSO solzaeItemSO;
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private Sprite bgWithoutSolzaeSprite;
    [SerializeField] private Sprite bgWithSolzaeSprite; // optional if you want both states


    

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

private void UpdateSpecialVisualsFromStock()
{
    if (StockInventory.Instance == null) return;

    bool hasSoup = false;
    bool hasSolzae = false;

    // Check based on the *stock inventory*, which drives the shop
    foreach (var entry in StockInventory.Instance.GetAllWithQuantity())
    {
        if (entry.itemSO == null) continue;

        if (entry.itemSO == soupItemSO)
            hasSoup = true;

        if (entry.itemSO == solzaeItemSO)
            hasSolzae = true;
    }

    // 1) Hide/show the soup-related object
    if (soupObjectToHide && soupObject2ToHide!= null)
        soupObjectToHide.SetActive(hasSoup); // hide if no soup
         soupObject2ToHide.SetActive(hasSoup); // hide if no soup

    // 2) Swap background sprite if Solzae not present
    if (backgroundRenderer != null)
    {
        if (!hasSolzae && bgWithoutSolzaeSprite != null)
        {
            backgroundRenderer.sprite = bgWithoutSolzaeSprite;
        }
        else if (hasSolzae && bgWithSolzaeSprite != null)
        {
            backgroundRenderer.sprite = bgWithSolzaeSprite;
        }
    }
}

public void RefreshShopFromStock()
    {
        shopItems.Clear();

        if (StockInventory.Instance == null)
        {
            Debug.LogWarning("<color=yellow>[ShopManager]</color> No StockInventory.Instance found. Shop will be empty.");
            PopulateShopItems();
            return;
        }

        var allStock = StockInventory.Instance.GetAllWithQuantity();

        Debug.Log($"<color=cyan>[ShopManager]</color> RefreshShopFromStock: Stock entries count = {allStock.Count}");

        foreach (var entry in allStock)
        {
            if (entry == null || entry.itemSO == null)
            {
                Debug.LogWarning("<color=yellow>[ShopManager]</color> Found null entry or null itemSO in stock.");
                continue;
            }

            if (entry.quantity <= 0)
            {
                Debug.Log($"<color=magenta>[ShopManager]</color> Skipping {entry.itemSO.name} because quantity <= 0 ({entry.quantity}).");
                continue;
            }

            var so = entry.itemSO;
            Debug.Log($"<color=green>[ShopManager]</color> Adding shop item from stock: {so.name} x{entry.quantity}, sell price {so.price}");

            shopItems.Add(new ShopItems
            {
                itemSO = so,
                price = so.price
            });
        }

        PopulateShopItems();
    }


   private void Start(){
    bool any = false;
    arrow.SetActive(false);
    buttonanim.SetBool("Sellable", false);

    DayManager.Instance.Night = true;
     

       RefreshShopFromStock();
    UpdateSpecialVisualsFromStock();

    //   UpdateSpecialVisualsFromStock();
    // RefreshShopFromStock();
 

     if (sellButton) sellButton.interactable = any;
     //stop button anim
        

     //owner = FindObjectsOfType<CharacterManager>(); 

//this adds the text to the shop items

//    PopulateShopItems();
   }

   public void PopulateShopItems(){
    for (int i=0; i< shopItems.Count && i < shopSlots.Length; i++){
        ShopItems shopItem = shopItems[i];
        //each item knows its own price here
        shopSlots[i].InitializeItem(shopItem.itemSO, shopItem.itemSO.price);
       // shopSlots[i].InitializeItem(shopItem.itemSO, shopItem.itemSO.price);
        shopSlots[i].gameObject.SetActive(true);


    }

    for (int i = shopItems.Count; i < shopSlots.Length; i++){
        shopSlots[i].gameObject.SetActive(false);
    }
   }


private void BuildShopItemsFromStock()
{
    shopItems.Clear();

    if (StockInventory.Instance == null) return;

    var allStock = StockInventory.Instance.GetAllWithQuantity();
    foreach (var entry in allStock)
    {
        var so = entry.itemSO;
        if (so == null) continue;

        // Use the SELL price from normal ItemSO
        var shopItem = new ShopItems
        {
            itemSO = so,
            price  = so.price   // assuming ItemSO has "price"
        };

        shopItems.Add(shopItem);
    }
}


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
    //play button seel anim here
    if (arrow) arrow.SetActive(anySellable);
      buttonanim.SetBool("Sellable", anySellable);

}


   


    // Optional: keep single-item path if you still call it
    public void TrySellItem(ItemSO itemSO, int price)
    {
        if (itemSO == null || inventoryManager == null) return;
        if (!IsSellable(itemSO)) return;     

        inventoryManager.countermoney.money += price;
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

    var soldItems      = new List<DragDrop>(); // sellable
    var toClearBadOnes = new List<(ItemSlot slot, DragDrop item)>(); // unsellable

    // 1) Scan slots
    foreach (var slot in itemSlots)
    {
        if (slot == null) continue;

        var item = slot.CurrentItem;
        if (item == null || item.itemSO == null) continue;

        if (IsSellable(item.itemSO))
        {
            localTotal += item.itemSO.price;
            soldItems.Add(item);
        }
        else
        {
            // mark for clearing only if we end up selling at least one thing
            toClearBadOnes.Add((slot, item));
           
        }
    }

    // Nothing sellable? Do nothing (don’t clear unsellables)
    if (soldItems.Count == 0)
    {
        selling = false;
        return;
       
    }

    // 2) Apply money once
    if (inventoryManager != null && localTotal > 0)
    {
        inventoryManager.countermoney.money += localTotal;
        inventoryManager.UpdateMoneyUI();
    }

    // 3) Reputation / follow-up node based only on sold items
    var profile = owner != null ? owner.CurrentNPC : null;
    if (profile != null)
    {
        foreach (var sold in soldItems)
        {
            var so = sold.itemSO;
            if (so == null) continue;

            if (profile.TryGetPref(so.category, out var pref))
            {
                repSum += pref.repDelta;
                if (!string.IsNullOrEmpty(pref.nextNode))
                    chosenNode = pref.nextNode; // last hit wins; adjust if you prefer first/most-extreme, etc.
            }
        }
    }

    if (owner != null)
        owner.OnItemsSoldResult(repSum, chosenNode);

    // 4) Clear sold items
    foreach (var sold in soldItems)
    {

         // Tell SaleTracker what was sold
        if (SaleTracker.Instance != null)
        {
            SaleTracker.Instance.AddSale(sold.itemSO);
        } 
        var slot = sold.CurrentItemSlot;
        if (slot != null) slot.ClearIfThis(sold);
        sold.gameObject.SetActive(false);
    }

    // 5) Clear unsellables ONLY because a sale occurred (mixed case)
    foreach (var tuple in toClearBadOnes)
    {
        var slot = tuple.slot;
        var bad  = tuple.item;
        if (slot != null) slot.ClearIfThis(bad);
        bad.gameObject.SetActive(false);
    }

    // 6) Refresh UI
    RecalculateTotal();
    selling = false;
    arrow.SetActive(false);
    buttonanim.SetBool("Sellable", false);
}

}


//gives access to System namespace (same as using system at the top of script)
//Serializable means that we will be able to see this in the inspector
[System.Serializable] 

public class ShopItems{
    public ItemSO itemSO;
    public int price;
}

