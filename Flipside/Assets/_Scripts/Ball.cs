using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public Rigidbody2D RigidBody => rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
