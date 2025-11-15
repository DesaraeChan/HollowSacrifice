using UnityEngine;

public class BackgroundNight : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite NightSprite;

    

    private SpriteRenderer sr;

    void Update()
    {
        sr = GetComponent<SpriteRenderer>();
        if(DayManager.Instance.Night)
        {
            sr.sprite = NightSprite;
        } else{
            sr.sprite = normalSprite;
        }
        
    }

}
