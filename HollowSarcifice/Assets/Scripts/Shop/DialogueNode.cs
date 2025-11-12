using UnityEngine;

[System.Serializable]
public class DialogueNode
{
   
    public string nodeName = "";

    [TextArea] public string text;          // line to type
   // public int nextIndex = -1;              // next node if no choice; -1 ends

    public bool hasChoice;
    public string choiceAText;
    public string choiceBText;

    public int repDeltaA;                   // reputation effects for dialogue A/B
    public int repDeltaB;

    public string nextIfA = "";                // branch indices for A/B, where you are in the dialogue tree
    public string nextIfB = "";

    public string goTo = "";

}
