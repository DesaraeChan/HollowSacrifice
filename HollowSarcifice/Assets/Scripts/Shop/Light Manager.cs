using UnityEngine;

public class LightManager : MonoBehaviour
{

    [SerializeField] GameObject Light;

    // Update is called once per frame
    void Update()
    {
        Light.SetActive(DayManager.Instance.Night);
    }
}
