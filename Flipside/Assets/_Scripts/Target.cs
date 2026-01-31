using UnityEngine;

public class Target : MonoBehaviour
{
    public enum TargetState
    {
        Hit,
        Active,
    }

    [SerializeField] private TargetState state;

    public TargetState State
    {
        get { return state; }
    }

    private Target_Set targetSet;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        state = TargetState.Active;
        targetSet = GetComponentInParent<Target_Set>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == TargetState.Active && collision.gameObject.CompareTag("Ball"))
        {
            Hit();
        }
    }

    public void Hit()
    {
        state = TargetState.Hit;
        spriteRenderer.color = Color.gray; // Change color to indicate it has been hit
        MaskController.Instance.TakeDirectDamage(5);
        Debug.Log("Target hit!");

        // Notify the Target_Set that this target has been hit
        Target_Set targetSet = GetComponentInParent<Target_Set>();
        if (targetSet != null)
        {
            targetSet.OnTargetHit();
        }
    }

    public void Activate()
    {
        state = TargetState.Active;
        spriteRenderer.color = Color.white; // Change color to indicate activation
        Debug.Log("Target activated!");
    }
}
