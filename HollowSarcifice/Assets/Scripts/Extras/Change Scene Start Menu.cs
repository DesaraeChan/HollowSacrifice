using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ChangeSceneStartMenu : MonoBehaviour
{
    public Fading fade;

    void Start(){
        fade = FindFirstObjectByType<Fading>();
    }
    public IEnumerator StartGameFade()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Home");
        
    }

    public void StartGame(){
        StartCoroutine(StartGameFade());
    }

}
