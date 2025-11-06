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
    private GameState gameState;
    private CharacterManager owner;
    private int index;
    private Coroutine typing;
    private bool waitingForClick;
    private bool waitingForChoice;

    // Input (new system)
    private bool PressedThisFrame =>
        (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame); 
       // (Keyboard.current != null && (Keyboard.current.spaceKey.wasPressedThisFrame ||
                                     // Keyboard.current.enterKey.wasPressedThisFrame)) ||
       // (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);

    public void Begin(NPCProfile profile, GameState gs, CharacterManager ownerMgr)
    {
        npc = profile;
        owner = ownerMgr;
        gameState = gs;

        textComponent.text = string.Empty;
        if (speakerName) speakerName.text = profile.displayName;

        index = 0;
        choicePanel.SetActive(false);
        waitingForClick = false;
        waitingForChoice = false;

        StartTypingNode();
    }


    private void Update()
    {
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

        // int next = 0;

        

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

    private void EndDialogue()
    {
        gameObject.SetActive(false); // hide the text box
        // optionally trigger a “post dialogue” animation:
        // owner.CharacterAnimator.SetTrigger("Outro");
    }

    private void GoThroughDialogue(string goTo){
        //this finds the dialogue node that you are wanting to go to based on name
            for(int i = 0; i < npc.dialogue.Length ; i++){
                // [i] is index of dialogue you are currently looking at
                DialogueNode nextNode = npc.dialogue[i];
                if(goTo == nextNode.nodeName){
                    index = i;
                    break;
                }
                if(i == npc.dialogue.Length - 1){
                    Debug.Log("Not found in array...");
                    EndDialogue();
                    return;
                }
            }
            StartTypingNode();

    }
}
