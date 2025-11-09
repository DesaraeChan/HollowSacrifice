using UnityEngine;

[CreateAssetMenu(menuName="NPC/NPC Profile")]
public class NPCProfile : ScriptableObject
{
    public string displayName;
    public CharacterType type;
    public Sprite portraitSprite;         // optional if you swap sprite renderers (maybe for later anim)
    public DialogueNode[] dialogue;         // the whole conversation graph
    public int startingReputation = 0;
}
