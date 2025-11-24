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
           endLines.Add("");
          
        } else if (!DayManager.Instance.homeOrwork){ //home
            completeCutscene.Add(riotImage[1]);
           endLines.Add("");
        }

        List<string> finalLines = new List<string>();

        cutscene.lines = finalLines.ToArray();
        cutscene.slideshow = completeCutscene.ToArray();
    }
}
