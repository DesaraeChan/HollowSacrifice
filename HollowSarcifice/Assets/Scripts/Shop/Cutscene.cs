using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;

    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName;

    public TextMeshProUGUI textComponent;
    public string[] lines;
   // public Sprite[] slideshow;

    public GameObject[] slideshow;
    public Image cutscene;
    public float textSpeed;
    public NPCStock shopCanvas;

    [SerializeField] private AudioSource lineSoundSource;
    [SerializeField] private AudioClip[] lineSoundClips;


    public bool inNPCZone = false;
    public bool allowSkip = true;

    public bool DialogueDone { get; private set; } = false;

    private int index;
    private int slideshowIndex;

    private Coroutine typingRoutine;     // track typing only
    private Coroutine fadeRoutine;       // track line sound fade only
    

    void Start()
    {
        if (textComponent != null)
            textComponent.text = string.Empty;

        StartDialogue();
    }

    void Update()
    {
        // Safely set slideshow image
        // if (cutscene != null && slideshow != null && slideshow.Length > 0)
        // {
        //     if (slideshowIndex >= 0 && slideshowIndex < slideshow.Length)
        //         cutscene.sprite = slideshow[slideshowIndex];
        // }

        if (textComponent == null || lines == null || lines.Length == 0)
            return;

        if (Input.GetMouseButtonDown(0) && (!inNPCZone || allowSkip))
        {
             // If still typing, skip to end of line
            if (typingRoutine != null)
            {
                StopCoroutine(typingRoutine);
                typingRoutine = null;
                textComponent.text = lines[index];

                //StopLineSoundImmediately(); // stop cleanly on skip (no coroutines killed)
                return;
            }

            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];

                if (index >= lines.Length - 1)
                {
                    EndCutscene();
                }
            }
        }
    }

    void ShowCurrentSlide()
{
    if (slideshow == null || slideshow.Length == 0) return;

    for (int i = 0; i < slideshow.Length; i++)
    {
        if (slideshow[i] != null)
            slideshow[i].SetActive(i == slideshowIndex);
    }
}


    public void StartDialogue()
    {
        if (textComponent == null || lines == null || lines.Length == 0)
            return;

        StopAllCoroutines();
        DialogueDone = false;
        textComponent.text = string.Empty;
        index = 0;
        slideshowIndex = 0;

        PlayCurrentLineSound();

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        if (textComponent == null || lines == null || lines.Length == 0)
            yield break;

        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void EndCutscene()
    {
        if (DialogueDone) return;

        DialogueDone = true;
        gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }

    void NextLine()
    {
        // if (textComponent == null || lines == null || lines.Length == 0)
        //     return;

        if (index < lines.Length - 1)
        {
            index++;

            int previousSlideIndex = slideshowIndex;
            
            // Safe slideshow swaps
            if (index == 2 && slideshow != null && slideshow.Length > 1)
                slideshowIndex = 1;
            else if (index == 4 && slideshow != null && slideshow.Length > 2)
                slideshowIndex = 2;

            textComponent.text = string.Empty;
            if (slideshowIndex != previousSlideIndex){
                 ShowCurrentSlide();
                 PlayCurrentLineSound(); 
            }
           
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);

            if (shopCanvas != null)
                shopCanvas.gameObject.SetActive(true);
        }
    }

    void PlayCurrentLineSound()
{
    if (lineSoundSource == null || lineSoundClips == null || lineSoundClips.Length == 0)
        return;

  //  int clipIndex = index % lineSoundClips.Length;
  int clipIndex = Mathf.Clamp(slideshowIndex, 0, lineSoundClips.Length -1);
    AudioClip clip = lineSoundClips[clipIndex];

    if(fadeRoutine !=null){
        StopCoroutine(fadeRoutine);
    }

        fadeRoutine = StartCoroutine(PlayLineSoundFaded(clip));
    }


IEnumerator PlayLineSoundFaded(AudioClip clip)
{
    // quick fade out if something is playing
    float fadeTime = 0.02f; // 20ms = enough to kill clicks
    float startVol = lineSoundSource.volume;

    if (lineSoundSource.isPlaying)
    {
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            lineSoundSource.volume = Mathf.Lerp(startVol, 0f, t / fadeTime);
            yield return null;
        }
        lineSoundSource.volume = 0f;
        lineSoundSource.Stop();
    }

    // play new tone
    lineSoundSource.PlayOneShot(clip);

    // quick fade in
    for (float t = 0; t < fadeTime; t += Time.deltaTime)
    {
        lineSoundSource.volume = Mathf.Lerp(0f, startVol, t / fadeTime);
        yield return null;
    }
    lineSoundSource.volume = startVol;
}


}
