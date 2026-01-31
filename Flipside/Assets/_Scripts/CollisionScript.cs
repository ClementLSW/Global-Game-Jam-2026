using UnityEngine;
using TMPro;

public class CollisionScript : MonoBehaviour
{
    public BumperController bumperController;
    public MaskController maskController;
    public int damageAmount;
    public bool isCritBumper = false; // mask takes more damage when hit
    public bool isSwapBumper = false; // swap mask when hit

    public GameObject damageTextCanvas;


    public void Start()
    {
        bumperController = GameController.Instance.GetComponent<BumperController>();
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

            maskController.TakeDamage(damageAmount, isCritBumper, isSwapBumper);

            Vector3 hitPos = collision.contacts[0].point; // this is where damage text spawns as an object
            GameObject instDamageNumber = Instantiate(damageTextCanvas, (Vector2)hitPos - hitNormal, Quaternion.identity); // instantiation
            instDamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = damageAmount.ToString();


            if (isCritBumper == true)
            {
                ResetCritBumper();
            }

            if (isSwapBumper == true)
            {
                ResetSwapBumper();
                MaskController.Instance.SwapMasks();
                bumperController.AssignNewSwapBumper(); // mask swapping also called in takedamage function lol
            }

            hit.GetComponent<Ball>().ReflectBall(hitNormal, hitPoint);
        }
    }

    public void SetAsCritBumper()
    {
        isCritBumper = true;
        GetComponent<SpriteRenderer>().color = Color.orange;
        Debug.Log("Crit bumper set!");
        // do crit bumper visuals here
    }

    public void SetAsSwapBumper()
    {
        isSwapBumper = true;
        GetComponent<SpriteRenderer>().color = Color.cyan;
        Debug.Log("Swap bumper set!");
        // do crit bumper visuals here
    }

    public void ResetCritBumper()
    {
        isCritBumper = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        Debug.Log("Crit bumper reset!");
    }

    public void ResetSwapBumper()
    {
        isSwapBumper = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        BumperController.Instance.swapBumperAssigned = false;
        Debug.Log("Swap bumper reset!");
    }
}
