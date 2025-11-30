using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [System.Serializable]
    public class SoundEntry
    {
        public string id;          
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.5f, 2f)] public float pitch = 1f;
        public bool loop = false;
    }

    [Header("All sounds")]
    [SerializeField] private SoundEntry[] sounds;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;  // one-shot sounds

    private Dictionary<string, SoundEntry> soundMap;

    private bool sfxMuted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { 
        Destroy(gameObject); 
        return; }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        soundMap = new Dictionary<string, SoundEntry>();
        foreach (var s in sounds)
        {
            if (s != null && !string.IsNullOrEmpty(s.id) && s.clip != null)
                soundMap[s.id] = s;
        }

        // auto-create sources if not assigned
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
        }
        
    }

     public void PauseAllSFX()
    {
        if (sfxSource != null && sfxSource.isPlaying)
            sfxSource.Stop();
    }

    // public void ResumeAllSFX()
    // {
    //     if (sfxSource != null)
    //         sfxSource.UnPause();
    // }

    // Play a one-shot sound by id
    public void PlaySFX(string id)
    {
        if (sfxMuted) return;
        if (!soundMap.TryGetValue(id, out var s)) return;

        sfxSource.pitch = s.pitch;
        sfxSource.PlayOneShot(s.clip, s.volume);
    }

 public void SetSFXMuted(bool mute)
    {
        sfxMuted = mute;

        // If we just muted, kill any currently playing SFX
        if (mute && sfxSource != null)
        {
            sfxSource.Stop();
        }
    }
   

}
