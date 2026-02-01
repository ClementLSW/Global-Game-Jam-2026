using UnityEngine;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine.VFX;

public class CollisionScript : MonoBehaviour
{
    [Header("General")]
    public BumperController bumperController;
    public MaskController maskController;
    public int damageAmount;
    public bool isCritBumper = false; // mask takes more damage when hit

    public GameObject damageTextCanvas;

    [Header("VFX")]
    [SerializeField] private GameObject ringVfxGO;
    [SerializeField] private GameObject hitVfxGO;


    public void Start()
    {
        bumperController = BumperController.Instance;
        maskController = MaskController.Instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            #region VFX
            Instantiate(ringVfxGO, gameObject.transform.position, Quaternion.Euler(90, 0, 0));
            GameObject hitVfx = Instantiate(hitVfxGO, collision.gameObject.transform.position, Quaternion.identity);
            VisualEffect hitVfxGraph = hitVfx.GetComponent<VisualEffect>();
            hitVfxGraph.SetVector4("Color 2", maskController.activeColor);
            #endregion

            GameObject hit = collision.gameObject;
            BumperController.RegisterHit();

            ContactPoint2D contact = collision.contacts[0];
            Vector2 hitPoint = contact.point;
            Vector2 hitNormal = contact.normal;

            maskController.TakeBumperDamage(damageAmount, isCritBumper);

            Camera.main.GetComponent<CameraController>().Shake(0.05f, 0.05f);

            Vector3 hitPos = collision.contacts[0].point; // this is where damage text spawns as an object
            GameObject instDamageNumber = Instantiate(damageTextCanvas, (Vector2)hitPos - hitNormal, Quaternion.identity); // instantiation

            if (isCritBumper == true)
            {
                instDamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = (damageAmount*5).ToString();
            }
            else
            {
                instDamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = damageAmount.ToString();
            }


            if (isCritBumper == true)
            {
                ResetCritBumper();
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

    public void ResetCritBumper()
    {
        isCritBumper = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        Debug.Log("Crit bumper reset!");
    }
}
