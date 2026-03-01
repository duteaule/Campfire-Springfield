using UnityEngine;

public class dirtVariation : MonoBehaviour
{
    [Header("Color range")]
    public Color minColor = new Color(0.4f, 0.25f, 0.15f);   // dark brown
    public Color maxColor = new Color(0.6f, 0.4f, 0.25f);    // lighter brown

    [Header("Stagger (spread over time to avoid freeze)")]
    [Tooltip("Max delay in seconds before this block applies its color")]
    public float maxStaggerDelay = 2f;

    void Start()
    {
        float delay = Random.Range(0f, maxStaggerDelay);
        Invoke(nameof(ApplyColor), delay);
    }

    void ApplyColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float t = Random.value;
            sr.color = Color.Lerp(minColor, maxColor, t);
        }
    }
}
