using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    public Fading fade;

    void Start()
    {
         fade = FindFirstObjectByType<Fading>();
    }


    public IEnumerator _ChangeScene()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Hypothetical2DWalking");
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoToSceneTwo()
    {
        StartCoroutine(_ChangeScene());
    }
}
