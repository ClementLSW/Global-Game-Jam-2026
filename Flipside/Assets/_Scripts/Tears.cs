using UnityEngine;

public class Tears : MonoBehaviour
{
    [SerializeField][Range(0.1f, 3.0f)] private float downForce = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            collision.GetComponent<Ball>().RigidBody.linearVelocityY += downForce;
        }
    }
}
