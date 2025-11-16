using UnityEngine;

public class SolzaeTrigger : MonoBehaviour
{
    public VignettePulse vignettePulse;   // assign in Inspector

    void Start()
    {
        if (SaleTracker.Instance == null)
        {
            Debug.LogWarning("No SaleTracker found in scene!");
            return;
        }

        int soupCount = SaleTracker.Instance.GetCount(ItemCategory.SolzaeSoup);
        int gearCount = SaleTracker.Instance.GetCount(ItemCategory.SolzaeGear);
        int combined  = soupCount + gearCount;

        if (combined >= 4)
        {
            // whatever else you already do (animations etc)
            if (vignettePulse != null)
                vignettePulse.PlayPulse();
        }
    }
}
