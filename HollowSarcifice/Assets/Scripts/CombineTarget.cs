using UnityEngine;

public class CombineTarget : MonoBehaviour
{
    [SerializeField] private DragDrop host;    // DragDrop on THIS bowl
    [SerializeField] private ShopManager shop; // optional; for totals
    [SerializeField] private ItemCategory[] acceptedCategories = { }; // empty = accept any
    [SerializeField] private bool adoptIncoming = true;   // bowl becomes dragged item
    [SerializeField] private bool destroyDragged = true;  // delete dragged clone
    [SerializeField] private bool combineOnce = false;

    private RectTransform rect;
    private bool combined;

    void Awake()
    {
        if (!host) host = GetComponent<DragDrop>();
        rect = GetComponent<RectTransform>();
        Debug.Log($"[CombineTarget:{name}] Awake host={host!=null} rect={rect!=null}");
    }

    public bool TryCombine(DragDrop dragging, Camera uiCam, Vector2 pointerPos)
    {
        if (dragging == null || dragging.itemSO == null) return false;
        if (combineOnce && combined) return false;

        bool inside = RectTransformUtility.RectangleContainsScreenPoint(rect, pointerPos, uiCam);
        if (!inside) return false;

        // category filter
        if (acceptedCategories != null && acceptedCategories.Length > 0)
        {
            bool ok = false;
            foreach (var c in acceptedCategories) if (c == dragging.itemSO.category) { ok = true; break; }
            if (!ok) { Debug.Log($"[CombineTarget:{name}] Pointer on me, but category {dragging.itemSO.category} not accepted."); return false; }
        }

        Debug.Log($"[CombineTarget:{name}] COMBINE with {dragging.itemSO.itemName} (${dragging.itemSO.price})");

        if (adoptIncoming)
        {
            host.InitializeItem(dragging.itemSO, dragging.itemSO.price);
            Debug.Log($"[CombineTarget:{name}] Host now {host.itemSO.itemName} (${host.itemSO.price})");
        }

        combined = true;
        shop?.RecalculateTotal();

        if (destroyDragged) DragDrop.Consume(dragging);
        return true;
    }
}
