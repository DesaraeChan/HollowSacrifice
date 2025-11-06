using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Scene Refs")]
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private GameObject textBox;
    [SerializeField] private DialogueController dialogue;
    [SerializeField] private GameState gameState;

    [Header("Active NPC")]
    [SerializeField] private NPCProfile currentNPC;

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
        Debug.Log($"[CharacterManager] BeginNPC: {npc?.name}");
        currentNPC = npc;

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
        ShowTextBox();
    }

    // Animation Event calls this at the end of the Enter clip
    // public void ShowTextBox()
    // {
    //     Debug.Log("[CharacterManager] ShowTextBox()");
    //     textBox.SetActive(true);
    //     if (dialogue != null && currentNPC != null)
    //         dialogue.Begin(currentNPC, gameState, this);
    // }

    public void ApplyReputation(CharacterType t, int delta)
    {
        gameState?.AddRep(t, delta);
    }

    public void ShowTextBox()
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

}
