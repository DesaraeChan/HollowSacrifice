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

         var newLines = new System.Collections.Generic.List<string>();


        if(zaetianRep >= 8 && plabRep <= 4){ //zaetian ending

        } else if (zaetianRep >= 8 && plabRep >= 4){ //best ending

        } else if (zaetianRep <= 8 && plabRep >= 4){ //plab ending

        } else if((zaetianRep <= 8 && plabRep <= 4)){ //worst ending
        }

        if(Family == 0)
        {
            
        } else if (Family == 1)
        {
            
        } else if(Family == 2)
        {
            
        } else if(Family == 3)
        {
            
        }

        


        
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
