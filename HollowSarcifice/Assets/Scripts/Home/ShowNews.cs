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

    public void OpenNews()
    {
        News.SetActive(true);
    }

    public void closeNews()
    {
            News.SetActive(false);

    }
}
