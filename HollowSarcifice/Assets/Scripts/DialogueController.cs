using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private TextMeshProUGUI speakerName;        // optional
    [SerializeField] private GameObject choicePanel;              // panel holding buttons
    [SerializeField] private Button choiceAButton;
    [SerializeField] private Button choiceBButton;
    [SerializeField] private TextMeshProUGUI choiceAText;
    [SerializeField] private TextMeshProUGUI choiceBText;

    [Header("Typing")]
    [SerializeField] private float textSpeed = 0.1f;

    private NPCProfile npc;
    private NPCProfile pendingProfile;
    private GameState pendingState;
    private CharacterManager pendingOwner;
    private bool hasPending;

    private GameState gameState;
    private CharacterManager owner;
    private int index;
    private Coroutine typing;
    private bool waitingForClick;
    private bool waitingForChoice;

    private string startNodeOverride; //opt start node name

    private bool interactionEnabled = true;
    public bool hasSold = false;

    // Input (new system)
    private bool PressedThisFrame =>
        (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame); 
       // (Keyboard.current != null && (Keyboard.current.spaceKey.wasPressedThisFrame ||
                                     // Keyboard.current.enterKey.wasPressedThisFrame)) ||
       // (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);

    public void Begin(NPCProfile profile, GameState gs, CharacterManager ownerMgr)
    {
        //basically, this is called immediately, but since textbox is hidden it crashes
        //so to avoid this, store the call info

        //saving parameters for later
        pendingProfile = profile;
        pendingState = gs;
        pendingOwner = ownerMgr;
        hasPending = true;

        //if already active and enabled, start immediately
        if (isActiveAndEnabled){
            BeginInternal();
        }


   }

   public void BeginFromNode(NPCProfile profile, GameState gs, CharacterManager ownerMgr, string nodeName)
{
    startNodeOverride = nodeName;
    Begin(profile, gs, ownerMgr);   // reuse your pending/OnEnable path
}


   // Returns the linear next node name, or null if current node had choices.
public string PeekNextNodeName()
{
    if (npc == null || npc.dialogue == null || index < 0 || index >= npc.dialogue.Length)
        return null;

    var node = npc.dialogue[index];
    if (node.hasChoice) return null; // ambiguous branch
    return node.goTo;
}

// Optional: where are we now?
public string CurrentNodeName()
{
    if (npc == null || npc.dialogue == null || index < 0 || index >= npc.dialogue.Length)
        return null;
    return npc.dialogue[index].nodeName;
}


private void OnEnable()
{
    // if Begin was called while inactive, start now
    //OnEnable() runs every time the obj becomes active in the scene

    //Does it have pending dialogue request (lines left to say?) and is it enabled
    //If yes, start the coroutine from begininternal()
    if (hasPending && isActiveAndEnabled)
        BeginInternal(); //now you can start the coroutine
}

private void BeginInternal()
{
    //this  uses stored pending data from Begin
    hasPending = false;

    npc       = pendingProfile;
    gameState = pendingState;
    owner     = pendingOwner;

    //default start
    index = 0;

    //default to 0, but if an override node name was provided, jump to it (dif text box)
    if (!string.IsNullOrEmpty(startNodeOverride)){

        int idx = FindNodeIndex(startNodeOverride);
        startNodeOverride = null; //use override
        if (idx >= 0) { 
            index = idx;
            
        } else{
            gameObject.SetActive(false);
            owner.OnOutroBeginSafe();
            return;
        }
    }

   // index = 0;
    waitingForClick  = false;
    waitingForChoice = false;

    if (speakerName) speakerName.text = npc.displayName;
    choicePanel.SetActive(false);
    textComponent.text = string.Empty;

    // SAFE now to start typing (this component is enabled on an active GO)
    StartTypingNode();
}

// small helper to find a node by name
private int FindNodeIndex(string nodeName)
{
    for (int i = 0; i < npc.dialogue.Length; i++)
        if (npc.dialogue[i].nodeName == nodeName) return i;
    return -1;
}

public void SetInteractionEnabled(bool enabled)
{
    interactionEnabled = enabled;

    // If we’re locking input, hide choices so the user can’t click them
    if (!interactionEnabled && choicePanel) choicePanel.SetActive(false);
}

