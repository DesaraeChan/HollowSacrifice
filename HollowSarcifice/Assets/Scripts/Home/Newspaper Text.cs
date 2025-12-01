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
            Header.text = "The Plab Government Moves to Secure Share of Solzae Revenues";
            BodyText.text = "Plabs propose a deal to share the profits of Solzae’s refinement and distribution. ";
            SubHeader.text = "The War Between The Plab Nation & Zaetia Continues";
            SubBody.text = "Zaetians are at an advantage against our Plab neighbours.";
        }

// Change back to 2
        else if (DayManager.Instance.currentDay == 2)
        {
            Header.text = "Fighting Picks Up In The War Aginast The Plab Nation";
            BodyText.text = "Zaetian soldiers are forced to fall back.";
            SubHeader.text = "Wave Of Sudden Illness Floods Hospitals";
            SubBody.text = "Hospitals begin to fill up at a rapid pace. Patients are being treated for an unknown sickness.";
        }
        else if (DayManager.Instance.currentDay == 3)
        {
            Header.text = "Three Minsters, Cenavin, Takri and Brecci Come To Zaetia";
            BodyText.text = "Discussions are ongoing about a possible link between the recent deadly sickness and Solzae.";
            SubHeader.text = "Plab Council Deliberates Future of Zaetian Trade";
            SubBody.text = "Plab diplomats and Zaetian congress members continue to discuss deal regarding Solzae refinement profits.";
        } else if (DayManager.Instance.currentDay == 4)
        {
            Header.text = "BREAKING NEWS";
            BodyText.text = "Minister Brecci leads a revolt against the use of solzae. The Zaetian civil conflict has erupted across the nation.";
            SubHeader.text = "The Fall Of Zaetia Is Imminent";
            SubBody.text = "Plab army is heavily pushing our Zaetian forces back.";
        }
    }
}