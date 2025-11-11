using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragCombine : MonoBehaviour
{
    [Header("Host (this object)")]
    [SerializeField] private DragDrop host;             // DragDrop on THIS object
   // [SerializeField] private ItemSlot hostSlot;         // Optional: slot the host lives in

    [Header("Managers")]
    [SerializeField] private ShopManager shop;          // Recalculate totals after combine

    [Header("Rules")]
    [Tooltip("If empty, accept any incoming category; otherwise only these categories combine.")]
    [SerializeField] private ItemCategory[] acceptedCategories = { };
    [SerializeField] private bool requireRigidbodyOnIncoming = true;
    [SerializeField] private bool destroyOtherOnCombine = true;
    [SerializeField] private bool allowMultipleCombines = false;  // set false to only combine once

    [Header("Result Mode")]
    [SerializeField] private bool adoptIncoming = true; // if true, host becomes incoming.itemSO
    [SerializeField] private ItemSO fallbackResultSO;   // optional: if no recipe/adopt, use this

    [System.Serializable]
    public struct Combinations
    {
        public ItemSO hostBefore;    // optional (leave null to match any host)
        public ItemSO incoming;      // incoming required (e.g., Soup ingredient)
        public ItemSO result;        // the ItemSO the host becomes
    }
    [SerializeField] private Combinations[] combos = { };

    private bool combinedOnce;

    void Awake()
    {
        // if (!host) host = GetComponent<DragDrop>();
        // if (!hostSlot) hostSlot = GetComponentInParent<ItemSlot>();

        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!allowMultipleCombines && combinedOnce) return;

        var incoming = other.GetComponent<DragDrop>();
        if (incoming == null || incoming.itemSO == null) return;

        // Optional: make sure incoming has Rigidbody2D (required by Unity triggers if host has none)
        if (requireRigidbodyOnIncoming && incoming.GetComponent<Rigidbody2D>() == null) return;

        if (!IsAccepted(incoming.itemSO.category)) return;

        // Decide result ItemSO
        var result = ResolveResultSO(host?.itemSO, incoming.itemSO);
        if (result == null) return;

        // Swap host to result (updates sprite/name/price)
        // Your DragDrop has InitializeItem(ItemSO, int)
        host.InitializeItem(result, result.price);

        // Keep slot bookkeeping consistent (so ShopManager counts it)
        // if (hostSlot != null)
        // {
        //     hostSlot.CurrentItem = host;
        //     host.CurrentItemSlot = hostSlot;
        // }

        if (destroyOtherOnCombine)
            Destroy(incoming.gameObject);

        shop?.RecalculateTotal();

        combinedOnce = true;
    }

    private bool IsAccepted(ItemCategory cat)
    {
        if (acceptedCategories == null || acceptedCategories.Length == 0) return true;
        for (int i = 0; i < acceptedCategories.Length; i++)
            if (acceptedCategories[i] == cat) return true;
        return false;
    }

    private ItemSO ResolveResultSO(ItemSO hostBefore, ItemSO incoming)
    {
        // 1) Try comnbos first (if any)
        for (int i = 0; i < combos.Length; i++)
        {
            bool hostMatches = combos[i].hostBefore == null || combos[i].hostBefore == hostBefore;
            if (hostMatches && combos[i].incoming == incoming)
                return combos[i].result;
        }

        // 2) Adopt incoming (host becomes incoming)
        if (adoptIncoming) return incoming;

        // 3) Fallback result (explicit)
        return fallbackResultSO;
    }
}
