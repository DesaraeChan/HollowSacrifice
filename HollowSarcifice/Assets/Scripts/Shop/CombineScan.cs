using UnityEngine;

public static class CombineScan
{
    public static void Try(Vector2 pointerPos, DragDrop dragging, Canvas canvas)
    {
        if (dragging == null) return;

        // Re-scan every call (fine for small UIs)
        var targets = Object.FindObjectsOfType<CombineTarget>(true);
        var cam = (canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;

        for (int i = 0; i < targets.Length; i++)
        {
            var t = targets[i];
            if (t == null || !t.isActiveAndEnabled) continue;
            if (t.TryCombine(dragging, cam, pointerPos)) return; // combined; stop
        }
    }
}
