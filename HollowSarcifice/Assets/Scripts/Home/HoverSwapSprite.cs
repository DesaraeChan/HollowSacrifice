using UnityEngine;

public class HoverSwapSprite : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite hoverSprite;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = normalSprite;
    }

    void OnMouseEnter()
    {
        sr.sprite = hoverSprite;
    }

    void OnMouseExit()
    {
        sr.sprite = normalSprite;
    }
}
