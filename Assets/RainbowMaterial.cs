using UnityEngine;

public class RainbowMaterial : MonoBehaviour
{
    public Renderer targetRenderer;
    public float speed = 1f;

    private float hue = 0f;

    void Update()
    {
        if (targetRenderer == null) return;

        hue += Time.deltaTime * speed;
        if (hue > 1f) hue -= 1f;

        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);
        targetRenderer.material.color = rainbowColor;
    }
}
