using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;
    public Vector2 offset = new Vector2(-30, -10);
    private Canvas Canvas;

    void Start()
    {
        Canvas = GetComponentInParent<Canvas>();
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(true);
        }
        UpdateTooltipPosition(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the tooltip when the mouse exits the object
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }

    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (tooltipPanel != null && tooltipPanel.activeSelf)
        {
            UpdateTooltipPosition(eventData);
        }
    }

     private void UpdateTooltipPosition(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas.transform as RectTransform,
            eventData.position,
            Canvas.worldCamera,
            out localPoint))
        {
            RectTransform tooltipRect = tooltipPanel.transform as RectTransform;
            tooltipRect.anchoredPosition = localPoint + offset;
        }
    }
}