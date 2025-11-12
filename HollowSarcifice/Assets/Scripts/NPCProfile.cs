using UnityEngine;

[CreateAssetMenu(menuName="NPC/NPC Profile")]
public class NPCProfile : ScriptableObject
{
    public string displayName;
    public CharacterType type;
    public Sprite portraitSprite;         // optional if you swap sprite renderers (maybe for later anim)
    public DialogueNode[] dialogue;         // the whole conversation graph
    public int startingReputation = 0;

    [System.Serializable]
    public struct NPCpref //this is a small data structure
    {
        public ItemCategory category;
        public int repDelta; // this adds/subtracts rep points
        public string nextNode; //this is for positive/negative dialogue nodes post sale
    }

    public NPCpref[] npcprefs;

    public bool TryGetPref(ItemCategory cat, out NPCpref pref){
        for (int i=0; i< npcprefs.Length; i++){
            if (npcprefs[i].category == cat){
                pref = npcprefs[i];
                return true;
            }
        }
        pref = default;
        return false;
    }
}
