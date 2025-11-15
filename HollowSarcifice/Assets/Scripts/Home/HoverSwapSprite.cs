using UnityEngine;

public class HoverSwapSprite : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite hoverSprite;

    public Sprite normalNightSprite;
    public Sprite hoverNightSprite;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if(DayManager.Instance.Night)
        {
            sr.sprite = normalNightSprite;
        } else{
            sr.sprite = normalSprite;
        }
        
    }

    void OnMouseEnter()
    {
        if(DayManager.Instance.Night)
        {
            sr.sprite = hoverNightSprite;
        } else{
            sr.sprite = hoverSprite;
        }
    }

    void OnMouseExit()
    {
        if(DayManager.Instance.Night)
        {
            sr.sprite = normalNightSprite;
        } else{
            sr.sprite = normalSprite;
        }
    }
}
