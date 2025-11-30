using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class EvaporateHomeless : MonoBehaviour
{
    public GameObject homeless;

    void Update()
    {
        if (DayManager.Instance.currentDay != 1){
            homeless.SetActive(false);
        } else {
            homeless.SetActive(true);
        }
    }
}
