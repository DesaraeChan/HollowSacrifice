using UnityEngine;

public enum ItemCategory { Soup, Gear, Solzae} //allows to define a set of named integer constraints

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]

public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite icon;
    public int price;
    public ItemCategory category;

    
    
}
