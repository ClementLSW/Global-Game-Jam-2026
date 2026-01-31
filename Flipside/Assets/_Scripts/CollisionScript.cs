using UnityEngine;
using UnityEngine.Rendering;

public class CollisionScript : MonoBehaviour
{
    public BumperController bumperController;
    public MaskController maskController;
    public int damageAmount;
    public bool isCritBumper = false; // mask takes more damage when hit
    public bool isSwapBumper = false; // swap mask when hit

    public void Start()
    {
        bumperController = GameObject.Find("BumperController").GetComponent<BumperController>();
        maskController = GameObject.Find("MaskController").GetComponent<MaskController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            BumperController.RegisterHit();

            ContactPoint2D contact = collision.contacts[0];
            Vector2 hitPoint = contact.point;
            Vector2 hitNormal = contact.normal;

            maskController.TakeDamage(damageAmount, isCritBumper, isSwapBumper);

            if (isCritBumper == true)
            {
                isCritBumper = false;
            }

            if (isSwapBumper == true)
            {
                isSwapBumper = false;
                bumperController.AssignNewSwapBumper(); // mask swapping also called in takedamage function lol
            }
        }
    }

    public void SetAsCritBumper()
    {
        isCritBumper = true;
        // do crit bumper visuals here
    }

    public void SetAsSwapBumper()
    {
        isSwapBumper = true;
        // do crit bumper visuals here
    }
}
