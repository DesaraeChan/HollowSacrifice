using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;
    public int currentDay = 1;
    public bool Night = false;
    //False = day - True = Night 
    public bool unlockDay = false;
    public int alleyInteractions = 0;
    public bool newsActive = true;
    public bool homeOrwork = false; //home = false, work = true
    public bool FinalSequence = false;
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
        if(currentDay == 5){
            SceneManager.LoadScene("EndCutscene");
        }
        Night = false;
        newsActive = true;
        Debug.Log("Day advanced to: " + currentDay);

        //Clear stock
        if (StockInventory.Instance != null){
            StockInventory.Instance.ClearAllStock();
        }

        FindFirstObjectByType<CutsceneStarter>(FindObjectsInactive.Include).restart();
        FindFirstObjectByType<ShowWindowImage>(FindObjectsInactive.Include).UpdateVisuals();
        FindFirstObjectByType<OpenMoneyMenu>(FindObjectsInactive.Include).CloseFinancial();;
    }

    public void UnlockNextDay()
    {
        unlockDay = false;
    }

    public IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}