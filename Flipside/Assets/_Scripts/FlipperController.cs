using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlipperController : MonoBehaviour
{
    public InputActionReference flipperAction;
    [SerializeField] private float upSpeed = 0;
    [SerializeField] private float downSpeed = 0f;

    private HingeJoint2D hinge;
    private SpriteRenderer 手;
    
    MaskController maskController;
    
    [SerializeField] Sprite 快乐的手,生气的手,悲伤的手;

    private void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();
        手 = GetComponentInChildren<SpriteRenderer>();
        SwapFlipper(MaskType.Happy);
    }

    public void SwapFlipper(MaskType maskType)
    {
        switch (maskType)
        {
            case MaskType.Happy:
                手.sprite = 快乐的手;
                break;
            case MaskType.Sad:
                手.sprite = 悲伤的手;
                break;
            case MaskType.Angry:
                手.sprite = 生气的手;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(maskType), maskType, null);
        }
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
