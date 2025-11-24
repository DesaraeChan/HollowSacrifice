using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [Header("Footstep Clips")]
    public AudioClip[] footstepClips;      // all the sounds your steps can use

    [Header("Audio Settings")]
    public AudioSource source;             // your AudioSource component
    public float maxVolume = 1.0f;

    [Header("Timing")]
    public float stepInterval = 0.4f;      // how often footsteps play

    private float timer;

    void Update()
    {
        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        if (isMoving)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                PlayFootstep();
                timer = stepInterval;
            }
        }
        else
        {
            timer = 0f;  
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        // Random clip
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

        source.volume =  maxVolume;

        // Play
        source.PlayOneShot(clip);
    }
}
