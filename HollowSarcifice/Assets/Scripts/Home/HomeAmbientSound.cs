using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeAmbientSound : MonoBehaviour
{
    [Header("Day/Night Sounds")]
    public AudioSource daySound;
    public AudioSource nightSound;

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
}
