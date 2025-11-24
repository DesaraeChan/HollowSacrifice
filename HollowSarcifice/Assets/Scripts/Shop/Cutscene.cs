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
    public Sprite[] slideshow;
    public Image cutscene;
    public float textSpeed;
    public NPCStock shopCanvas;

    public bool inNPCZone = false;
    public bool allowSkip = true;

    public bool DialogueDone { get; private set; } = false;

    private int index;
    private int slideshowIndex;

    void Start()
    {
        if (textComponent != null)
            textComponent.text = string.Empty;

        StartDialogue();
    }

    void Update()
    {
        // Safely set slideshow image
        if (cutscene != null && slideshow != null && slideshow.Length > 0)
        {
            if (slideshowIndex >= 0 && slideshowIndex < slideshow.Length)
                cutscene.sprite = slideshow[slideshowIndex];
        }

        if (textComponent == null || lines == null || lines.Length == 0)
            return;

        if (Input.GetMouseButtonDown(0) && (!inNPCZone || allowSkip))
        {
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

    public void StartDialogue()
    {
        if (textComponent == null || lines == null || lines.Length == 0)
            return;

        StopAllCoroutines();
        DialogueDone = false;
        textComponent.text = string.Empty;
        index = 0;
        slideshowIndex = 0;

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
        if (textComponent == null || lines == null || lines.Length == 0)
            return;

        if (index < lines.Length - 1)
        {
            index++;

            // Safe slideshow swaps
            if (index == 2 && slideshow != null && slideshow.Length > 1)
                slideshowIndex = 1;
            else if (index == 4 && slideshow != null && slideshow.Length > 2)
                slideshowIndex = 2;

            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);

            if (shopCanvas != null)
                shopCanvas.gameObject.SetActive(true);
        }
    }
}
