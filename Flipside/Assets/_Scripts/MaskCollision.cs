using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class MaskCollision : MonoBehaviour
{
    public MaskController maskController;
    public int damageAmount;

    public GameObject damageTextCanvas;
    public HitStop hitStop;

    [Header("VFX")]
    [SerializeField] private GameObject ringVfxGO;
    [SerializeField] private GameObject hitVfxGO;
    //[SerializeField] private GameObject hitRingVfxGO;

    public void Start()
    {
        maskController = GameObject.Find("MaskController").GetComponent<MaskController>();
        hitStop = GameObject.Find("HitStop").GetComponent<HitStop>();
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

            Vector3 dir = collision.transform.position - gameObject.transform.position;
            //GameObject hitRingVfx = Instantiate(hitRingVfxGO, gameObject.transform.position, Quaternion.Euler(dir.x, -90, -90));
            //VisualEffect hitRingVfxGraph = hitRingVfx.GetComponent<VisualEffect>();
            //hitRingVfxGraph.SetVector4("Color", maskController.activeColor);
            #endregion

            GameObject hit = collision.gameObject;
            BumperController.RegisterHit();

            ContactPoint2D contact = collision.contacts[0];
            Vector2 hitPoint = contact.point;
            Vector2 hitNormal = contact.normal;

            maskController.TakeDirectDamage(damageAmount);

            hitStop.StopTime(0.05f, 6, 0.15f);
            Camera.main.GetComponent<CameraController>().Shake(0.1f, 0.25f);

            hit.GetComponent<Ball>().ReflectBall(hitNormal, hitPoint);
        }
    }
}
