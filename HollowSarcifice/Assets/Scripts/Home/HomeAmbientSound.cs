using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeAmbientSound : MonoBehaviour
{
    [Header("Day/Night Sounds")]
    public AudioSource daySound;
    public AudioSource nightSound;

     [Header("Window Ambience")]
    public AudioSource windowAmbienceSource;
    public AudioClip[] windowAmbienceClips; 

    void Start()
    {
        // Ensure we are in the Home scene
        if (SceneManager.GetActiveScene().name != "Home")
            return;

        // Safely stop both before deciding
        if (daySound) daySound.Stop();
        if (nightSound) nightSound.Stop();

        // Check DayManager's night/day state
        if (DayManager.Instance != null)
        {
            if (DayManager.Instance.Night)
            {
                if (nightSound != null)
                {
                    nightSound.loop = true;
                    nightSound.Play();
                }
            }
            else
            {
                if (daySound != null)
                {
                    daySound.loop = true;
                    daySound.Play();
                }
            }
        }
    }

    
    public void StopAllAmbience()
    {
        if (daySound) daySound.Stop();
        if (nightSound) nightSound.Stop();
        if (windowAmbienceSource) windowAmbienceSource.Stop();
    }


    public void PlayWindowAmbience(int index)
    {
        if (windowAmbienceSource == null || windowAmbienceClips == null) return;
        if (index < 0 || index >= windowAmbienceClips.Length) return;

        StopAllAmbience();

        windowAmbienceSource.clip = windowAmbienceClips[index];
        windowAmbienceSource.loop = true;
        windowAmbienceSource.Play();
    }
    
    public void ResumeHomeAmbience()
    {
        StopAllAmbience();

        if (DayManager.Instance.Night)
        {
            if (nightSound)
            {
                nightSound.loop = true;
                nightSound.Play();
            }
        }
        else
        {
            if (daySound)
            {
                daySound.loop = true;
                daySound.Play();
            }
        }
    }
}
