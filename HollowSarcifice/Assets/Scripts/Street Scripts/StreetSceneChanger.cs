using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class StreetSceneChanger : MonoBehaviour
{
    public Fading fade;
    public KeyCode interactKey = KeyCode.E;
    private bool playerInRange = false;
    public Vector2 playerPosition;
    public VectorValue playerStorage;

//string for us to assign what scene to load when E pressed
    public string sceneToLoad = "Home";

    void Start()
    {
         fade = FindFirstObjectByType<Fading>();
    }


    public IEnumerator _ChangeScene()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        playerStorage.initialValue = playerPosition;
        SceneManager.LoadScene(sceneToLoad);
    }

    

   
    
    
    private void Update()
    {
        // Check for input when player is inside collider
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            StartCoroutine(_ChangeScene());
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
