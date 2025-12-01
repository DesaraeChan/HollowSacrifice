using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RiotCutscene : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;

    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName;

    public TextMeshProUGUI textComponent;
    public Image cutscene;

    public string[] lines;
    public Sprite[] slideshow;
    public int[] imageSwap;
    public float textSpeed;
    public NPCStock shopCanvas;

    public bool inNPCZone = false;
    public bool allowSkip = true;

    public bool DialogueDone { get; private set; } = false;

    private int index;

    private Coroutine typingRoutine;
    private bool finishedTyping = false;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();

        if (slideshow.Length > 0)
            cutscene.sprite = slideshow[0];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (!inNPCZone || allowSkip))
        {
            // -----------------------------------------
            // CLICK BEHAVIOR
            // -----------------------------------------

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

            // If typing finished and this is the final line → END CUTSCENE
            if (finishedTyping && index >= lines.Length - 1)
            {
                EndTheCutscene();
                return;
            }

            // Otherwise → go to next line
            NextLine();
        }
    }

    public void StartDialogue()
    {
        StopAllCoroutines();
        DialogueDone = false;
        textComponent.text = string.Empty;
        index = 0;

        typingRoutine = StartCoroutine(TypeLine());  // <-- track routine
    }

    IEnumerator TypeLine()
    {
        finishedTyping = false;
        textComponent.text = "";

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        finishedTyping = true; 
    }

    void EndTheCutscene()
    {
        if (DialogueDone) return;
        DialogueDone = true;

        gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            DayManager.Instance.FinalSequence = true;
            DayManager.Instance.unlockDay = false;
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;

            // Change image if index matches imageSwap[]
            for (int i = 0; i < imageSwap.Length; i++)
            {
                if (index == imageSwap[i] && i < slideshow.Length)
                {
                    cutscene.sprite = slideshow[i];
                    break;
                }
            }

            textComponent.text = string.Empty;
            typingRoutine = StartCoroutine(TypeLine());
        }
        else
        {
            // Reached end — close & show shop UI
            gameObject.SetActive(false);
            if (shopCanvas != null && shopCanvas.gameObject)
                shopCanvas.gameObject.SetActive(true);
        }
    }
}
