using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;
    public int currentDay = 1;
    public bool Night = false;
    //False = day - True = Night 
    public bool unlockDay = false;

    public bool newsActive = true;

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
        unlockDay = true;
        currentDay++;
        Night = false;
        Debug.Log("Day advanced to: " + currentDay);

        FindFirstObjectByType<ShowNews>(FindObjectsInactive.Include).OpenNews();
        FindFirstObjectByType<CutsceneStarter>(FindObjectsInactive.Include).restart();
        FindFirstObjectByType<ShowWindowImage>(FindObjectsInactive.Include).UpdateVisuals();
        FindFirstObjectByType<OpenMoneyMenu>(FindObjectsInactive.Include).CloseFinancial();;
    }

    public void UnlockNextDay()
    {
        unlockDay = false;
    }
}