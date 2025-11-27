using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D normalClickCursor;

    [SerializeField] private Texture2D solzaeCursor;
    [SerializeField] private Texture2D solzaeClickCursor;

    public bool isSolzaeCursor = false;


    private Vector2 cursorHotspot;

    private Texture2D currentCursorTex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetCursor(normalCursor);
    }

    // Update is called once per frame
    void Update()
    {
      UpdateCursorState();
    
}

  private void UpdateCursorState()
    {
         int soupCount = SaleTracker.Instance.GetCount(ItemCategory.SolzaeSoup);
        int gearCount = SaleTracker.Instance.GetCount(ItemCategory.SolzaeGear);
        int combined  = soupCount + gearCount;

        bool solzaeActive = combined >= 4;

        // Determine which texture to use based on BOTH:
        // - solzae vs default mode
        // - clicking vs not clicking
        Texture2D targetCursor;

        if (solzaeActive)
        {
            if (Input.GetMouseButton(0))
                targetCursor = solzaeClickCursor;
            else
                targetCursor = solzaeCursor;
        }
        else
        {
            if (Input.GetMouseButton(0))
                targetCursor = normalClickCursor;
            else
                targetCursor = normalCursor;
        }

        // Only set cursor if changed (avoids spamming SetCursor every frame)
        if (targetCursor != currentCursorTex)
        {
            SetCursor(targetCursor);
            currentCursorTex = targetCursor;
        }
    }

  private void SetCursor(Texture2D tex)
    {
        if (tex == null) return;

        cursorHotspot = new Vector2(tex.width / 2f, tex.height / 2f);
        Cursor.SetCursor(tex, cursorHotspot, CursorMode.Auto);
    }
}