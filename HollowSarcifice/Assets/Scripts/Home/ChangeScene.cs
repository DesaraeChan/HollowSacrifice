using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    public Fading fade;
    public Vector2 playerPosition;
    public VectorValue playerStorage;

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
        if(DayManager.Instance.currentDay == 4 && SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 10){
                StartCoroutine(_ChangeSceneDay4Shop());
        } else{
        if(!DayManager.Instance.Night){
                  
                  FindFirstObjectByType<ShowNews>(FindObjectsInactive.Include).OpenNews();
        } else {
                 StartCoroutine(_ChangeScene2DNight());
             }
       
        }
        
    }

}
