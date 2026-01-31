using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BallManager : MonoBehaviour
{
    public static BallManager Instance { get; private set; }

    [Header("Spawn Settings")]
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public float respawn = 0.5f;

    [Header("Events")]
    public UnityEvent<Ball> onBallSpawned;
    public UnityEvent<Ball> onBallDrained;

    private Ball activeBall;

    public Ball ActiveBall => activeBall;
    public bool HasActiveBall => activeBall != null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnBall()
    {
        if (activeBall != null) return;

        GameObject ballObject = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        activeBall = ballObject.GetComponent<Ball>();

        onBallSpawned?.Invoke(activeBall);
    }

    public void SpawnBallDelayed()
    {
        StartCoroutine(SpawnAfterDelay());
    }

    IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(respawn);
        SpawnBall();
    }


    public void DrainBall(Ball ball)
    {
        if (ball == activeBall) activeBall = null;

        onBallDrained?.Invoke(ball);
        Destroy(ball.gameObject);
    }
}
