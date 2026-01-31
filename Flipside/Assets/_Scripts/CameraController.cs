using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float damping = 0.125f;
    public float maxY = 5f;
    public float minY = -5f;

    public string ballTag = "Ball";
    private Transform ball;
    private Vector3 velocity = Vector3.zero;

    private void OnEnable()
    {
        BallManager.Instance.onBallSpawned.AddListener(OnBallSpawn);
        BallManager.Instance.onBallDrained.AddListener(OnBallDrain);
    }

    private void OnDisable()
    {
        if (BallManager.Instance != null)
        {
            BallManager.Instance.onBallSpawned.RemoveListener(OnBallSpawn);
            BallManager.Instance.onBallDrained.RemoveListener(OnBallDrain);
        }
    }

    void OnBallSpawn(Ball ball)
    {
        this.ball = ball.transform;
        Debug.Log("Camera following new ball");
    }

    void OnBallDrain(Ball ball)
    {
        this.ball = null;
        Debug.Log("Camera lost ball reference");
    }

    private void Awake()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;


    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ball == null) return;

        float targetY = Mathf.Clamp(ball.position.y, minY, maxY);

        Vector3 desiredPosition = new Vector3(transform.position.x, targetY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, damping);
    }

    
}
