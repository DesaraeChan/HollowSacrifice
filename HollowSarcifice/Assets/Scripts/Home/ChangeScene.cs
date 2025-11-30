using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
public class ChangeScene : MonoBehaviour
{
    public Fading fade;
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    public TMP_Text text;

    void Start()
    {
         fade = FindFirstObjectByType<Fading>();
    }


    public IEnumerator _ChangeScene2D()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        playerStorage.initialValue = playerPosition;
        SceneManager.LoadScene("Street");
        SoundManager.Instance.PlaySFX("HomeDoorOpen");
        
    }

    public IEnumerator _ChangeScene2DNight()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        playerStorage.initialValue = playerPosition;
        SceneManager.LoadScene("Street Night");
        
    }

    public IEnumerator _ChangeSceneDay4Shop()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("ShopSICK-DAY");
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SceneOutside()
    {
        if(!DayManager.Instance.FinalSequence){
            if(DayManager.Instance.currentDay == 4 && SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 10){
                    StartCoroutine(_ChangeSceneDay4Shop());
            } else{
            if(!DayManager.Instance.Night){
                    if(DayManager.Instance.newsActive){
                    FindFirstObjectByType<ShowNews>(FindObjectsInactive.Include).OpenNews();
                    } else {
                        StartCoroutine(_ChangeScene2D());
                    }
                      
            } else {
                    StartCoroutine(_ChangeScene2DNight());
                    
                }
        
            }
        } else {
            text.text = "I can't go outside.";
            SoundManager.Instance.PlaySFX("GoWork");
            StartCoroutine(ShowBedMessage());
        }
    }

    private IEnumerator ShowBedMessage()
        {
    yield return new WaitForSeconds(2f);   // wait 2 seconds (change if you want)
    text.text = "I think I should go to bed...";
   
        }

}
