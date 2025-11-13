using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Scene/Managers")]
    [SerializeField] private Canvas canvas;               // Assign in Inspector (fallbacks provided)
    [SerializeField] private ShopManager shopManager;
    private CharacterManager Owner => CharacterManager.Active;

    [Header("Duplication")]
    [Tooltip("If true, this object acts as a TEMPLATE: begin drag will spawn a clone and drag the clone.")]
    [SerializeField] private bool spawnCloneOnDrag = true;
    [Tooltip("Optional: parent for dragged clones (a top-level UI layer under the Canvas).")]
    [SerializeField] private RectTransform dragLayer;

    [Header("UI Refs")]
    public ItemSO itemSO;
    public TMP_Text itemNameText;
    public TMP_Text itempriceText;
    public Image itemImage;

    [Header("Runtime")]
    public RectTransform Rect { get; private set; }
    public bool WasDropped { get; set; }
    public RectTransform CurrentSlot { get; set; }
    public ItemSlot CurrentItemSlot { get; set; }

    private CanvasGroup cg;
    private Vector2 homePos;
    private bool isDragging;
    private static DragDrop itemBeingDragged; // the active dragged instance (clone)

 
    private int itemprice;  
    public static Vector2 PointerPos { get; private set; }     // add this
    public static DragDrop CurrentDragging => itemBeingDragged; // already have itemBeingDragged; just expose


    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        if (!cg) cg = gameObject.AddComponent<CanvasGroup>();
        homePos = Rect.anchoredPosition;

        if (!canvas)
        {
            canvas = GetComponentInParent<Canvas>();
            if (!canvas)
            {
                var tagged = GameObject.FindGameObjectWithTag("UI Canvas");
                if (tagged) canvas = tagged.GetComponent<Canvas>();
            }
        }
    }

    private void Start()
    {
        // If itemSO is assigned in Inspector, push to UI immediately
        ApplyItemVisuals();
    }



    public void InitializeItem(ItemSO newItemSO, int itemprice)
    {
         //fill the slot with new info 
        itemSO = newItemSO; 
        itemImage.sprite = itemSO.icon; 
        // itemNameText.text = itemSO.itemName.ToString(); 
        // itempriceText.text = itemSO.price.ToString(); 

        ApplyItemVisuals();
        }
   
    // public void InitializeItem(ItemSO newItemSO, int price)
    // {
    //     itemSO = newItemSO;
    //     ApplyItemVisuals();
    // }

    private void ApplyItemVisuals()
    {
        if (itemSO == null) return;

        if (itemImage)     itemImage.sprite = itemSO.icon;
        if (itemNameText)  itemNameText.text = itemSO.itemName;
        if (itempriceText) itempriceText.text = itemSO.price.ToString();

        itemNameText?.ForceMeshUpdate();
        itempriceText?.ForceMeshUpdate();
    }

   

    public void OnSellButtonClicked()
    {
        if (!shopManager || !itemSO) return;

        shopManager.TrySellItem(itemSO, itemSO.price);
        shopManager.SellAllInSlots();
    }


public void OnBeginDrag(PointerEventData e)
{
    if (Owner == null || !Owner.IsInAskPhase) return;

    if (spawnCloneOnDrag)
    {
        var parent = dragLayer ? dragLayer :
                     (canvas ? canvas.transform as RectTransform : (RectTransform)transform.parent);

        var cloneGO = Instantiate(gameObject, parent);
        var clone   = cloneGO.GetComponent<DragDrop>();

        // clone config
        clone.spawnCloneOnDrag = false;
        clone.InitializeItem(itemSO, itemprice);

        // size/position
        var srcRT = Rect;
        var dstRT = clone.Rect;
        dstRT.sizeDelta = srcRT.sizeDelta;
        dstRT.position  = srcRT.position;

        // make the dragged clone not block raycasts while dragging
        var cloneCG = clone.GetComponent<CanvasGroup>() ?? clone.gameObject.AddComponent<CanvasGroup>();
        cloneCG.blocksRaycasts = false;

        // ---- HAND THE DRAG TO THE CLONE (the key bit) ----
        itemBeingDragged = clone;
        clone.isDragging = true;

        // tell the EventSystem the clone is now the thing being dragged
        e.pointerDrag  = cloneGO;
        e.pointerPress = cloneGO;
        EventSystem.current.SetSelectedGameObject(cloneGO);

        // optional: make sure the template doesn't hijack raycasts while you drag
        // (leave it interactable normally; we just don't want this drag to hit it)
        foreach (var g in GetComponentsInChildren<Graphic>())
            g.raycastTarget = true; // keep template clickable for next time

        return;
    }

    // clone path (dragging an already-spawned item)
    WasDropped = false;
    isDragging = true;
    cg.blocksRaycasts = false;

    if (CurrentItemSlot != null)
    {
        CurrentItemSlot.ClearIfThis(this);
        CurrentItemSlot = null;
    }
}

// private void TryUICombine()
// {
//     var dragging = spawnCloneOnDrag ? itemBeingDragged : this;
//     if (dragging == null) return;

//     // super simple: scan all CombineTarget in scene (fine for a few targets)
//     var targets = Object.FindObjectsOfType<CombineTarget>(true);
//     var cam = canvas ? canvas.worldCamera : null;

//     for (int i = 0; i < targets.Length; i++)
//     {
//         if (targets[i] != null && targets[i].TryCombine(dragging, cam))
//             break; // combined; stop
//     }
// }


    public void OnDrag(PointerEventData eventData)
    {
        // move whatever is currently being dragged
        if (spawnCloneOnDrag)
        {
            if (itemBeingDragged != null)
                itemBeingDragged.Rect.position = eventData.position;
        }
        else
        {
            Rect.position = eventData.position;
        }

        PointerPos = eventData.position;
          CombineScan.Try(PointerPos, spawnCloneOnDrag ? itemBeingDragged : this, canvas);
    }

    public static void Consume(DragDrop d)
{
    if (itemBeingDragged == d) itemBeingDragged = null;
    if (d != null) Object.Destroy(d.gameObject);
}



    public void OnEndDrag(PointerEventData e)
    {
        // TEMPLATE path: finalize clone
        if (spawnCloneOnDrag)
        {
            
            if (itemBeingDragged != null)
            {
                bool isUnslottable = itemBeingDragged.CompareTag("NotSlottable"); //tag for not slottable item
                // If a slot didn’t mark it as dropped, destroy the clone
                if (isUnslottable || !itemBeingDragged.WasDropped)
                {
                    Destroy(itemBeingDragged.gameObject);
                }
                else
                {
                    // Dropped successfully: restore raycasts to interactable
                    var cloneCG = itemBeingDragged.GetComponent<CanvasGroup>();
                    if (cloneCG) cloneCG.blocksRaycasts = true;
                }
                itemBeingDragged.isDragging = false;
                itemBeingDragged = null;
            }
            return;
        }

        // CLONE path: finish drag on this instance
        isDragging = false;
        cg.blocksRaycasts = true;

        if (!WasDropped)
        {
            // Not dropped on a valid slot, revert / destroy clone
            // If you prefer reverting to home instead of destroying, swap these lines:
            // Rect.anchoredPosition = homePos;
            // CurrentSlot = null;
            Destroy(gameObject);
            return;
        }
    }
}
