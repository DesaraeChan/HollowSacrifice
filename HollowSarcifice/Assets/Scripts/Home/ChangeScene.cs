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


    public IEnumerator _ChangeScene2D()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Street");
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SceneOutside()
    {
        StartCoroutine(_ChangeScene2D());
    }

}
