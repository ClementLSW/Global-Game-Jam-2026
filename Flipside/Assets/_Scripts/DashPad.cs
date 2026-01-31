using UnityEngine;

public class DashPad : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("DashPad triggered by: " + other.name);

        if (other.CompareTag("Ball"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 currentVector = rb.linearVelocity.normalized; // Assuming the dash pad's right direction is the dash direction
                float dashForce = 2f; // Adjust the force as needed
                rb.AddForce(currentVector * dashForce, ForceMode2D.Impulse);
            }
        }
    }
}
