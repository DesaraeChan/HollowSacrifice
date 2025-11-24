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
           endLines.Add("The miner that came to speak to you gets cut off by sudden glass shattering and yelling. The shop door swings open, while the windows fall down to pieces. You feel the ground shake beneath your feet.");
           endLines.Add("Angry Zaetians fill your shop shouting \"RID OF SOLZAE, WE MUST DISPOSE OF IT. BRING LIFE TO ZAETIA!\". They begin scavenging your shop for any Solzae and throwing it to the ground and stomping on it. Amidst the chaos you get knocked over and trampled on by the rioters." );
           endLines.Add("You try to find any way to get up off the ground but there isn’t any. Lying on the floor you feel some regret deciding to sell anything at the shop today. Behind the legs of those above you, you make out some guards getting pushed over outside your shop.");
           endLines.Add("You feel a sudden shock of pain down by your knee, and suddenly your leg is overwhelmed with warmth spreading around the area. In the very far distance you hear multiple gunshots go off. The Zaetians cry out and start to panic and disperse from the area.");
           endLines.Add("After quite awhile you are alone only hearing cries in the distance. When you’re finally able to get up off your back you realize your leg is bent sideways.");
          
          
        } else if (!DayManager.Instance.homeOrwork){ //home
            completeCutscene.Add(riotImage[1]);
           endLines.Add("Deciding to listen to the seller’s warnings you head back to your home.");
           endLines.Add("There wasn’t really anything to do so you just sit and listen to the sounds coming from outside in the city.");
        endLines.Add("You get up to look out your window hoping to hear some things more clearly. You hear a mix of angry cries, glass shattering, and general banging noises. From the near distance you can make out, \"DEATH TO SOLZAE!\" and \"LIFE TO ZAETIA!\"");
        endLines.Add("From your window you can see a few other Zaetians laying against the walls of the streets. What isn’t clear if they’re like that because of the riot or because their illness has taken their strength where all they can do is lay until death takes them.");
        endLines.Add("You decide to go sit back down, but when you turn around you hear gunshots suddenly fire.");
        endLines.Add("After the sudden shock, you just sit on your bed and wait.");


        }

       

        cutscene.lines = endLines.ToArray();
        cutscene.slideshow = completeCutscene.ToArray();
    }
}
