using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DOFFocusOnPlayer : MonoBehaviour
{
    [SerializeField] private Volume volume;      // Volume that has a DepthOfField override
    [SerializeField] private Transform player;   // Your player transform
    [SerializeField] private Camera cam;         // Usually Camera.main
    [SerializeField] private float smooth = 10f; // Higher = snappier focus

    DepthOfField dof;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        if (volume && volume.profile.TryGet(out dof))
        {
            dof.mode.value = DepthOfFieldMode.Bokeh;   // ensure Bokeh mode
        }
    }

    void LateUpdate()
    {
        if (dof == null || player == null || cam == null) return;

        // Distance to player along camera forward (works even if camera is tilted)
        float dist = Vector3.Dot(player.position - cam.transform.position, cam.transform.forward);
        dist = Mathf.Max(0.01f, dist); // clamp to positive

        // Smoothly converge focusDistance to the target distance
        float t = 1f - Mathf.Exp(-smooth * Time.deltaTime);
        dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, dist, t);
    }
}
