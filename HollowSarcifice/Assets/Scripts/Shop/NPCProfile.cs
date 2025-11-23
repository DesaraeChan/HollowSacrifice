using UnityEngine;

[CreateAssetMenu(menuName="NPC/NPC Profile")]
public class NPCProfile : ScriptableObject
{
    public string displayName;
    public CharacterType type;
    public Sprite portraitSprite;         // optional if you swap sprite renderers (maybe for later anim)
    public DialogueNode[] dialogue;         // the whole conversation graph
    public int startingReputation = 0;

    [Header("Dialogue Audio")]
public AudioClip[] typingSoundClips;

[Range(-3, 3)]
public float minPitch = 0.8f;

[Range(-3, 3)]
public float maxPitch = 1.2f;

[Range(1, 10)]
public int frequencyLevel = 3;

[Range(0f, 1f)]
public float volume = 0.7f;

public bool makePredictable = false;
public bool stopAudioSource = false;


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
