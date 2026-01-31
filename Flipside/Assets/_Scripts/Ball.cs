using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public Rigidbody2D RigidBody => rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ReflectBall(Vector2 hitNormal, Vector2 hitPoint)
    {
        Vector2 incomingVelocity = rb.linearVelocity;
        Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, hitNormal);

        // Optional: Add some speed boost on reflection
        //float speedBoostFactor = 1.1f;
        //reflectedVelocity *= speedBoostFactor;

        rb.linearVelocity = Vector2.zero; // Reset velocity before applying new one
        rb.AddForce(reflectedVelocity * 2, ForceMode2D.Impulse);

        rb.linearVelocity = Mathf.Clamp(rb.linearVelocity.magnitude, 0, 20) * rb.linearVelocity.normalized;

        //rb.linearVelocity = reflectedVelocity;

        //// Optional: Add some spin based on hit point
        //Vector2 offset = (Vector2)transform.position - hitPoint;
        //float spinFactor = 5f;
        //rb.angularVelocity += offset.x * spinFactor;
    }
}
