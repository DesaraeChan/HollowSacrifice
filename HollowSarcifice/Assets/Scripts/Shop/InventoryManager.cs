using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Player Money")]
    public int money;
    public MoneyCounter countermoney;                     // the actual player money value
    public TMP_Text moneyText;            // UI text showing the money

    private void Start()
    {
        // Initialize the UI text when the game starts
        if (moneyText != null)
        {
            moneyText.text = countermoney.money.ToString();
        }
        else
        {
            Debug.LogWarning("[InventoryManager] moneyText is not assigned in the Inspector!");
        }
    }

    void Awake()
{
    //so money persists across scenes but no duplicate inventory managers
    //if this doesn't work can consider a MoneySO
    var existing = FindObjectsOfType<InventoryManager>();
    if (existing.Length > 1)
    {
        Destroy(gameObject);
        return;
    }

    DontDestroyOnLoad(gameObject);

     if (countermoney == null)
    countermoney = MoneyCounter.Instance;
}


    public void UpdateMoneyUI()
    {
        // Call this anytime the money value changes
        if (moneyText != null)
        {
            moneyText.text = countermoney.money.ToString();
        }
    }
}
