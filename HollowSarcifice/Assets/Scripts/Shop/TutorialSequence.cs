using UnityEngine;
using TMPro;

public class TutorialSequence : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator tutorialAnimator;

    [Tooltip("Animator trigger names, in order of steps.")]
    [SerializeField] private string[] stepTriggers;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI tutorialText;
    [TextArea]
    [SerializeField] private string[] tutorialLines;

    [Header("Input")]
    [SerializeField] private KeyCode advanceKey = KeyCode.Mouse0; // left click

    private int currentStep = 0;
    private bool tutorialActive = true;

    private void Start()
    {
        if (tutorialLines.Length == 0 || tutorialText == null || tutorialAnimator == null)
        {
            Debug.LogWarning("[TutorialSequence] Missing refs or lines.");
            tutorialActive = false;
            return;
        }

         tutorialAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

        // Pause the entire game
        Time.timeScale = 0f;

       if(SoundManager.Instance != null){
        SoundManager.Instance.SetSFXMuted(true);
       }

        currentStep = 0;
        ShowStep(currentStep);
    }

    private void Update()
    {
        if (!tutorialActive) return;

        // Inputs still work, even when paused
        if (Input.GetKeyDown(advanceKey))
        {
            AdvanceStep();
        }
    }



    private void ShowStep(int stepIndex)
    {
        // Clamp for safety
        if (stepIndex < 0 || stepIndex >= tutorialLines.Length)
        {
            EndTutorial();
            return;
        }

        // Set text
        tutorialText.text = tutorialLines[stepIndex];

        // Fire matching trigger if it exists
        if (stepTriggers != null && stepIndex < stepTriggers.Length && !string.IsNullOrEmpty(stepTriggers[stepIndex]))
        {
            tutorialAnimator.SetTrigger(stepTriggers[stepIndex]);
        }
    }

    public void AdvanceStep()
    {
        currentStep++;

        if (currentStep >= tutorialLines.Length)
        {
            EndTutorial();
        }
        else
        {
            ShowStep(currentStep);
        }
    }

    private void EndTutorial()
    {
        tutorialActive = false;

        // Resume the game
        Time.timeScale = 1f;

      if (SoundManager.Instance != null)
    {
        SoundManager.Instance.SetSFXMuted(false);

        // Play your two SFX now that SFX is unmuted
        SoundManager.Instance.PlaySFX("ShopDoor");
        SoundManager.Instance.PlaySFX("ShopFootsteps");
    }

        // Hide the whole tutorial panel/object
        gameObject.SetActive(false);
 

        Debug.Log("[TutorialSequence] Tutorial finished.");
    }
}
