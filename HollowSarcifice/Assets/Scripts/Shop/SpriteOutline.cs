using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutline : MonoBehaviour
{
    [Header("Outline Settings")]
    [Range(0.01f, 0.2f)] public float outlineThickness = 0.05f;
    public Color outlineColor = Color.black;

    private SpriteRenderer spriteRenderer;
    private Material outlineMaterial;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Create an outline material instance
        outlineMaterial = new Material(Shader.Find("Sprites/Outline"));
        outlineMaterial.SetColor("_OutlineColor", outlineColor);
        outlineMaterial.SetFloat("_OutlineSize", outlineThickness);

        spriteRenderer.material = outlineMaterial;
    }

    private void OnValidate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sharedMaterial != null)
        {
            spriteRenderer.sharedMaterial.SetColor("_OutlineColor", outlineColor);
            spriteRenderer.sharedMaterial.SetFloat("_OutlineSize", outlineThickness);
        }
    }
}
