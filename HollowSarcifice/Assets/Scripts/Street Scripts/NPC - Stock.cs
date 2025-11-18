using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NPCStock : MonoBehaviour
{
    [Header("Main Windows")]
    public GameObject shopCanvas;        // The shop window (opens AFTER closeup ends)
    public GameObject closeupCanvas;     // The dialogue closeup canvas (opens DURING cutscene)

    [Header("Input")]
    public KeyCode interactKey = KeyCode.D;

    private bool playerInRange = false;

    [Header("References")]
    [SerializeField] private NPCCloseup closeup;
    [SerializeField] private Cutscene cutscene;

    void Start()
    {
        if (closeupCanvas) closeupCanvas.SetActive(false);
        if (shopCanvas) shopCanvas.SetActive(false);

        if (closeup == null)
            closeup = GetComponent<NPCCloseup>();
    }

    private void Update()
    {
        if (!playerInRange) return;

        // Wait for interact key
        if (Input.GetKeyDown(interactKey))
        {
            // FIRST Run the closeup dialogue
            if (!closeup.DialogueDone)
            {
                // Show the closeup canvas
                if (closeupCanvas) closeupCanvas.SetActive(true);

                closeup.StartCloseup();
                return;
            }

            // AFTER dialogue is finished allow shop window toggle
            ToggleShop();
        }
    }

    void ToggleShop()
    {
        if (shopCanvas == null) return;

        bool isActive = shopCanvas.activeSelf;
        shopCanvas.SetActive(!isActive);

        // Lock/unlock player movement
        var pm = FindFirstObjectByType<PlayerMovement>();
        if (pm) pm.enabled = isActive;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        // Make sure cutscene object is active
        if (cutscene && !cutscene.gameObject.activeSelf)
            cutscene.gameObject.SetActive(true);

        if (cutscene)
        {
            cutscene.inNPCZone = true;
            cutscene.allowSkip = false;

            cutscene.StartDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        cutscene.gameObject.SetActive(false);

        // Reset cutscene state
        if (cutscene)
        {
            cutscene.inNPCZone = false;
            cutscene.allowSkip = true;
        }

        // Close UI when leaving
        if (closeupCanvas) closeupCanvas.SetActive(false);
        if (shopCanvas) shopCanvas.SetActive(false);

        // Re-enable player movement
        var pm = FindFirstObjectByType<PlayerMovement>();
        if (pm) pm.enabled = true;
    }
}
