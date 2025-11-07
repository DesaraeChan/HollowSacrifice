using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Scene Refs")]
    [SerializeField] public Animator characterAnimator;
    [SerializeField] private GameObject textBox;
    [SerializeField] private DialogueController dialogue;
    [SerializeField] private GameState gameState;

    [Header("This NPC")]
    [SerializeField] private NPCProfile currentNPC;

    [Header("Next in Chain")]
    [SerializeField] private CharacterManager nextManager; //next NPC manager goes here
  
    private bool busy;


    void Awake()
    {
        // Fallback so you don't forget to assign in Inspector
        if (characterAnimator == null)
            characterAnimator = GetComponentInChildren<Animator>();

        Debug.Log($"[CharacterManager] Awake. Animator set? {characterAnimator != null}");
    }

    void Start()
    {
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
    }

    public void BeginNPC(NPCProfile npc)
    {
        if (busy) { Debug.LogWarning("Begin NPC ignored: busy"); return; }
        busy = true;

        Debug.Log($"[CharacterManager] BeginNPC: {npc?.name}");
        currentNPC = npc;

       // if (characterSprite) characterSprite.sprite = npc.portraitSprite;

        if (gameState != null && currentNPC != null && gameState.GetRep(npc.type) == 0)
            gameState.SetRep(npc.type, npc.startingReputation);

        CharacterIntro(); 
    }


public void OnOutroComplete(){
        // 1) Release busy on THIS manager
        gameObject.SetActive(false);
        busy = false;

        // 2) Start the NEXT manager (NOT this one)
        if (nextManager != null && this.gameObject ==null)
        {
            nextManager.gameObject.SetActive(true);             // ensure NPC2 is active
            nextManager.BeginNPC(nextManager.currentNPC);       // start NPC2
        }

        // 3) Optionally hide this NPC after leaving
        
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

    public void ApplyReputation(CharacterType t, int delta)
    {
        gameState?.AddRep(t, delta);
    }

    public void ShowTextBox() //this is an animation event
{
    Debug.Log("[CM] ShowTextBox");
    textBox.SetActive(true);
    if (dialogue != null && currentNPC != null)
    {
        Debug.Log($"[CM] Begin dialogue for NPC '{currentNPC.displayName}'");
        dialogue.Begin(currentNPC, gameState, this);
    }
    else
    {
        Debug.LogError("[CM] Dialogue or currentNPC is null.");
    }
}

public void IntroComplete() //this is related to the event in the animation 
{
    textBox.SetActive(true);
    dialogue.Begin(currentNPC, gameState, this);  // OK: controller will start in OnEnable if needed
}

// public void OnOutroBegin(){
//    busy = false;
//     seqIndex++;
//     if (seqIndex < sequence.Length){
//         BeginNPC(sequence[seqIndex]);
//     }else{
//         Debug.Log("Sequence finished");
//     }

// }

}
    




