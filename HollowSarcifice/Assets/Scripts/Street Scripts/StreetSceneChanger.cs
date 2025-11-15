using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class StreetSceneChanger : MonoBehaviour
{
    public Fading fade;
    public KeyCode interactKey = KeyCode.E;
    private bool playerInRange = false;

    void Start()
    {
         fade = FindFirstObjectByType<Fading>();
    }


    public IEnumerator _ChangeSceneHome()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Home");
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SceneHome()
    {
        StartCoroutine(_ChangeSceneHome());
    }

    

   
    
    
    private void Update()
    {
        // Check for input when player is inside collider
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            SceneHome();
        }
        
    }

     private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
         playerInRange = true;
            
    

    }

        
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
        

    }
}
