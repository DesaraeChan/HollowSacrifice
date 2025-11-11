using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class NPC : MonoBehaviour
{
    public GameObject canvas;

    void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

}
