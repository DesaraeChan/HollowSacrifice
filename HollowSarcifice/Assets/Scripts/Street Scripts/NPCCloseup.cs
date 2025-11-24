using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

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

    [Header("Typing Audio")]
    [SerializeField] private AudioClip[] typingSoundClips;
    [SerializeField] private bool makePredictable = false;
    [SerializeField] private bool stopAudioSource = false;
    [SerializeField] private int frequencyLevel = 3;

    [Range(-3f, 3f)]
    [SerializeField] private float minPitch = 0.8f;
    [Range(-3f, 3f)]
    [SerializeField] private float maxPitch = 1.2f;

    [Range(0f, 1f)]
    [SerializeField] private float typingVolume = 0.7f;

    private AudioSource typingAudioSource;

    // runtime
    bool playerInRange;
    bool isOpen;
    // bool donedialogue;  
    bool playedChoice;  
    //public bool DialogueDone => donedialogue;             
    string[] currentLines;
    int index;
    public Fading fade;
    Coroutine typing;

    public Cutscene cutscene;

    //public Cutscene cutscene;

    public bool DialogueDone { get; private set; } = false;


    public string npcId = "";
    private int lastChoice = -1; 
    

    [SerializeField] Canvas closeupcanvas;

    void Start(){
        fade = FindFirstObjectByType<Fading>();
    }


    void Awake()
    {
        if (dialogueRoot) dialogueRoot.SetActive(false);
        if (choicePanel) choicePanel.SetActive(false);

        //adding audio source
         typingAudioSource = GetComponent<AudioSource>();
        if (typingAudioSource == null)
            typingAudioSource = gameObject.AddComponent<AudioSource>();
        
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

    //same as NPC shop dialogue
        private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        if (typingSoundClips == null || typingSoundClips.Length == 0)
            return;

        if (currentDisplayedCharacterCount % frequencyLevel != 0)
            return;

        if (stopAudioSource)
            typingAudioSource.Stop();

        AudioClip clip;

        if (makePredictable)
        {
            int hash = currentCharacter.GetHashCode();
            int idx = Mathf.Abs(hash) % typingSoundClips.Length;
            clip = typingSoundClips[idx];

            // predictable pitch within min/max
            float t = (Mathf.Abs(hash) % 100) / 100f; // 0..1
            typingAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, t);
        }
        else
        {
            clip = typingSoundClips[Random.Range(0, typingSoundClips.Length)];
            typingAudioSource.pitch = Random.Range(minPitch, maxPitch);
        }

        typingAudioSource.volume = typingVolume;
        typingAudioSource.PlayOneShot(clip);
    }


    void FinishDialogue()
{

    DialogueDone = true;

    // Close UI
    CloseDialogue();

    // Reenable movement
    var pm = FindFirstObjectByType<PlayerMovement>();
    if (pm) pm.enabled = true;

    if (npcId != "Seller")
        {
                // Prevent retriggering this closeup
    var col = GetComponent<Collider2D>();
    if (col) col.enabled = false;

        }
    if(npcId == "SickGuy"){
            DayManager.Instance.alleyInteractions++;
        }

    
}


    public void StartCloseup(){
        if (DialogueDone) return;
        if (isOpen) return;

        //seller extra dialogue
        if(DayManager.Instance.currentDay == 4 && npcId == "Seller"){
            hasChoices = true;
            introLines = new string[]
        {
            "Oh you’re here? I guess I didn’t expect to see you here today.",  "You know there’s a riot happening right?",
            "You’ve seen it haven’t you? People are dying. Innocent good people.",
            "I wouldn’t interfere, otherwise I don’t think you’d live to see another day. You’d be fighting your own nation.",
            "Will you be buying anything today then?"
        };

        // Replace choice labels
        choiceALabel = "I think I'll take the day off.";
        choiceBLabel = "I will still purchase some stock today.";

        // Replace choice response lines safely
        afterChoiceA = new string[]
        {
            "Right, I think the safest option for you would be to hole up at your home."
        };

        afterChoiceB = new string[]
        {
            "Suit Yourself. Don’t say I didn’t warn ya."
        };
        }

        //seller Alleyway dialogue
        
        if(npcId == "SickGuy" && DayManager.Instance.alleyInteractions == 1){
            introLines = new string[]
        {
            "Oh, it’s you. You came back?",  "You left, why would you come back?",
            "...",
            "Well, don’t be a stranger. Not like any of us have a choice to be here.",
            "Atleast we get to look at a nice view.", "Do you ever wonder if your life could be better?", "I think about it everyday if I’m being honest with you."
        };
        } else if(npcId == "SickGuy" && DayManager.Instance.alleyInteractions == 2){
            introLines = new string[]
        {
            "I’m glad you’re back again. You’re now my favourite person to talk to.",
            "I’ve already talked a lot with everyone else in here.",
            "There’s not much else to do. Nothing really changes around here.",
            "...", "You’d be surprised how much people have to say when they have nothing better to do.", "Thanks for coming by. I appreciate your company"
        };
        }

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

                    //stop audio from playing when skipping
                    typingAudioSource.Stop();
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

        if(DayManager.Instance.currentDay == 4 && npcId == "Seller" && lastChoice == 0) { //choice a
            StartCoroutine(goHome());
        }

        if (typingAudioSource) typingAudioSource.Stop();
        
    }

    public IEnumerator goHome()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Home");
        
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

        int charCount = 0;
        foreach (char c in line)
        {
            textBox.text += c;
            charCount++;

            PlayDialogueSound(charCount, c);
            if (d > 0f) yield return new WaitForSeconds(d);
            else yield return null;
        }
        typingAudioSource.Stop();
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
        lastChoice = idx;
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
