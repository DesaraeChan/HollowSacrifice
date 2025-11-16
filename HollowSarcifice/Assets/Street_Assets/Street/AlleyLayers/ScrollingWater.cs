using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScrollingWater : MonoBehaviour
{
    [Tooltip("How fast the water scrolls downward")]
    public float scrollSpeed = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Material mat;
    private float offsetY;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // This gives us a unique material instance for JUST this sprite,
        // so we don't accidentally scroll every sprite using the same material.
        mat = spriteRenderer.material;
    }

    void Update()
    {
        // Move offset over time
        offsetY += scrollSpeed * Time.deltaTime;

        // Keep it in [0,1] so it doesn't grow forever (optional but tidy)
        if (offsetY > 1f)
            offsetY -= 1f;

        // Current X offset stays the same, we only move Y
        Vector2 offset = mat.GetTextureOffset("_MainTex");
        offset.y = -offsetY;      // negative = scroll downward
        mat.SetTextureOffset("_MainTex", offset);
    }
}
