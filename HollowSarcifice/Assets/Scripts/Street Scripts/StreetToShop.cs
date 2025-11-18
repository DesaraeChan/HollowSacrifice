using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class StreetToShop : MonoBehaviour
{
    public Fading fade;
    public KeyCode interactKey = KeyCode.E;
    private bool playerInRange = false;
    public bool hasInventory = false;

    void Start()
    {
         fade = FindFirstObjectByType<Fading>();
    }


    public IEnumerator _ChangeSceneShop()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
         SceneManager.LoadScene("Shop-DAY2");

        //FIX LATER FOR PROPER DAYS
        //SceneManager.LoadScene("Shop-DAY1");
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SceneShop()
    {
        StartCoroutine(_ChangeSceneShop());
    }

    

   
    
    
    private void Update()
    {
        // Check for input when player is inside collider
        if (playerInRange && Input.GetKeyDown(interactKey) && hasInventory)
        {
            SceneShop();
        }

        if(playerInRange && Input.GetKeyDown(interactKey) && !hasInventory)
        {
            Debug.Log("you need to get stock for the day");
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
