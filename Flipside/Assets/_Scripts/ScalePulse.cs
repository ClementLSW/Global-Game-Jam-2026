using UnityEngine;

public class ScalePulse : MonoBehaviour
{
    [Header("Timing")]
    public float totalDuration = 1f;
    public float maxScale = 2f;

    private float timer;
    private Vector3 startScale;

    void Start()
    {
        startScale = Vector3.one;
        transform.localScale = Vector3.zero;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float half = totalDuration / 2f;

        if (timer <= half)
        {
            // Scale up: 0 → max
            float t = timer / half;
            transform.localScale = Vector3.Lerp(
                Vector3.zero,
                startScale * maxScale,
                t
            );
        }
        else if (timer <= totalDuration)
        {
            // Scale down: max → 0
            float t = (timer - half) / half;
            transform.localScale = Vector3.Lerp(
                startScale * maxScale,
                Vector3.zero,
                t
            );
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
