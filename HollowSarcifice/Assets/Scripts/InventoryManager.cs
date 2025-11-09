using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public int money;
    public TMP_Text moneyText;
    private ShopItems shopItems;

void Start(){
    if (moneyText){
        moneyText.text = money.ToString();
    }
}

    }
   
