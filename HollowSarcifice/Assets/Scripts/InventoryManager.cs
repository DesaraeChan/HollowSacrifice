using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Player Money")]
    public int money;                     // the actual player money value
    public TMP_Text moneyText;            // UI text showing the money

    private void Start()
    {
        // Initialize the UI text when the game starts
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
        else
        {
            Debug.LogWarning("[InventoryManager] moneyText is not assigned in the Inspector!");
        }
    }

    public void UpdateMoneyUI()
    {
        // Call this anytime the money value changes
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
    }
}
