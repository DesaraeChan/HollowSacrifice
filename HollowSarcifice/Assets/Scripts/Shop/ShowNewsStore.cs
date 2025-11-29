using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowNewsStore : MonoBehaviour
{
    public GameObject News;



    void Start()
    {
        News.SetActive(false);
    }
    
    public void OpenNews()
    {
        News.SetActive(true);
        SoundManager.Instance.PlaySFX("Newspaper");
    }

    public void closeNews()
    {
        News.SetActive(false);
    }
}
