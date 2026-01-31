using UnityEngine;
using TMPro;

public class MaskCollision : MonoBehaviour
{
    public MaskController maskController;
    public int damageAmount;

    public GameObject damageTextCanvas;


    public void Start()
    {
        maskController = GameObject.Find("MaskController").GetComponent<MaskController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameObject hit = collision.gameObject;
            BumperController.RegisterHit();

            ContactPoint2D contact = collision.contacts[0];
            Vector2 hitPoint = contact.point;
            Vector2 hitNormal = contact.normal;

            maskController.TakeDirectDamage(damageAmount);

            Camera.main.GetComponent<CameraController>().Shake(0.1f, 0.25f);

            hit.GetComponent<Ball>().ReflectBall(hitNormal, hitPoint);
        }
    }
}
