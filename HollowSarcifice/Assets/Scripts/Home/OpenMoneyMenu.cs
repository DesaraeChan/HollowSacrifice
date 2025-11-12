using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class OpenMoneyMenu : MonoBehaviour
{

    public GameObject Canvas;
    public GameObject Total;
    public GameObject Difference;
    private TMP_Text profit;
    private TMP_Text subtract;

    void Start()
    {
        profit = Total.GetComponent<TMP_Text>();
        subtract = Difference.GetComponent<TMP_Text>();

        if (Canvas != null)
        {
            Canvas.SetActive(false);
        }

    }

    void Update()
    {
        profit.text = MoneyCounter.Instance.money.ToString();
        subtract.text = MoneyCounter.Instance.setAmount().ToString();
        // Probably could be more efficient to call when the toggles are clicked rather then every frame :/
    }
    public void OpenFinancial()
    {
        if (DayManager.Instance.unlockDay)
        {
            Debug.Log("Its time to go to work");
            return;
        }

        if (Canvas != null)
        {
            Canvas.SetActive(true);
        }
    }

    public void CloseFinancial()
    {
        if (Canvas != null)
        {
            Canvas.SetActive(false);
        }

    }
    
}
