using UnityEngine;

public class SaleTracker : MonoBehaviour
{
    public static SaleTracker Instance;

    public int solzaeSoupCount = 0;
    public int solzaeGearCount = 0;

    [Header("Cutscene Threshold")]
    public int threshold = 7;     // number required to trigger cutscene

    public bool cutsceneTriggered = false;

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

     public int GetCount(ItemCategory category)
    {
        switch (category)
        {
            case ItemCategory.SolzaeSoup:
                return solzaeSoupCount;

            case ItemCategory.SolzaeGear:
                return solzaeGearCount;

            default:
                return 0;
        }
    }

    public void RegisterSale(ItemSO so)
    {
        if (so.category == ItemCategory.SolzaeSoup)
            solzaeSoupCount++;

        if (so.category == ItemCategory.SolzaeGear)
            solzaeGearCount++;

        //CheckCutsceneCondition();
    }

    // private void CheckCutsceneCondition()
    // {
    //     if (cutsceneTriggered) return;

    //     int total = solzaeSoupCount + solzaeGearCount;

    //     if (total >= threshold)
    //     {
    //         cutsceneTriggered = true;

    //         Debug.Log(">>> CUTSCENE SHOULD PLAY NOW!");

    //         // Example for Sleep in Day:
    //         // SceneManager.LoadScene("SolzaeCutscene");

            
    //     }
    // }

    public void AddSale(ItemSO item)
    {
        if (item == null) return;

        // Only track these categories
        if (item.category == ItemCategory.SolzaeSoup)
        solzaeSoupCount++;
        if(item.category == ItemCategory.SolzaeGear)
        solzaeGearCount++;
            
            }
}
