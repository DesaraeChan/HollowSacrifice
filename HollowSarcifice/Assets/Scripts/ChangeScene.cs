using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoToSceneTwo()
    {
        SceneManager.LoadScene("Hypothetical2DWalking");
    }
}
