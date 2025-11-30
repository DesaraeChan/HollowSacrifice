using UnityEngine;

public class ResetGameState : MonoBehaviour
{
   public GameState Reps;
   private CharacterType npcType;

   public void resetRep(){

    Reps.SetRep(CharacterType.Farmer, 0);
    Reps.SetRep(CharacterType.Plab, 0);
    Reps.SetRep(CharacterType.Miner, 0);
   
   }
    
}
