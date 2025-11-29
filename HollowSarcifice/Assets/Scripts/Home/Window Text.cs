using UnityEngine;
using TMPro;
using System.Collections;

public class WindowDialogueManager : MonoBehaviour
{
    public Canvas windowCanvas;
    public TextMeshProUGUI dialogueText;

    public float textSpeed = 0.03f;
    public string[] dayLines;
    public string[] nightLines;
    public string[] fogLines;

    private int index = 0;
    private bool isTyping = false;

    void Awake()
    {
        // Preload TMP *before the first frame*
        StartCoroutine(PreloadTMP());
    }

    private IEnumerator PreloadTMP()
    {
        windowCanvas.gameObject.SetActive(true);
        dialogueText.text = " ";   // force TMP to build atlas
        dialogueText.ForceMeshUpdate(true, true);

        yield return null;         // allow TMP + Canvas to finish building

        windowCanvas.gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        index = DayManager.Instance.currentDay - 1;

        windowCanvas.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TypeLine(GetLine()));
    }

    private string GetLine()
    {
        bool night = DayManager.Instance.Night;
        int sales = SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount;

        if (night)
            return sales >= 8 ? fogLines[1] : nightLines[index];
        else
            return sales >= 8 ? fogLines[0] : dayLines[index];
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    void Update()
    {
        if (!windowCanvas.gameObject.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = GetLine();
                isTyping = false;
            }
            else
            {
                windowCanvas.gameObject.SetActive(false);
            }
        }
    }
}
