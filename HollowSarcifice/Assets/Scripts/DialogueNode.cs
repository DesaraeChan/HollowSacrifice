using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    //Basically, all dialogue choices are linear and the way you display them is by a number index
    [TextArea] public string text;          // line to type
    public int nextIndex = -1;              // next node if no choice; -1 ends

    public bool hasChoice;
    public string choiceAText;
    public string choiceBText;

    public int repDeltaA;                   // reputation effects for dialogue A/B
    public int repDeltaB;

    public int nextIfA = -1;                // branch indices for A/B, where you are in the dialogue tree
    public int nextIfB = -1;
}
