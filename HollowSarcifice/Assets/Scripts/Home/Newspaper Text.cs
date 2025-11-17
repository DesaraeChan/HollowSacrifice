using UnityEngine;
using TMPro;

public class DayDisplay : MonoBehaviour
{
    public TMP_Text Header;
    public TMP_Text BodyText;
    public TMP_Text SubHeader;
    public TMP_Text SubBody;

    private int lastDays;

    void Start()
    {
        lastDays = DayManager.Instance.currentDay;
        UpdateDayText();
    }

    void Update()
    {
        if (DayManager.Instance.currentDay != lastDays)
        {
            UpdateDayText();
            lastDays = DayManager.Instance.currentDay;
        }
    }

    void UpdateDayText()
    {
        if (DayManager.Instance.currentDay == 1)
        {
            Header.text = "Plabs Moves to Secure Share of Solzae Revenues";
            BodyText.text = "Plabs propose a deal to share the profits of Solzae’s refinement and distribution.";
            SubHeader.text = "War Turns in Zaetia’s Favor Against Plab Aggression";
            SubBody.text = "Zaetian forces now hold a tactical advantage over our Plab neighbours.";
        }

// Change back to 2
        else if (DayManager.Instance.currentDay == 2)
        {
            Header.text = "Hostiliies resume in Plab-Zaetian Conflict";
            BodyText.text = "Plab–Zaetian conflict intensifies as hostilities resume on multiple fronts.";
            SubHeader.text = "Wave of Sudden Illness floods hospitals with No Cure";
            SubBody.text = "Hospitals are swarmed by ill patients at a record breaking pace";
        }
    }
}