// Jump straight to a node by name and start typing it
public void JumpToNode(string nodeName)
{
    if (npc?.dialogue == null) return;

    for (int i = 0; i < npc.dialogue.Length; i++)
    {
        if (npc.dialogue[i].nodeName == nodeName)
        {
            index = i;
            StartTypingNode();
            return;
        }
    }

    Debug.LogWarning($"[Dialogue] JumpToNode: '{nodeName}' not found.");
}


    private void Update()
    {
        if (!interactionEnabled) return;

        if (waitingForChoice) return;

        if (PressedThisFrame)
        {
            if (typing != null)
            {
                // Skip to end of current line
                StopCoroutine(typing);
                typing = null;
                textComponent.text = npc.dialogue[index].text;
                waitingForClick = true; // now ready to advance or show choices
                return;
            }

            if (waitingForClick)
            {
                // If node has choice, reveal choices instead of advancing
                var node = npc.dialogue[index];
                if (node.hasChoice)
                {
                    ShowChoices(node);
                }
                else
                {
                    Advance();
                    
                }
            }
        }
    }

    private void StartTypingNode()
    {
        var node = npc.dialogue[index];
        textComponent.text = string.Empty;
        waitingForClick = false;
        waitingForChoice = false;

        if (typing != null) StopCoroutine(typing);
        typing = StartCoroutine(TypeLine(node.text));
    }

    private IEnumerator TypeLine(string line)
    {
        float delay = (textSpeed > 0f) ? textSpeed : 0f;
        foreach (char c in line)
        {
            textComponent.text += c;
            if (delay > 0f) yield return new WaitForSeconds(delay);
            else yield return null;
        }
        typing = null;
        waitingForClick = true; // click will either show choices or advance
    }

    private void ShowChoices(DialogueNode node)
    {
        waitingForChoice = true;
        choicePanel.SetActive(true);
        choiceAText.text = node.choiceAText;
        choiceBText.text = node.choiceBText;

        choiceAButton.onClick.RemoveAllListeners();
        choiceBButton.onClick.RemoveAllListeners();

        choiceAButton.onClick.AddListener(() => OnChoose(node, isA:true));
        choiceBButton.onClick.AddListener(() => OnChoose(node, isA:false));
    }

    private void OnChoose(DialogueNode node, bool isA)
    {
        choicePanel.SetActive(false);
        waitingForChoice = false;

        // Apply reputation change
        int delta = isA ? node.repDeltaA : node.repDeltaB;
        owner?.ApplyReputation(npc.type, delta);

        

        // Branch next index
        string next = isA ? node.nextIfA : node.nextIfB;
        
            GoThroughDialogue(next);
            // StartTypingNode();
    
     }

    private void Advance()
    {
        var currentNode = npc.dialogue[index];
        
        
            GoThroughDialogue(currentNode.goTo);
            
    }

    private bool Valid(int i) => (i >= 0 && i < npc.dialogue.Length);

    // private void GoThroughDialogue(string goTo){
    //     //this finds the dialogue node that you are wanting to go to based on name
    //         for(int i = 0; i < npc.dialogue.Length ; i++){
    //             // [i] is index of dialogue you are currently looking at
    //             DialogueNode nextNode = npc.dialogue[i];
                
    //             if(goTo == nextNode.nodeName){
    //                 index = i;
    //                 break;
    //             } if (goTo == "AskForItem") // <-- use your actual node name
    //             {
    //                 Debug.Log("AskForItem reached");
    //             owner.SwitchToAskFromNext();   // or owner.ShowAskForItemFromNext();
    //             return;                        // stop this controller; the item box will take over
    //             }

    //             if(i == npc.dialogue.Length - 1){
    //                 Debug.Log("Not found in array...");
    //                 owner.OnOutroBeginSafe();
    //                 return;
    //             }
    //         }
    //         StartTypingNode();

    // }

    private void GoThroughDialogue(string goTo)
{
    // find the target node by name
    for (int i = 0; i < npc.dialogue.Length; i++)
    {
        DialogueNode nextNode = npc.dialogue[i];

        if (nextNode.nodeName == goTo)
        {
            // SPECIAL HANDOFF: if this target is the AskForItem node,
            // hide the main box and show the item box instead.
            if (goTo == "AskForItem") // <-- your special node name
            {
                Debug.Log("Switching to item dialogue box");
                owner.SwitchToAskFromNext();          // or owner.ShowAskForItemFromNext();
                return;                                // stop this controller (no StartTypingNode)
            }

            // normal path: advance main dialogue to this node
            index = i;
            StartTypingNode();
            return;
        }
    }

    // not found end
    Debug.Log("Not found in array...");
    owner.OnOutroBeginSafe();
}



}
