using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignettePulse : MonoBehaviour
{
    [Header("References")]
    public Volume volume;                 // assign your Global Volume here

    [Header("Pulse Settings")]
    public float baseIntensity = 0.1f;    // normal vignette level
    public float peakIntensity = 0.5f;    // how strong the pulse gets
    public float pulseDuration = 0.5f;    // time for one up/down pulse
    public int pulseCount = 3;            // how many pulses

    Vignette vignette;
    Coroutine pulseRoutine;

    void Awake()
    {
        if (volume == null)
            volume = GetComponent<Volume>();

        if (volume == null)
        {
            Debug.LogError("[VignettePulse] No Volume assigned or found.");
            return;
        }

        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("[VignettePulse] Volume has no Vignette override.");
        }
        else
        {
            vignette.intensity.value = baseIntensity;
        }
    }

    // Call this from code OR as an Animation Event
    public void PlayPulse()
    {
        if (vignette == null) return;

        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);

        pulseRoutine = StartCoroutine(PulseCo());
    }

   IEnumerator PulseCo()
{
    float half = pulseDuration * 5f;

    while (true) // ← infinite loop
    {
        // up
        float t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float lerp = t / half;
            vignette.intensity.value = Mathf.Lerp(baseIntensity, peakIntensity, lerp);
            yield return null;
        }

        // down
        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float lerp = t / half;
            vignette.intensity.value = Mathf.Lerp(peakIntensity, baseIntensity, lerp);
            yield return null;
        }
    }
}

}
