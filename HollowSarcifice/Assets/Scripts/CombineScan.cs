using UnityEngine;

public static class CombineScan
{
    private static CombineTarget[] cache;

    public static void Try(Vector2 pointerPos, DragDrop dragging, Canvas canvas)
    {
        if (dragging == null) return;

        // cache on first use
        if (cache == null || cache.Length == 0)
            cache = Object.FindObjectsOfType<CombineTarget>(true);

        var cam = (canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;

        for (int i = 0; i < cache.Length; i++)
        {
            var t = cache[i];
            if (t == null || !t.isActiveAndEnabled) continue;
            if (t.TryCombine(dragging, cam, pointerPos)) return; // combined; stop
        }
    }

    // Call if you dynamically add/remove targets a lot
    public static void ResetCache() => cache = null;
}
