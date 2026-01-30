using UnityEngine;
using UnityEngine.InputSystem;

public class FlipperController : MonoBehaviour
{
    public InputActionReference flipperAction;
    [SerializeField] private float upSpeed = 0;
    [SerializeField] private float downSpeed = 0f;

    private HingeJoint2D hinge;

    private void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();
    }

    private void OnEnable()
    {
        flipperAction.action.performed += OnFlipStart;
        flipperAction.action.canceled += OnFlipCanceled;
        flipperAction.action.Enable();
    }

    private void OnDisable()
    {
        flipperAction.action.performed -= OnFlipStart;
        flipperAction.action.canceled -= OnFlipCanceled;
        flipperAction.action.Disable();
    }

    void OnFlipStart(InputAction.CallbackContext context)
    {
        SetMotorSpeed(upSpeed);
    }

    void OnFlipCanceled(InputAction.CallbackContext context)
    {
        SetMotorSpeed(downSpeed);
    }

    void SetMotorSpeed(float speed)
    {
        JointMotor2D motor = hinge.motor;
        motor.motorSpeed = speed;
        hinge.motor = motor;
    }
}
