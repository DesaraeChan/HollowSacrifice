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

    private Coroutine typingRoutine;
    private Coroutine fadeRoutine;

    private bool finishedTyping = false;  // <-- NEW

    void Start()
    {
        if (textComponent != null)
            textComponent.text = string.Empty;

        StartDialogue();
    }

    void Update()
    {
        if (textComponent == null || lines == null || lines.Length == 0)
            return;

        if (Input.GetMouseButtonDown(0) && (!inNPCZone || allowSkip))
        {
            // ---------------------------------------
            // CLICK LOGIC REWORKED
            // ---------------------------------------

            // If still typing → skip to full line
            if (!finishedTyping)
            {
                if (typingRoutine != null)
                {
                    StopCoroutine(typingRoutine);
                    typingRoutine = null;
                }

                textComponent.text = lines[index];
                finishedTyping = true;
                return;
            }

            // If typing finished and this is the last line → end cutscene
            if (finishedTyping && index >= lines.Length - 1)
            {
                EndCutscene();
                return;
            }

            // Otherwise go to next line
            NextLine();
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

        typingRoutine = StartCoroutine(TypeLine());  
    }

    IEnumerator TypeLine()
    {
        finishedTyping = false;  
        textComponent.text = "";

        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        finishedTyping = true;  
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
        if (index < lines.Length - 1)
        {
            index++;

            int previousSlideIndex = slideshowIndex;

            if (index == 2 && slideshow != null && slideshow.Length > 1)
                slideshowIndex = 1;
            else if (index == 4 && slideshow != null && slideshow.Length > 2)
                slideshowIndex = 2;

            textComponent.text = string.Empty;
            finishedTyping = false;

            if (slideshowIndex != previousSlideIndex)
            {
                ShowCurrentSlide();
                PlayCurrentLineSound();
            }

            typingRoutine = StartCoroutine(TypeLine());  // <-- assign routine
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

        int clipIndex = Mathf.Clamp(slideshowIndex, 0, lineSoundClips.Length - 1);
        AudioClip clip = lineSoundClips[clipIndex];

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(PlayLineSoundFaded(clip));
    }

    IEnumerator PlayLineSoundFaded(AudioClip clip)
    {
        float fadeTime = 0.02f;
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

        lineSoundSource.PlayOneShot(clip);

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            lineSoundSource.volume = Mathf.Lerp(0f, startVol, t / fadeTime);
            yield return null;
        }

        lineSoundSource.volume = startVol;
    }
}
