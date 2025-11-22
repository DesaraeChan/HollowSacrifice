using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowNews : MonoBehaviour
{
    public Canvas Canvas;
    public GameObject News;
    private ChangeScene changeScene;


    void Start()
    {
        Canvas = GetComponentInParent<Canvas>();
        changeScene = FindFirstObjectByType<ChangeScene>(FindObjectsInactive.Include);
    }
    
    void Update()
    {
        if(DayManager.Instance.newsActive == false)
        {
            News.SetActive(false);
        }
    }

    public void OpenNews()
    {
        News.SetActive(true);
        DayManager.Instance.newsActive = true;
    }

    public void closeNews()
    {
        News.SetActive(false);
        DayManager.Instance.newsActive = false;
        changeScene.StartCoroutine(changeScene._ChangeScene2D());

    }
}
