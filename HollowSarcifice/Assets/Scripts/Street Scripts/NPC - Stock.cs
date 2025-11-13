using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class NPCStock : MonoBehaviour
{
    public GameObject canvas;
    public KeyCode interactKey = KeyCode.E;
    private bool playerInRange = false;

   [SerializeField] GameObject dialogueBox;
  [SerializeField] Cutscene cutscene;

   
    void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }

    }
    
    private void Update()
    {
        // Check for input when player is inside collider
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            ToggleWindow();
        }
        
    }

    private void ToggleWindow()
    {
        if (canvas != null)
        {
            bool isActive = canvas.activeSelf;
            canvas.SetActive(!isActive);
            var playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = isActive; 
        }

        

    }

     private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered NPC range");
            if (cutscene != null)
    {
        // Make sure the coroutine host is active
        if (!cutscene.gameObject.activeSelf)
            cutscene.gameObject.SetActive(true);

        cutscene.inNPCZone = true;
        cutscene.allowSkip = false;
        cutscene.StartDialogue();    
    }
}
          

        }

        
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            cutscene.inNPCZone = false;
            cutscene.allowSkip = true;
            dialogueBox.SetActive(false);
            Debug.Log("Player left NPC range");
            if (canvas != null)
                canvas.SetActive(false);
        }
    }
}
