using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float floatSpeed = 50f;
    public float lifetime = 1f;
    public float fadeDuration = 0.5f;

    private TextMeshProUGUI text;
    private Color startColor;
    private float timer;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        startColor = text.color;
    }

    public void Initialize(int damage, bool isCrit)
    {
        text.text = damage.ToString();

        if (isCrit)
            text.color = Color.red;

        timer = lifetime;
    }
}
