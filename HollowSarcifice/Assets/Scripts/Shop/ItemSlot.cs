using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Canvas canvas; // same canvas as UI

    [SerializeField] private ShopManager shopManager;


// CurrentItem points to what item is currently in the slot
//{get; private set;}  makes this a property instead of a plain variable
// get=public, other scripts can read it
//set = private, only this script can assign it
 public DragDrop CurrentItem { get; private set; }


    public void OnDrop(PointerEventData e)
    {
        //pointerDrag is the obj being dragged, when mouse relased over slot call OnDrop()
        var go = e.pointerDrag;
        if (!go) return;

        if(go.CompareTag("NotSlottable")) //if not able to slot, return
        return;

    //does obj being dragged have DragDrop script attached, if so access it's data
        var dragItem = go.GetComponent<DragDrop>();
        if (!dragItem) return;

var slotRect = (RectTransform)transform;
var itemRT   = dragItem.Rect;
var parentRT = itemRT.parent as RectTransform;

Camera uiCam = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera;

// 1) World center of the slot rect
Vector3 worldCenter = slotRect.TransformPoint(slotRect.rect.center);

// 2) Convert that world point to a screen point (for ScreenPointToLocalPointInRectangle)
Vector2 screenPt = RectTransformUtility.WorldToScreenPoint(uiCam, worldCenter);

// 3) Convert to the parent’s local space
Vector2 localPt;
RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, screenPt, uiCam, out localPt);

// 4) Ensure the item’s pivot is centered, then place its anchoredPosition
itemRT.pivot = new Vector2(0.5f, 0.1f);
itemRT.anchoredPosition = localPt;

        // Get slot center in screen space
    //     var slotRect = (RectTransform)transform;
    //      var itemRT = dragItem.Rect;

    //     // itemRT.SetParent(slotRect, worldPositionStays: false); //this makes itema child of the slot

    //     Camera uiCam = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera;
    // //get pos in world space
    //     Vector3 worldCenter = slotRect.TransformPoint(slotRect.rect.center);
    //     //Vector2 screenCenter = RectTransformUtility.WorldToScreenPoint(uiCam, worldCenter);
    //     itemRT.pivot = new Vector2(0.5f, 0.5f);
    //     itemRT.position = worldCenter; 
    //     // // Convert to item's parent local space
        // RectTransform parentRect = dragItem.Rect.parent as RectTransform;
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenCenter, uiCam, out var localPoint);

        // // Snap by position only 
        // dragItem.Rect.anchoredPosition = localPoint;

    //Sets wasdropped to true so obj snaps to slot and not home pos
        dragItem.WasDropped  = true;
        // records which slot the item is in
        dragItem.CurrentItemSlot = this;

        CurrentItem = dragItem;

        if (dragItem.itemSO){
            Debug.Log($" {dragItem.itemSO.itemName} (${dragItem.itemSO.price})");
        }

        if (shopManager){
            shopManager.RecalculateTotal();
        } 
    }

    public void ClearIfThis(DragDrop dragitem){
        if (CurrentItem == dragitem){
            CurrentItem = null;
            if(shopManager) shopManager.RecalculateTotal();
        }
    }
}
