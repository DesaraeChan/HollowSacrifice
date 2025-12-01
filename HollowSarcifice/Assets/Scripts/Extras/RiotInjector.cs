using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class RiotInjector : MonoBehaviour
{
     public RiotCutscene cutscene;
     public Sprite[] riotImage;
    void Start()
    {
        if (cutscene == null) return;

         var endLines = new List<string>();

         var completeCutscene = new List<Sprite>();

        if(DayManager.Instance.homeOrwork){ //work
           completeCutscene.Add(riotImage[0]);
           endLines.Add("The miner is cut off by rioters, shouting, \"RID OF SOLZAE, WE MUST DISPOSE OF IT. BRING LIFE TO ZAETIA!\". The rioters trample into the shop breaking the windows and knocking the entrance door from its hinges.");
           endLines.Add("They scavenge through the shop for anything that might contain Solzae and throw it to the ground. Amidst the chaos you get knocked onto the floor. ");
           endLines.Add("Now on the floor, you feel regret that you came into the shop today. You can hear gunshots in the distance, and see guards getting pushed back outside your shop.");
           endLines.Add("One of the rioter steps on your leg, sending immense pain throughout your body. Hearing the gunshots the Zaetian’s disperse from your shop. You’re left in pain and shock from the riot.");
          
          
        } else if (!DayManager.Instance.homeOrwork){ //home
            completeCutscene.Add(riotImage[1]);
           endLines.Add("Deciding to listen to the seller’s warnings you head back to your home.");
           endLines.Add("You decide to head back home and wait until it’s safe to go outside. You’re able to make out some sounds from outside your window.");
        endLines.Add("You can hear people shouting, \"DEATH TO SOLZAE!\" and \"LIFE TO ZAETIA!\"");
        endLines.Add("From your window you can see people laying on the streets. Some appear covered head to toe in yellow splotches. From the distance you hear multiple gunshots fire, and the panic that follows after.");
        endLines.Add("You’re safe inside your house but left in shock from the riot.");

        }

       

        cutscene.lines = endLines.ToArray();
        cutscene.slideshow = completeCutscene.ToArray();
    }
}
