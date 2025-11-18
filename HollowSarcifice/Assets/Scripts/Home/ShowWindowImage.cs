using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowWindowImage : MonoBehaviour
{
    public Canvas Canvas;
    public Sprite[] dayImages;
    public GameObject Image; 
    public Image uiImage;
    void Start()
    {
        Canvas = GetComponentInParent<Canvas>();

        UpdateVisuals();
        if (uiImage != null)
        {
            Image.SetActive(false);
        }

    }

    public void OpenImage()
    {
        if (uiImage != null)
        {
            Image.SetActive(true);
            UpdateVisuals();
        }
    }

    public void closeImage()
    {
        if (uiImage != null)
        {
            Image.SetActive(false);
        }

    }


   
    public void UpdateVisuals()
    {
        if(DayManager.Instance.Night == false)
        {
            if(SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 4)
            {
                uiImage.sprite = dayImages[1];
            }
            else
            {
                uiImage.sprite = dayImages[0];
            }

            
        } else if (DayManager.Instance.Night == true)
        {
            if(SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 4)
            {
                uiImage.sprite = dayImages[3];
            }
            else
            {
                uiImage.sprite = dayImages[2];
            }
            
        }
        
    }
    }

