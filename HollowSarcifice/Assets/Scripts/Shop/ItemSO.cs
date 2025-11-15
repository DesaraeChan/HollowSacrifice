using UnityEngine;

public enum ItemCategory { Soup, Bowl, Ladle, Glass,Gear, SolzaeGear, Solzae, SolzaeSoup};

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]

public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite icon;
    public int price;
    public ItemCategory category;

    
    
}
