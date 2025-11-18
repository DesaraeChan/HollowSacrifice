using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopSlotStock : MonoBehaviour
{
    public ItemSOStock itemSO;
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public Image itemImage;
     public Button buyButton;
    
    [SerializeField] private ShopManagerStock shopManager;
    public int price;

    // void Update()
    // {
    //     UpdateButtonState();
    //     //This is hella in efficent but i think its good right now for bug fixing purposes
    // }
    public void Initialize(ItemSOStock newItemSO, int price)
    {
        itemSO = newItemSO;
        itemNameText.text = itemSO.itemName;
        this.price = price;
        priceText.text = price.ToString();
        // UpdateButtonState();
    }

    public void OnBuyButtonClicked()
{
    if (shopManager.TryBuyItem(itemSO, price))
    {
        buyButton.interactable = false;   
    }
}

    // public void UpdateButtonState()
    // {   

    //     if (MoneyCounter.Instance != null)
    //     {
    //         buyButton.interactable = MoneyCounter.Instance.money >= price;
    //     }
    // }

   
}
