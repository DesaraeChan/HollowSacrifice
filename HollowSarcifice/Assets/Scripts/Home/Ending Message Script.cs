using UnityEngine;

public class EndingMessageScript : MonoBehaviour
{
    public GameObject Message; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Message.SetActive(DayManager.Instance.FinalSequence);    
    }
}
