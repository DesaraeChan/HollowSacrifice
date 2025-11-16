using UnityEngine;
using System.Collections.Generic;

public class StockInventory : MonoBehaviour
{
    public static StockInventory Instance { get; private set; }

    [System.Serializable]
    public class StockEntry
    {
        public ItemSO itemSO;
        public int quantity;
    }

    [SerializeField] private List<StockEntry> entries = new List<StockEntry>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddStock(ItemSO item, int amount = 1)
    {
        if (item == null) return;

        var entry = entries.Find(e => e.itemSO == item);
        if (entry == null)
        {
            entry = new StockEntry { itemSO = item, quantity = 0 };
            entries.Add(entry);
        }

        entry.quantity += amount;
    }

    public void ConsumeOne(ItemSO item)
    {
        if (item == null) return;

        var entry = entries.Find(e => e.itemSO == item);
        if (entry == null) return;

        entry.quantity--;

        if (entry.quantity <= 0)
        {
            entries.Remove(entry);  // completely removes 0-quantity entries
        }
    }

    public List<StockEntry> GetAllWithQuantity()
    {
        // only return entries with strictly positive quantity
        return entries.FindAll(e => e.quantity > 0);
    }
}
