using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
public class OpenMoneyMenu : MonoBehaviour
{

    public GameObject Canvas;
    public GameObject Total;
    public GameObject Difference;
    private TMP_Text profit;
    private TMP_Text subtract;
     public GameObject MessageBox;
    public TMP_Text messageText;
    private Coroutine messageRoutine;
    void Start()
    {
        profit = Total.GetComponent<TMP_Text>();
        subtract = Difference.GetComponent<TMP_Text>();

        if (Canvas != null)
        {
            Canvas.SetActive(false);
        }

         if (MessageBox != null)
            MessageBox.SetActive(false);

    }

    void Update()
    {
        profit.text = MoneyCounter.Instance.money.ToString();
        subtract.text = MoneyCounter.Instance.setAmount().ToString();
        return;
        // Probably could be more efficient to call when the toggles are clicked rather then every frame :/
    }
    public void OpenFinancial()
    {
        if (DayManager.Instance.unlockDay)
        {
            ShowTemporaryMessage("No time to sleep. I need to get to work.", 5f);
            //play not allowed ot sleep sound here
            SoundManager.Instance.PlaySFX("GoWork");
            return;
        }

        if (Canvas != null)
        {
            EventSystem.current.SetSelectedGameObject(null); 
            Canvas.SetActive(true);
            SoundManager.Instance.PlaySFX("BedSound");
        }
    }

    public void CloseFinancial()
    {
        if (Canvas != null)
        {
            Canvas.SetActive(false);
        }

    }

    public void ShowTemporaryMessage(string text, float duration)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(MessageRoutine(text, duration));
    }

    private IEnumerator MessageRoutine(string text, float duration)
{
    messageText.text = text;
    MessageBox.SetActive(true);

    yield return new WaitForSeconds(duration);

    MessageBox.SetActive(false);
    messageRoutine = null;
}
    
}
