using UnityEngine;

public class CombineTarget : MonoBehaviour
{
    [Header("Host (this object changes)")]
    [SerializeField] private DragDrop host;
    [SerializeField] private ShopManager shop;

    [Header("Recipes (A + B -> C) — exact ItemSO refs")]
    [SerializeField] private Recipe[] recipes = { };
    [System.Serializable]
    public struct Recipe
    {
        public ItemSO hostBefore;   // e.g., EmptyBowlSO, SoupSO
        public ItemSO incoming;     // e.g., SoupSO, SpiceSO
        public ItemSO result;       // e.g., SoupSO, SpicySoupSO
    }

    [Header("Filters")]
    [SerializeField] private bool requireAcceptedCategory = false;
    [SerializeField] private ItemCategory[] acceptedCategories = { }; // empty = accept all

    [Header("Behavior")]
    [SerializeField] private bool destroyDragged = true;
    [SerializeField] private bool combineOnce = false;   // set FALSE for multi-step chains
    [SerializeField] private bool verboseLogs = true;    // turn on to debug

    private RectTransform rect;
    private bool combined;

    void Awake()
    {
        if (!host) host = GetComponent<DragDrop>();
        rect = GetComponent<RectTransform>();
        if (verboseLogs)
            Debug.Log($"[CombineTarget:{name}] Awake hostSO={(host? host.itemSO?.name : "null")}");
    }

    public bool TryCombine(DragDrop dragging, Camera uiCam, Vector2 pointerPos)
    {
        if (dragging == null || dragging.itemSO == null || host == null) return false;
        if (combineOnce && combined) return false;

        if (!RectTransformUtility.RectangleContainsScreenPoint(rect, pointerPos, uiCam)) return false;

        if (requireAcceptedCategory && acceptedCategories != null && acceptedCategories.Length > 0)
        {
            bool ok = false;
            foreach (var c in acceptedCategories) if (c == dragging.itemSO.category) { ok = true; break; }
            if (!ok)
            {
                if (verboseLogs) Debug.Log($"[CombineTarget:{name}] Category {dragging.itemSO.category} not accepted.");
                return false;
            }
        }

        var before = host.itemSO;
        var incoming = dragging.itemSO;

        if (verboseLogs)
        {
            Debug.Log(
                $"[CombineTarget:{name}] Try: BEFORE='{before?.name}'#{before?.GetInstanceID()}  +  IN='{incoming.name}'#{incoming.GetInstanceID()}"
            );
        }

        ItemSO result = ResolveRecipe(before, incoming);

        if (result == null)
        {
            if (verboseLogs)
            {
                Debug.LogWarning(
                    $"[CombineTarget:{name}] No recipe matched for BEFORE='{before?.name}'#{before?.GetInstanceID()} + IN='{incoming.name}'#{incoming.GetInstanceID()}. " +
                    "Check that your recipe uses the exact same ItemSO asset instances the host and dragged item use."
                );
            }
            return false;
        }

        host.InitializeItem(result, result.price);

        if (verboseLogs)
        {
            Debug.Log(
                $"[CombineTarget:{name}] COMBINED: RESULT='{result.name}'#{result.GetInstanceID()}  (host now '{host.itemSO.name}'#{host.itemSO.GetInstanceID()})"
            );
        }

        shop?.RecalculateTotal();

        if (destroyDragged) DragDrop.Consume(dragging);
        if (combineOnce) combined = true;

        return true;
    }

    private ItemSO ResolveRecipe(ItemSO hostBefore, ItemSO incoming)
    {
        if (recipes == null) return null;

        for (int i = 0; i < recipes.Length; i++)
        {
            var r = recipes[i];
            bool hostMatch = (r.hostBefore == hostBefore);
            bool inMatch   = (r.incoming   == incoming);

            if (verboseLogs)
                Debug.Log($"[CombineTarget:{name}] Check recipe[{i}]: hostBefore='{r.hostBefore?.name}'#{r.hostBefore?.GetInstanceID()} " +
                          $"incoming='{r.incoming?.name}'#{r.incoming?.GetInstanceID()}  -> '{r.result?.name}'");

            if (hostMatch && inMatch)
                return r.result;
        }
        return null;
    }
}
