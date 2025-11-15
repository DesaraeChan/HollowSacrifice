using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour


{
    [SerializeField] private Animator characterAnimator;
    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName;
  

    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    public bool inNPCZone = false;
    public bool allowSkip = true;

//track where we are within the text
    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       textComponent.text = string.Empty;
       //StartDialogue();
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

                if (index >= lines.Length -1){
                    EndCutscene();
                }
            }
        }
    }

    public void StartDialogue(){
        StopAllCoroutines();
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

    void EndCutscene(){
        gameObject.SetActive(false);

         if (!string.IsNullOrEmpty(nextSceneName))
        {
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
        
        //SCENE TRANSITION SHOULD BE HERE
        // characterAnimator.SetTrigger("DialogueDone");
        }
    }
}
