using UnityEngine;
using UnityEngine.UI;

public class StageCollider : MonoBehaviour
{
    public MaskController maskController;
    public Image ringFill;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            ringFill.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            maskController.ResetHitsLeft();
            ringFill.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Collider2D c = col != null ? col : GetComponent<Collider2D>();
        if (c == null) return;

        Gizmos.color = Color.cyan;

        Bounds b = c.bounds;
        Gizmos.DrawWireCube(b.center, b.size);
    }
}