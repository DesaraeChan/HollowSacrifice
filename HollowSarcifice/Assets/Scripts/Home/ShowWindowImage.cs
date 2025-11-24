using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowWindowImage : MonoBehaviour
{
    public Canvas Canvas;
    public Sprite[] dayImages;
    public GameObject Image; 
    public Image uiImage;

    public HomeAmbientSound ambient;

     [Header("Window Sound")]
    public AudioSource windowSoundSource;
    public AudioClip[] windowImageClips;

    private int currentImageIndex = 0;
    
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

       
        ambient.StopAllAmbience();
        ambient.PlayWindowAmbience(currentImageIndex);
        PlayWindowImageSound(currentImageIndex);

           
    }

    public void closeImage()
    {
        if (uiImage != null)
        {
            Image.SetActive(false);
        }
        ambient.StopAllAmbience();
        ambient.ResumeHomeAmbience();

    }
    
    public void UpdateVisuals()
    {
        if(DayManager.Instance.Night == false)
        {
            if(SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 4)
            {
                 currentImageIndex = 1;
                uiImage.sprite = dayImages[1];
               // ambient.StopAllAmbience();
               // ambient.PlayWindowAmbience(1);
             
              
               
            }
            else
            {
                currentImageIndex = 0;
                uiImage.sprite = dayImages[0];
            }

            
        } else if (DayManager.Instance.Night == true)
        {
            if(SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount > 4)
            {
                currentImageIndex = 3;
                uiImage.sprite = dayImages[3];
              
            }
            else
            {
                currentImageIndex = 2;
                uiImage.sprite = dayImages[2];
            }
            
        }
        
    }

     private void PlayWindowImageSound(int index)
    {
        if (windowSoundSource == null || windowImageClips == null) return;
        if (index < 0 || index >= windowImageClips.Length) return;

        AudioClip clip = windowImageClips[index];
        if (clip == null) return;

        windowSoundSource.Stop(); // prevents stacking if spammed
        windowSoundSource.PlayOneShot(clip);
    }
    }

