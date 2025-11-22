using UnityEngine;

public class CutsceneTextInjector : MonoBehaviour
{
     public Cutscene cutscene;
     public GameState repPoints;
     public CharacterType npcType;
    float farmerRep, plabRep, minerRep, zaetianRep;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        farmerRep = repPoints.GetRep(CharacterType.Farmer);
        plabRep = repPoints.GetRep(CharacterType.Plab);
        minerRep = repPoints.GetRep(CharacterType.Miner);

        zaetianRep = farmerRep + minerRep;
        float Family = MoneyCounter.Instance.sentToFamily;
        if (cutscene == null) return;

         var endLines = new System.Collections.Generic.List<string>();
         var familyLines = new System.Collections.Generic.List<string>();


        if(zaetianRep >= 8 && plabRep <= 4){ //zaetian ending
           

        } else if (zaetianRep >= 8 && plabRep >= 4){ //best ending
            endLines.Add("The war was resolved through depomacy. The Zaetians would mine Solzae and the Plabs would refine it. The Plabs can refine Solzae so that it would not plague the people with illness. ");
            endLines.Add("Most soldiers fighting in the war were able to return to their respective nations. Both nations would become superpowers from their cooperation.");
            endLines.Add("You continue to run your shop also still being able to sell Solzae without questioning if you might be hurting those around you.");
            endLines.Add("You feel at peace with your life. Everyday feels new and promising, even in a world cursed by acid rain. ");
        } else if (zaetianRep <= 8 && plabRep >= 4){ //plab ending
         // newLines.Add("The war is over. The Plabs became the most powerful faction leaving Zaetia to rot in their sickness.");
            endLines.Add("The war is over. The Zaetians were left rotting in their sickness, which let the plabs take over with ease.");
            endLines.Add("The Plab nation were able to harvest and refine Solzae in such a manner that caused them no illness. Harvesting the power of stone, and controlling it let them prosper against the Zaetian army.");
            endLines.Add("Everything makes you feel churning regret, that you somehow ended up here with no meaning to your decaying life.");
            endLines.Add("Your body is so weak that you can’t even sell to passerbys, most your days are spent laying against a wall or in your bed. When you glance out your window you see equal suffering, from the entirety of Zaetia.");
            endLines.Add("Maybe it didn’t have to be like this.");

        } else if((zaetianRep <= 8 && plabRep <= 4)){ //worst ending
            endLines.Add("The war ended, with the Plabs eventually retreating when a stalemate was met. They didn’t gain access to Solzae refinement like they had hoped for.");
            endLines.Add("Zaetia is still the master of earth refinery. Solzae continued to be used in the daily lives of Zaetians costing them their lives.");
            endLines.Add("Yourself included, spend your last days wondering when you’ll meet the end of your short lived life.");
            endLines.Add("You wonder if there was anything that could have been done to not feel this existential dread. The smell of death engulfs from your body, while you wish you didn’t feel this horrid sickness...");
        }

        if(Family == 0 || Family == 1)
        {
            familyLines.Add("You go on surviving day by day without your father’s return. Your life feels more and more saddening by each day that passes.");      
        } else if(Family == 2)
        {
            familyLines.Add("Your father returns from the war. Although you’re reunited, you spend your days caring for him day and night. His body tathered and broken from the toil of war.");
             familyLines.Add("You’re glad he’s back but you spend everyday wishing you had more time to yourself.");
            
        } else if(Family == 3)
        {
            familyLines.Add("Fortunately, your father returned from the war. You feel thankful everyday that he returned safely, with no injuries.");
            familyLines.Add("Everyday you try to spend as much time with him with the fear that he might have to leave you with the possibility of never returning.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
