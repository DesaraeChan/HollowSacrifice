using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;   // assign in Inspector
    [SerializeField] private ShopManager shopManager;
    public RectTransform Rect { get; private set; }

   private CharacterManager Owner =>  CharacterManager.Active; //global active manager

    private CanvasGroup cg;
    
    //position of obj before you touch it
    private Vector2 homePos;

    // set by ItemSlot
    public bool WasDropped { get; set; }
    public RectTransform CurrentSlot { get; set; }

    public ItemSlot CurrentItemSlot { get; set; }

    //item stats stuff
    public ItemSO itemSO;
    public TMP_Text itemNameText;
    public TMP_Text itempriceText;
    public Image itemImage;

   

    
    private int itemprice;


    public void InitializeItem(ItemSO newItemSO, int itemprice){
        //fill the slot with new info
        itemSO = newItemSO;
        itemImage.sprite = itemSO.icon;
        itemNameText.text = itemSO.itemName.ToString();
        itempriceText.text = itemSO.price.ToString();

      

    }

    public void OnSellButtonClicked(){
        //when sell button is clicked, destroy objects placed onto slots
        // update player's money value
        //play the customer's last line of dialogue

       if(!shopManager || !itemSO){
        return;
       }
       shopManager.TrySellItem(itemSO, itemSO.price);
       shopManager.SellAllInSlots();
       

    }

   private void Start()
    {
        // Initialize the UI text when the game starts
        if (itemprice != null)
        {
            itempriceText.text = itemSO.price.ToString();
        }
        else
        {
            Debug.LogWarning("[DragDrop] itemprice is not assigned in the Inspector!");
        }

      
        if (itemSO.itemName != null)
        {
            itemNameText.text = itemSO.itemName.ToString();
        }
        else
        {
            Debug.LogWarning("[DragDrop] itemname is not assigned in the Inspector!");
        }
    }

    void Awake()
    {
        Rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        if (!cg) cg = gameObject.AddComponent<CanvasGroup>();

        //record original pos of obj
        homePos = Rect.anchoredPosition;

        // Fallback: auto-find an active CharacterManager if none wired
       // if (!Owner) Owner = FindFirstObjectByType<CharacterManager>();
    }

    public void OnBeginDrag(PointerEventData e)
    {

        if (Owner == null || !Owner.IsInAskPhase)
            return;
 
        WasDropped = false;
        cg.blocksRaycasts = false;   // allows slot to detect item/ item to get grabbed

        if (CurrentItemSlot != null){
            CurrentItemSlot.ClearIfThis(this);
            CurrentItemSlot = null;
        }
    }
    public void OnDrag(PointerEventData e)
    {
        if (Owner == null || !Owner.IsInAskPhase)
            return;

        Rect.anchoredPosition += e.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData e)
    {
        cg.blocksRaycasts = true;    //prevents clicking/further interaction with obj when being dragged

        if (!WasDropped)
        {
            // not dropped on a slot, then go back to og spot
            Rect.anchoredPosition = homePos;
            CurrentSlot = null;
        }
    }

}
