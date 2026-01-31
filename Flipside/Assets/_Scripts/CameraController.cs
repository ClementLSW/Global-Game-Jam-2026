using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow")]
    public float damping = 0.125f;
    public float maxY = 5f;
    public float minY = -5f;

    [Header("Screen Shake")]
    public float shakeStrength = 0.3f;
    public float shakeDuration = 0.2f;

    public string ballTag = "Ball";
    private Transform ball;
    private Vector3 velocity = Vector3.zero;

    private Vector3 shakeOffset = Vector3.zero;
    private float shakeTimer;

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
    }

    void OnBallDrain(Ball ball)
    {
        this.ball = null;
    }

    private void Awake()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    void LateUpdate()
    {
        if (ball == null) return;

        float targetY = Mathf.Clamp(ball.position.y, minY, maxY);
        Vector3 desiredPosition = new Vector3(transform.position.x, targetY, transform.position.z);

        // Smooth follow
        Vector3 basePosition = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            damping
        );

        // Screen shake
        UpdateShake();

        transform.position = basePosition + shakeOffset;
    }

    void UpdateShake()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            shakeOffset = Random.insideUnitSphere * shakeStrength;
            shakeOffset.z = 0f; // keep camera depth stable

            if (shakeTimer <= 0f)
                shakeOffset = Vector3.zero;
        }
    }

    public void Shake(float strength, float duration)
    {
        shakeStrength = strength;
        shakeDuration = duration;
        shakeTimer = duration;
    }
}
