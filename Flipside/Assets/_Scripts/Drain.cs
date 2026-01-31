using UnityEngine;

public class Drain : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
