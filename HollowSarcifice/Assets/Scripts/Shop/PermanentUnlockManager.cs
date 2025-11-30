using UnityEngine;

public class PermanentUnlockManager : MonoBehaviour
{
    

    [Header("Object permanently visible after unlock")]
    [SerializeField] private GameObject homelessPermanentObject;

    void Start()
    {   
        
        RefreshUnlockState();
        if(DayManager.Instance.currentDay == 2){
            homelessPermanentObject.SetActive(false);
        }
        
            
    }

    public void ActivateHomelessObject()
    {
        if (homelessPermanentObject != null)
            homelessPermanentObject.SetActive(true);

        // Save permanently
        DecisionTracker.Instance.SetChoice("Homeless_Unlock", 1);
    }

    public void RefreshUnlockState()
    {
        // When scenes load, check stored state
        if (DecisionTracker.Instance.TryGetChoice("Homeless_Unlock", out int unlock) && unlock == 1)
        {
            if (homelessPermanentObject != null)
                homelessPermanentObject.SetActive(true);
        }
    }
}
