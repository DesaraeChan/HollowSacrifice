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
        SceneManager.LoadScene("Street");
        
    }

    public IEnumerator _ChangeScene2DNight()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        playerStorage.initialValue = playerPosition;
        SceneManager.LoadScene("Street Night");
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SceneOutside()
    {
        if(!DayManager.Instance.Night){
            StartCoroutine(_ChangeScene2D());
        } else{
            StartCoroutine(_ChangeScene2DNight());
        }
       
    }

}
