using UnityEngine;

public class DashPad : MonoBehaviour
{
    [SerializeField][Range(1.0f, 20.0f)] public float minExitSpeed = 3f;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("DashPad triggered by: " + other.name);

        if (other.CompareTag("Ball"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float currentMagnitude = rb.linearVelocity.magnitude;
                float newMagnitude = Mathf.Max(currentMagnitude, minExitSpeed);
                rb.linearVelocity = transform.up * currentMagnitude;
            }
        }
    }
}
