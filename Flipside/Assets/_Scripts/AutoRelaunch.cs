using System.Collections;
using UnityEngine;

public class AutoRelaunch : MonoBehaviour
{
    public enum state
    {
        Idle,
        Fired
    }

    public state currentState = state.Idle;

    public Transform lockpos;

    private void Awake()
    {
        BallManager.Instance.onBallDrained.AddListener(OnBallDrained);
    }

    private void OnBallDrained(Ball ball)
    {
        if(currentState == state.Fired)
        {
            currentState = state.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (currentState == state.Idle)
            {
                collision.gameObject.transform.position = lockpos.position;
                collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                StartCoroutine(FireBall(collision.attachedRigidbody));
                currentState = state.Fired;
            }
        }
    }

    private IEnumerator FireBall(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(0.5f);
        rb.AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
    }
}
