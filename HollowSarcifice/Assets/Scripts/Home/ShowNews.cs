using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowNews : MonoBehaviour
{
    public Canvas Canvas;
    public GameObject News;


    void Start()
    {
        Canvas = GetComponentInParent<Canvas>();
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

    }
}
