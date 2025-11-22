using UnityEngine;

public class CutsceneTextInjector : MonoBehaviour
{
     public Cutscene cutscene;
     public GameState repPoints;
     public CharacterType npcType;
     int farmerRep = gameState.GetRep(CharacterType.Farmer);
    int plabRep = gameState.GetRep(CharacterType.Plab);
    int minerRep = gameState.GetRep(CharacterType.Miner);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int Family = MoneyCounter.Instance.sendToFamily;
        if (cutscene == null) return;

         var newLines = new System.Collections.Generic.List<string>();
        if(Family == 0)
        {
            
        } else if (Family == 1)
        {
            
        } else if(Family == 2)
        {
            
        } else if(Family == 3)
        {
            
        }

        // if(SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 7)
        // {
            
        // }else if (plabRep > minerRep && plabRep > farmerRep && plabRep > 5) //Plab ending
        // {
        //     newLines.add("");
        // } else if (farmerRep > 5 && minerRep > 5)
        // {
            
        // } else //default
        // {
            
        // }


        
        

        // if(minerRep > farmerRep && minerRep > plabRep && minerRep > 5) //miner ending
        // {
            
        // } else if (minerRep < farmerRep && farmerRep > plabRep && farmerRep > 5) //farmer Ending
        // {
            
        // } else if (plabRep > minerRep && plabRep > farmerRep && plabRep > 5) //Plab ending
        // {
            
        // } else if(minerRep > 5 && plabRep > 5 && farmerRep > 5) //Hero of the people
        // {
            
        // }else if (minerRep < 5 && plabRep < 5 && farmerRep < 5){ //scum of earth
        
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
