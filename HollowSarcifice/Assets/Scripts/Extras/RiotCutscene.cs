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

//track where we are within the text
    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       textComponent.text = string.Empty;
       StartDialogue();
       if(slideshow.Length > 0) cutscene.sprite = slideshow[0];
       //change this later to make it change if family is made
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && (!inNPCZone || allowSkip))
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }else{
                StopAllCoroutines();
                textComponent.text = lines [index];

                if (index >= lines.Length - 1 && textComponent.text == lines[index])
{
    EndTheCutscene();
}
            }
        }
    }

    public void StartDialogue(){
        StopAllCoroutines();
        DialogueDone = false;
        textComponent.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine(){
        //Type each character one by one
        foreach (char c in lines [index].ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);

        }
    }

    void EndTheCutscene(){
        if (DialogueDone) return;
        DialogueDone = true;
        gameObject.SetActive(false);

         if (!string.IsNullOrEmpty(nextSceneName))
        {
            DayManager.Instance.FinalSequence = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void NextLine(){
        if(index <lines.Length -1){
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());

        }else{
            //close text box
        gameObject.SetActive(false);
          if (shopCanvas.gameObject) shopCanvas.gameObject.SetActive(true);
        
        
        }
    }
}
