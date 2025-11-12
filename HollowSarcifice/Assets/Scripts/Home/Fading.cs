using UnityEngine;

public class Fading : MonoBehaviour
{

    public CanvasGroup canvas;
    public bool fadein = false;
    public bool fadeout = false;

    public float TimeToFade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {
        if (fadein)
        {
            if (canvas.alpha < 1)
            {
                canvas.alpha += TimeToFade * Time.deltaTime;
            }

            if (canvas.alpha >= 1)
            {
                fadein = false;
            }
        }

        if (fadeout)
        {
            if (canvas.alpha >= 0)
            {
                canvas.alpha -= TimeToFade * Time.deltaTime;
            }

            if (canvas.alpha == 0)
            {
                fadeout = false;
            }
        }
    }

    public void FadeIn()
    {
        fadein = true;
    }
    
    public void FadeOut()
    {
        fadeout = true;
    }
}
