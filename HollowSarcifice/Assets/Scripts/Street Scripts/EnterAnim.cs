using UnityEngine;

public class EnterAnim : MonoBehaviour
{

     [SerializeField] private string playerTag = "Player";
     bool playerInRange;
     [SerializeField] GameObject canvas;

     void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        playerInRange = true;
        canvas.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        playerInRange = false;
        canvas.SetActive(false);
        
    }


    
}
