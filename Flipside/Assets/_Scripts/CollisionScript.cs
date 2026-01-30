using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public MaskController maskController;
    public int damageAmount;
    public bool isCritSpot = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            ContactPoint2D contact = collision.contacts[0];
            Vector2 hitPoint = contact.point;
            Vector2 hitNormal = contact.normal;

            maskController.TakeDamage(damageAmount, isCritSpot);
        }
    }
}
