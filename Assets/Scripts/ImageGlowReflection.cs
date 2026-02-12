using UnityEngine;
using UnityEngine.UI;

public class ImageGlowReflection : MonoBehaviour
{
    public Image img;
    public float speed = 2f;
    public float glowAmount = 0.3f;

    private Material mat;
    private float value = 0;

    void Start()
    {
        mat = Instantiate(img.material);
        img.material = mat;
    }

    void Update()
    {
        // Move reflection 0 → 1 → 0
        value = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;

        // Brighten effect
        Color c = img.color;
        float add = Mathf.Lerp(0, glowAmount, value);
        img.color = new Color(c.r + add, c.g + add, c.b + add, c.a);

        // Sharp shine line
        mat.SetFloat("_Glossiness", value); // if shader supports it
    }
}
