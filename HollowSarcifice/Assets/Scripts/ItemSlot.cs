using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Canvas canvas; // same canvas as UI

    public void OnDrop(PointerEventData e)
    {
        //pointerDrag is the obj being dragged, when mouse relased over slot call OnDrop()
        var go = e.pointerDrag;
        if (!go) return;

    //does obj being dragged have DragDrop script attached, if so access it's data
        var drag = go.GetComponent<DragDrop>();
        if (!drag) return;

        // Get slot center in screen space
        var slotRect = (RectTransform)transform;
        Camera uiCam = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera;
    //get pos in world space
        Vector3 worldCenter = slotRect.TransformPoint(slotRect.rect.center);
        Vector2 screenCenter = RectTransformUtility.WorldToScreenPoint(uiCam, worldCenter);

        // Convert to item's parent local space
        RectTransform parentRect = drag.Rect.parent as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenCenter, uiCam, out var localPoint);

        // Snap by position only 
        drag.Rect.anchoredPosition = localPoint;

    //Sets wasdropped to true so obj snaps to slot and not home pos
        drag.WasDropped  = true;
        // records which slot the item is in
        drag.CurrentSlot = slotRect;
    }
}
