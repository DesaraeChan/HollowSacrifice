using UnityEngine;
using TMPro;
public class LetterScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text BodyText;
    public TMP_Text BodyText2;
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
            BodyText.text = "I hope you are getting comfortable at the shop. I hope things go smoothly. I’m sorry I can’t be there with you right now. I’ll also make you proud and fight for us both and our country against those Plabs.";
            BodyText2.text = "I know you haven't run the store on your own so I’ll give you some tips. \n1. Every morning you need to get your stock for the day. \n2. Once you’re back at the shop set everything up on the shelves, and start selling to the customers that walk in. \n\nA lot of different types of people will come in and it’s your job to provide their requests. Some like Solzae and some don’t. Keep a decerning eye on your customers to figure out what they like \nOh also I put some money in here so you canbuy some stock today. It’s not much but it should help you get things going. I’ll be relying on you.";
        }
// Change back to 3
        else if (DayManager.Instance.currentDay == 2)
        {
            BodyText.text = "I hope things have been going well for you. By now I assume you have the hang of things. \nThings have started to pick up again and the fighting is getting bad again. I can tell everyone else is tired.";

            BodyText2.text = "I might need some cash for medical supplies. It’s rough. \n\nAnyways, I bet you’re just a fine shopkeeper. \nRemember happy customers means more profits for us. Keep up the good work";
        }
    }
}

