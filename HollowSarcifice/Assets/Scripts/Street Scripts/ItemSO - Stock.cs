using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/ItemSO　- Stock")]
public class ItemSOStock : ScriptableObject
{
    public string itemName;
    [TextArea]public string itemDescription;
    public Sprite icon;

    public bool isGold;

    
}
