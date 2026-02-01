using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public Rigidbody2D RigidBody => rb;

    public float currentStagnantT = 0f;
    public float stagnantT = 3f;
    public bool isTouchingFlipper = false;
    public bool isInPlay = false;

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

    public void Update()
    {
        if(rb.linearVelocity.magnitude < 0.1f && !isTouchingFlipper && isInPlay) // In Play, Not Moving, Not Touching Flipper
        {
            currentStagnantT += Time.deltaTime;
            if (currentStagnantT >= stagnantT)
            {
                rb.AddForce(new Vector2(0.2f, 5f), ForceMode2D.Impulse);
                currentStagnantT = 0f;
            }
        }
        else
        {
            currentStagnantT = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Flipper"))
        {
            isTouchingFlipper = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Flipper"))
        {
            isTouchingFlipper = false;
        }
    }
}
