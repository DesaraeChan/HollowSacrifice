using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;   // assign in Inspector
    public RectTransform Rect { get; private set; }

    private CanvasGroup cg;
    
    //position of obj before you touch it
    private Vector2 homePos;

    // set by ItemSlot
    public bool WasDropped { get; set; }
    public RectTransform CurrentSlot { get; set; }

    void Awake()
    {
        Rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        if (!cg) cg = gameObject.AddComponent<CanvasGroup>();

        //record original pos of obj
        homePos = Rect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        WasDropped = false;
        cg.blocksRaycasts = false;   // allows slot to detect item/ item to get grabbed
    }
    public void OnDrag(PointerEventData e)
    {
        Rect.anchoredPosition += e.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData e)
    {
        cg.blocksRaycasts = true;    //prevents clicking/further interaction with obj when being dragged

        if (!WasDropped)
        {
            // not dropped on a slot, then go back to og spot
            Rect.anchoredPosition = homePos;
            CurrentSlot = null;
        }
    }

}
