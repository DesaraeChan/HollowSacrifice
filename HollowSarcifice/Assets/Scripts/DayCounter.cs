using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;
    public int currentDay = 1;
    public bool unlockDay = false; 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NextDay()
    {
        if (unlockDay)
        {
            Debug.Log("Its time to go to work");
            return;
        }
        unlockDay = true;
        currentDay++;

        Debug.Log("Day advanced to: " + currentDay);

   
        FindFirstObjectByType<ShowWindowImage>(FindObjectsInactive.Include).UpdateVisuals();
    }

    public void UnlockNextDay()
    {
        unlockDay = false;
    }
}