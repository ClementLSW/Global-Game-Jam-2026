using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class LaunchChute : MonoBehaviour
{
    public InputActionReference pullAction;
    public Rigidbody2D plunger;
    public SpringJoint2D spring;
    public float pullspeed = 1.5f;          // How fast the plunger pulls down
    public float maxPullDistance = 2.0f;    // How much the plunger can be pulled down
    public float releaseForce = 500.0f;     // How much force is applied when plunger is fully charged then released

    private Vector2 restPosition;
    private bool isPulling;

    [Header("VFX")]
    [SerializeField] private GameObject smokeVfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restPosition = plunger.position;
    }

    private void OnEnable()
    {
        pullAction.action.performed += OnPullStarted;
        pullAction.action.canceled += OnPullReleased;
        pullAction.action.Enable();
    }

    private void OnDisable()
    {
        pullAction.action.performed -= OnPullStarted;
        pullAction.action.canceled -= OnPullReleased;
        pullAction.action.Disable();
    }

    private void OnPullStarted(InputAction.CallbackContext context)
    {
        isPulling = true;
        spring.enabled = false; // Disable spring while pulling
    }

    private void OnPullReleased(InputAction.CallbackContext context)
    {
        if (!isPulling) return;

        isPulling = false;
        spring.enabled = true; // Re-enable spring on release

        // Apply force based on how far the plunger was pulled
        float pullDistance = Vector2.Distance(plunger.position, restPosition);
        float forceToApply = (pullDistance / maxPullDistance) * releaseForce;
        plunger.AddForce(Vector2.up * forceToApply, ForceMode2D.Impulse);
        //BallManager.Instance.ActiveBall.isInPlay = true;

        Instantiate(smokeVfx, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPulling) return;

        Vector2 newPos = plunger.position;
        newPos.y -= pullspeed * Time.deltaTime;
        newPos.y = Mathf.Max(newPos.y, restPosition.y - maxPullDistance);

        plunger.position = newPos;
        plunger.linearVelocity = Vector2.zero; // Prevent unwanted velocity buildup
    }
}
