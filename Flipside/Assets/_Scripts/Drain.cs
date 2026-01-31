using UnityEngine;

public class Drain : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Drain triggered by: " + collision.name);
        if (collision.CompareTag("Ball"))
        {
            Ball ball = collision.GetComponent<Ball>();
            if (ball != null)
            {
                BallManager.Instance.DrainBall(ball);
            }
        }
    }
}
