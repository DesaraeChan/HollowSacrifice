using UnityEngine;

public class NPCSpriteChange : MonoBehaviour
{
    [Header("NPC ID (must match NPCCloseup npcId)")]
    public string npcId;

    [Header("Sprites")]
    public Sprite choiceASprite;
    public Sprite choiceBSprite;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            Debug.LogError("[SpriteChanger] No SpriteRenderer found on this object!");
    }

    void Start()
    {
        // If we have a saved decision, apply it
        if (DecisionTracker.Instance.TryGetChoice(npcId, out int choice))
        {
            ApplySpriteForChoice(choice);
        }
    }

    private void ApplySpriteForChoice(int choice)
    {
        if (choice == 0)
        {
            // Player picked Choice A
            if (choiceASprite != null)
                sr.sprite = choiceASprite;
        }
        else if (choice == 1)
        {
            // Player picked Choice B
            if (choiceBSprite != null)
                sr.sprite = choiceBSprite;
        }
        else
        {
            Debug.LogWarning($"[SpriteChanger] Unknown choice index {choice} for NPC {npcId}");
        }
    }
}
