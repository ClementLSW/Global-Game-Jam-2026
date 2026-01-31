using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Game Settings")]
    public int startingBalls = 3;

    [Header("References")]
    public MaskController maskController;

    [Header("Events")]
    public UnityEvent onGameOver;
    public UnityEvent onBallDrain;
    public UnityEvent onMaskKill;
    public UnityEvent<int> onBallsChanged;

    private int ballsRemaining;
    private bool isGameActive;

    public int BallsRemaining => ballsRemaining;
    public bool IsGameActive => isGameActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        BallManager.Instance.onBallDrained.AddListener(OnBallDrained);
    }

    private void OnDisable()
    {
        BallManager.Instance.onBallDrained.RemoveListener(OnBallDrained);
    }

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        ballsRemaining = startingBalls;
        isGameActive = true;

        onBallsChanged?.Invoke(ballsRemaining);

        BallManager.Instance.SpawnBall();
    }

    void OnBallDrained(Ball ball)
    {
        if (!isGameActive) return;

        ballsRemaining--;
        onBallDrain?.Invoke();
        onBallsChanged?.Invoke(ballsRemaining);

        if(ballsRemaining > 0)
        {
            BallManager.Instance.SpawnBallDelayed();
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameActive = false;
        onGameOver?.Invoke();
    }
}
