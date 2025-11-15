using UnityEngine;
using UnityEngine.UI;

public class FamilyToggleConnector : MonoBehaviour
{
    public Toggle familyToggle;
    public Button sendMoneyButton;

    void Start()
    {
        familyToggle.isOn = MoneyCounter.Instance.family;
        sendMoneyButton.onClick.RemoveAllListeners();

        
        familyToggle.onValueChanged.AddListener(MoneyCounter.Instance.FamilyBool);
        sendMoneyButton.onClick.AddListener(MoneyCounter.Instance.sendMoney);
    }
}