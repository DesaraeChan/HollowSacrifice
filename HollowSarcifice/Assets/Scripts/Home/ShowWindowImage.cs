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
        if (dayImages.Length == 0) return;
        Debug.Log("Day is changed to " + DayManager.Instance.currentDay);

        int index = Mathf.Clamp(DayManager.Instance.currentDay - 1, 0, dayImages.Length - 1);
        uiImage.sprite = dayImages[index];
    }
    
    
}
