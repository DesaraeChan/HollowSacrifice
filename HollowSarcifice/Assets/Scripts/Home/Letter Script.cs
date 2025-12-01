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
            BodyText.text = "I hope you’re able to adapt to your new schedule running the old shop. I’m sorry I can’t be there, but I trust that you’ll make things run smoothly. We’ll both be doing our best. I know you’ve never done something like this before so I’ll help you get things going.";
            BodyText2.text = "It’s not too complicated. Here are the steps: \n1. Every morning you need to purchase your stock for the day. You can buy stock from a nearby stand. Here you’ll decide what to sell. \n2. Once you return to the shop, you can start selling your stock to the customers that walk in. \n\nThere is a variety of different people out there with different opinions. Some may want Solzae and some won’t. \n\nAlso, I put some money in the envelope to help you for today.\nI’ll write again soon.";
        }
        else if (DayManager.Instance.currentDay == 2)
        {
            BodyText.text = "I hope things have been going well for you. By now, I assume you have the hang of things. \n\nIt's cold on the front line. The battle has started to pick up again and the fighting is getting pretty bad. Everyone's morale is getting worse.";

            BodyText2.text = "Supplies are tight. If you’re able to send a small amount of cash, it would help me get the medical items I can’t find here. \n\nAnyways, I bet you’re just a fine shopkeeper. Remember happy customers equals more profits for us. \n\nKeep up the good work, I’m relying on you.";
        }
        else if (DayManager.Instance.currentDay == 3)
        {
            BodyText.text = "I wish I was there with you right now. I’m sorry I can’t be there to say goodnight and wish you a good morning before you go to the shop.";

            BodyText2.text = "There is more pressure on the front line that ever. I’m hoping this will be over soon. The generals are ordering a large scale offensive with my battalion at the head.\n\nI’m looking forward to coming home and seeing you again.";
        }
        else if (DayManager.Instance.currentDay == 4)
        {
            BodyText.text = "Morale is low. The smell of corpses is a constant reminder of our near fate. Many have deserted. At least they have a chance at survival.";

            BodyText2.text = "The one’s that aren’t fleeing are beginning to get sick and topple over. It’s hard to watch the people around me suffer like this. The bodies are piling in our trenches. I’m still staying put. I made a vow to stay in this position and keep fighting for our home. Maybe I'll get to go home soon.\n\nI’m thinking of you.";
        }
    }
}

