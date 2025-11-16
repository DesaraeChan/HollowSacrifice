using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class NPCCloseup : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueRoot;          // whole popup panel
    [SerializeField] private TextMeshProUGUI textBox;          // main text
    [SerializeField] private float charDelay = 0.03f;          // typing speed

    [Header("Intro Lines (before choices)")]
    [TextArea] public string[] introLines;

    [Header("Choices UI")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button choiceAButton;
    [SerializeField] private Button choiceBButton;
  
    [SerializeField] private TextMeshProUGUI choiceAText;
    [SerializeField] private TextMeshProUGUI choiceBText;


    [Header("Choice Labels")]
    public string choiceALabel = "Yes";
    public string choiceBLabel = "No";
 

    [Header("Response Lines per Choice")]
    [TextArea] public string[] afterChoiceA;
    [TextArea] public string[] afterChoiceB;
  

    [Header("Behaviour")]
    [SerializeField] private bool clickSkipsWhileTyping = true; // left-click to skip current line reveal

    [SerializeField] private bool hasChoices = true;
    // runtime
    bool playerInRange;
    bool isOpen;
    // bool donedialogue;  
    bool playedChoice;  
    //public bool DialogueDone => donedialogue;             
    string[] currentLines;
    int index;
    Coroutine typing;

    public Cutscene cutscene;

    //public Cutscene cutscene;

    public bool DialogueDone { get; private set; } = false;


    public string npcId = "";

    [SerializeField] Canvas closeupcanvas;




    void Awake()
    {
        if (dialogueRoot) dialogueRoot.SetActive(false);
        if (choicePanel) choicePanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        playerInRange = false;
        CloseDialogue();
    }

    void FinishDialogue()
{
    DialogueDone = true;

    // Close UI
    CloseDialogue();

    // Reenable movement
    var pm = FindFirstObjectByType<PlayerMovement>();
    if (pm) pm.enabled = true;

    // Prevent retriggering this closeup
    // var col = GetComponent<Collider2D>();
    // if (col) col.enabled = false;
}


    public void StartCloseup(){
        if (DialogueDone) return;
        if(isOpen) return;

        OpenDialogue();
    }

   
    void Update()
    {
        // Open/close with E inside trigger
        if (playerInRange && Input.GetKeyDown(interactKey)&& !DialogueDone)
        {
            // if (!isOpen) OpenDialogue();
            // else CloseDialogue();
            closeupcanvas.gameObject.SetActive(true);
            cutscene.StartDialogue();
        }

        if (!isOpen || choicePanel.activeSelf) return;

        // Advance / skip with left click or space/enter
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            // skip reveal if still typing
            if (typing != null)
            {
                if (clickSkipsWhileTyping)
                {
                    StopCoroutine(typing);
                    typing = null;
                    textBox.text = currentLines[index];
                }
                return;
            }

            // next line or show choices / finish
            if (index < currentLines.Length - 1)
            {
                index++;
                StartTypingCurrent();
            }
            else
            {
                // finished this block
                if (hasChoices && !playedChoice)
                {
                    ShowChoices();
                }
                else
                {
                   FinishDialogue();
                   // CloseDialogue();
                   // donedialogue = true;
                    //reenable movement
                    var pm = FindFirstObjectByType<PlayerMovement>();
                    if (pm) pm.enabled = true;

                     // optional: never trigger again
                    // var col = GetComponent<Collider2D>();
                    // if (col) col.enabled = false;
                }
            }
        }
    }

    void OpenDialogue()
    {
         if (dialogueRoot) dialogueRoot.SetActive(true);
        isOpen = true;
        playedChoice = false;
        currentLines = introLines;
        index = 0;
        textBox.text = "";
        StartTypingCurrent();

        // optional: lock movement while closeup is open
        var pm = FindFirstObjectByType<PlayerMovement>();
        if (pm) pm.enabled = false;
    }

    void CloseDialogue()
    {
        if (typing != null) { StopCoroutine(typing); typing = null; }
        if (choicePanel) choicePanel.SetActive(false);
        if (dialogueRoot) dialogueRoot.SetActive(false);
        isOpen = false; if (typing != null) { StopCoroutine(typing); typing = null; }
        if (choicePanel) choicePanel.SetActive(false);
        if (dialogueRoot) dialogueRoot.SetActive(false);
        isOpen = false;

        // optional: re-enable movement
        var pm = FindFirstObjectByType<PlayerMovement>();
        if (pm) pm.enabled = true;
    }

    void StartTypingCurrent()
    {
        if (typing != null) StopCoroutine(typing);
        typing = StartCoroutine(TypeLine(currentLines[index]));
    }

    IEnumerator TypeLine(string line)
    {
        textBox.text = "";
        float d = Mathf.Max(0f, charDelay);
        foreach (char c in line)
        {
            textBox.text += c;
            if (d > 0f) yield return new WaitForSeconds(d);
            else yield return null;
        }
        typing = null;
    }

    void ShowChoices()
    {
        // set labels & show/hide
        choiceAText.text = choiceALabel;
        choiceBText.text = choiceBLabel;
       

        choiceAButton.gameObject.SetActive(!string.IsNullOrEmpty(choiceALabel));
        choiceBButton.gameObject.SetActive(!string.IsNullOrEmpty(choiceBLabel));
   

        // reset listeners
        choiceAButton.onClick.RemoveAllListeners();
        choiceBButton.onClick.RemoveAllListeners();
    

        // wire choices
        choiceAButton.onClick.AddListener(() => PickChoice(0));
        choiceBButton.onClick.AddListener(() => PickChoice(1));
        // if (!string.IsNullOrEmpty(choiceCLabel))
        //     choiceCButton.onClick.AddListener(() => PickChoice(2));

        choicePanel.SetActive(true);
    }

    void PickChoice(int idx)
    {
        choicePanel.SetActive(false);
        playedChoice = true;

        if (DecisionTracker.Instance != null)
        {
            DecisionTracker.Instance.SetChoice(npcId, idx);
        }

        switch (idx)
        {
            case 0: currentLines = afterChoiceA; break;
            case 1: currentLines = afterChoiceB; break;
         
        }

        if (currentLines == null || currentLines.Length == 0)
        {
            CloseDialogue();
            return;
        }

        index = 0;
        StartTypingCurrent();
        playerInRange = false;
    }
}
