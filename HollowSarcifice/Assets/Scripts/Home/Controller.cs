using UnityEngine;

public class Controller : MonoBehaviour
{
    
    Fading fade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fade = FindFirstObjectByType<Fading>();

        fade.FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
