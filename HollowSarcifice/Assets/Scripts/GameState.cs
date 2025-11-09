using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="NPC/Game State")]
public class GameState : ScriptableObject
{
    [System.Serializable] public class RepEntry
    {
        public CharacterType type;
        public int reputation;
    }

    public List<RepEntry> reps = new List<RepEntry>();

    public int GetRep(CharacterType t)
    {
        var e = reps.Find(x => x.type == t);
        return e != null ? e.reputation : 0;
    }

    public void SetRep(CharacterType t, int value)
    {
        var e = reps.Find(x => x.type == t);
        if (e != null) e.reputation = value;
        else reps.Add(new RepEntry { type = t, reputation = value });
    }

    public void AddRep(CharacterType t, int delta) => SetRep(t, GetRep(t) + delta);

    
}
