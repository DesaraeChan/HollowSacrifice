using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Scene Refs")]
    [SerializeField] public Animator characterAnimator;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private DialogueController mainDialogue;
    [SerializeField] private GameState gameState;
    [SerializeField] private ShopManager shop;


    [Header("This NPC")]
    [SerializeField] private NPCProfile currentNPC;
    public NPCProfile CurrentNPC => currentNPC;

    [Header("Next in Chain")]
    [SerializeField] private CharacterManager nextManager; //next NPC manager goes here

    [Header("AskforItemBox")]
    [SerializeField] private GameObject askForItemBox;       // the second panel    
    [SerializeField] private DialogueController askDialogue;  // controller on that panel

    public static CharacterManager Active {get; private set; }
  
    private bool busy;

    public bool IsInAskPhase { get; private set; }

   // public CharacterType CurrentType => currentNPC.type;
    

// Optional guard so we don't double-fire outro begin (call from DialogueController when last line ends)
private bool outroFired = false;

private void SetDragEnabled(bool enabled)
{
    foreach (var drag in FindObjectsOfType<DragDrop>(true))
        drag.enabled = enabled;
}


//this function starts the outro once the last node is reached
public void OnOutroBeginSafe()
{
   IsInAskPhase = false;
   SetDragEnabled(false);
    
    if (outroFired) return;
    outroFired = true;

    // Hide UI and trigger exit
    if (dialogueBox != null) dialogueBox.SetActive(false);
    if (characterAnimator != null)
        characterAnimator.SetTrigger("DialogueDone");
}


//this function sets the next NPC manager to active and starts it from Start() function
public void OnOutroComplete_ActivateNextOnly()
{
    // mark this NPC no longer busy and hide it
    busy = false;
    // gameObject.SetActive(false);

    if (Active == this) Active = null;  // <-- clear if you’re the current owner
        gameObject.SetActive(false);

    // wake next NPC (its Start() will run now and call BeginNPC(currentNPC))
    if (nextManager != null && nextManager.gameObject != null)
    {
        nextManager.gameObject.SetActive(true);
    }
}

    void Awake()
    {
        // Fallback so you don't forget to assign in Inspector
        if (characterAnimator == null)
            characterAnimator = GetComponentInChildren<Animator>();

        Debug.Log($"[CharacterManager] Awake. Animator set? {characterAnimator != null}");
    }

    void Start()
    {
        IsInAskPhase = false;
        SetDragEnabled(false);
        // TEMP: call BeginNPC so we know the path runs (remove once you call it elsewhere)
        if (currentNPC != null)
        {
            Debug.Log("[CharacterManager] Auto BeginNPC from Start()");
            BeginNPC(currentNPC);
        }
        else
        {
            Debug.LogWarning("[CharacterManager] No currentNPC assigned; BeginNPC won't run.");
        }

        if (shop!=null) shop.Initialize(this);
    }

    public void OnItemsSoldResult(int repDelta, string gotoNodeIfAny)
{
    // 1) Apply reputation if there is a delta
    if (repDelta != 0 && currentNPC != null)
        ApplyReputation(currentNPC.type, repDelta);

    // 2) Choose the post-sale node
    string targetNode =
    //if no next node after sale assigned, default to these
        !string.IsNullOrEmpty(gotoNodeIfAny) ? gotoNodeIfAny :
        (repDelta < 0 ? "Positive"
        : repDelta > 0 ? "Negative"
        : "Neutral");

    // 3) Jump back to main dialogue at the chosen node
   SwitchBackToMainAt(targetNode);
}

    public void BeginNPC(NPCProfile npc)
    {
        if (busy) { Debug.LogWarning("Begin NPC ignored: busy"); return; }
        busy = true;

        Debug.Log($"[CharacterManager] BeginNPC: {npc?.name}");
        currentNPC = npc;
        Active = this;

       // if (characterSprite) characterSprite.sprite = npc.portraitSprite;

        if (gameState != null && currentNPC != null && gameState.GetRep(npc.type) == 0)
            gameState.SetRep(npc.type, npc.startingReputation);

        CharacterIntro(); 
    }

    void CharacterIntro()
    {
        if (characterAnimator == null)
        {
            Debug.LogError("[CharacterManager] Animator ref is NULL.");
            return;
        }
//change this so that it shows after anim is done
        characterAnimator.SetTrigger("Character_Enter");
        
        Debug.Log("[CharacterManager] SetTrigger(Character_Enter) fired.");
        
    }

    public void IntroComplete() //this is related to the event in the animation 
{
    dialogueBox.SetActive(true);
    mainDialogue.Begin(currentNPC, gameState, this);  // OK: controller will start in OnEnable if needed
}

    public void ApplyReputation(CharacterType t, int delta)
    {
        gameState?.AddRep(t, delta);
    }

    public void ShowTextBox() //this is an animation event
{
    Debug.Log("[CM] ShowTextBox");
    dialogueBox.SetActive(true);
    if (mainDialogue != null && currentNPC != null)
    {
        Debug.Log($"[CM] Begin mainDialogue for NPC '{currentNPC.displayName}'");
        mainDialogue.Begin(currentNPC, gameState, this);
    }
    else
    {
        Debug.LogError("[CM] Dialogue or currentNPC is null.");
    }
}

public void SwitchToAskFromNext()
{
    if (dialogueBox) dialogueBox.SetActive(false);

    string nextNode = mainDialogue ? mainDialogue.PeekNextNodeName() : null;
    if (string.IsNullOrEmpty(nextNode))
        nextNode = "AskForItem"; // fallback node

    if (askForItemBox) askForItemBox.SetActive(true);

    IsInAskPhase = true; //reached item ask node

    // Start the item box at the target node
    askDialogue.BeginFromNode(currentNPC, gameState, this, nextNode);

    // IMPORTANT: lock input so player can't advance until sale happens
    askDialogue.SetInteractionEnabled(false);
   SetDragEnabled(true); //let player interact with items
}


// 2) Hide ask box, show main again at any node you want (resume point or fixed node).
public void SwitchBackToMainAt(string nodeName)
{
    askForItemBox.SetActive(false);
    dialogueBox.SetActive(true);
    IsInAskPhase = false;
    SetDragEnabled(false);//stop drag again
    mainDialogue.BeginFromNode(currentNPC, gameState, this, nodeName);
   
}


}
    




