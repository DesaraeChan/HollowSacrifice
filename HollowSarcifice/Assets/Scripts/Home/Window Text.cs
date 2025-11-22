using UnityEngine;
using TMPro;
using System.Collections;

public class WindowDialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Canvas windowCanvas;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Settings")]
    public float textSpeed = 0.03f;
    public string[] dayLines;      // One line per day index
    public string[] nightLines;
    public string[] fogLines;
    private int index = 0;
    private bool isTyping = false;

    void Start()
    {
        windowCanvas.gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        index = DayManager.Instance.currentDay - 1;

        

        // Ensure the canvas is visible
        windowCanvas.gameObject.SetActive(true);

        // Start dialogue
        StopAllCoroutines();
        if(DayManager.Instance.Night){
            if(SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount >= 8){
                StartCoroutine(TypeLine(fogLines[1]));
            } else {
                StartCoroutine(TypeLine(nightLines[index]));
            }
             
        } else if(SaleTracker.Instance.solzaeSoupCount + SaleTracker.Instance.solzaeGearCount >= 8){
            StartCoroutine(TypeLine(fogLines[0]));
        } else{
            StartCoroutine(TypeLine(dayLines[index]));
        }
        
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line.ToCharArray())
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
                // Skip typing
                StopAllCoroutines();
                dialogueText.text = dayLines[index];
                isTyping = false;
            }
            else
            {
                // Close window after finishing
                windowCanvas.gameObject.SetActive(false);
            }
        }
    }
}
