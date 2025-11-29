using UnityEngine;
using TMPro;
public class EndingMessageScript : MonoBehaviour
{
    public GameObject Message; 
    public TMP_Text message;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Message.SetActive(DayManager.Instance.FinalSequence);    
    }
}
