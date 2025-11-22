using UnityEngine;
using UnityEngine.Rendering.Universal; 


public class LightPulse : MonoBehaviour
{
    [SerializeField] private Light2D light2D;     
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float speed = 2f;
    
    void Update()
    {
        if (light2D == null) return;
        

        // PingPong smoothly moves between 0 and 1
        float t = Mathf.PingPong(Time.time * speed, 1f);

        // Lerp blends between min and max intensity based on t
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}
