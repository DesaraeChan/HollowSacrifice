using UnityEngine;

public class NPCSpawn : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if( DayManager.Instance.currentDay == 4){
            gameObject.SetActive(false);
        }
        
    }

    
}
