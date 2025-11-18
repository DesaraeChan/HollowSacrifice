using UnityEngine;

public class HoverSwapWindow : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite[] Images;
    public Sprite[] ImageHover;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if(DayManager.Instance.Night)
        {
            if (SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 4)
            {
                sr.sprite = Images[3];
            }
            else
            {
                sr.sprite = Images[2];
            }
            
        } else{
            if (SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 4)
            {
                sr.sprite = Images[1];
            }
            else
            {
                sr.sprite = Images[0];
            }
        }
        
    }

    void OnMouseEnter()
    {
        if(DayManager.Instance.Night)
        {
            if (SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 4)
            {
                sr.sprite = ImageHover[3];
            }
            else
            {
                sr.sprite = ImageHover[2];
            }
            
        } else{
            if (SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 4)
            {
                sr.sprite = ImageHover[1];
            }
            else
            {
                sr.sprite = ImageHover[0];
            }
        }
    }

    void OnMouseExit()
    {
        if(DayManager.Instance.Night)
        {
            if (SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 4)
            {
                sr.sprite = Images[3];
            }
            else
            {
                sr.sprite = Images[2];
            }
            
        } else{
            if (SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 4)
            {
                sr.sprite = Images[1];
            }
            else
            {
                sr.sprite = Images[0];
            }
        }
    }
}
