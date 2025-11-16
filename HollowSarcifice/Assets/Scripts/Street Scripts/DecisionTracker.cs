using UnityEngine;
using System.Collections.Generic;

public class DecisionTracker : MonoBehaviour
{
    public static DecisionTracker Instance { get; private set; }

    // simple key choice index
    private Dictionary<string, int> choices = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetChoice(string npcId, int choiceIndex)
    {
        choices[npcId] = choiceIndex;
        Debug.Log($"[DecisionTracker] {npcId} choice = {choiceIndex}");
    }

    public bool TryGetChoice(string npcId, out int choiceIndex)
    {
        return choices.TryGetValue(npcId, out choiceIndex);
    }
}
