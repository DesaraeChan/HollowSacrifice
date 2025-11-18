using UnityEngine;
using UnityEngine.UI;
public class ToggleCallers : MonoBehaviour
{
    public Toggle familyToggle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Make sure the toggle exists
        if (familyToggle == null)
        {
            familyToggle = GetComponent<Toggle>();
        }
    }

    public void OnToggleChanged(bool value)
    {
        // Call the MoneyCounter function
        MoneyCounter.Instance.FamilyBool(value);
    }

    public void buttonClicked()
    {
        MoneyCounter.Instance.sendMoney();
    }

}
