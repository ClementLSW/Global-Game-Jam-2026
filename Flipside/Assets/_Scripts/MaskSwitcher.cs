using UnityEngine;

public class MaskSwitcher : MonoBehaviour
{
    //Um so basically this animator controls the state of the mask with health
    public Animator AngerMaskAnim;
    public Animator SadnessMaskAnim;
    public Animator JoyMaskAnim;


    void Start()
    {
        AngerMaskAnim.keepAnimatorStateOnDisable = true;
        SadnessMaskAnim.keepAnimatorStateOnDisable = true;
        JoyMaskAnim.keepAnimatorStateOnDisable = true;
    }

    /*use this with 2 different ones. One for the NewMask & OldMask.
    Use this to set which one is the old mask and which one's the new mask.
    
    by int
    0 - joy
    1 - Sadness
    2 - Anger*/

    public void SetActiveMask(int currentToBeActive, int health)
    {
        switch (currentToBeActive)
        {
            case 0://joy
                JoyMaskAnim.SetInteger("Health", health);

                JoyMaskAnim.gameObject.SetActive(true);
                SadnessMaskAnim.gameObject.SetActive(false);
                AngerMaskAnim.gameObject.SetActive(false);
                break;
            case 1:
                //sadness
                SadnessMaskAnim.SetInteger("Health", health);
                JoyMaskAnim.gameObject.SetActive(false);
                SadnessMaskAnim.gameObject.SetActive(true);
                AngerMaskAnim.gameObject.SetActive(false);
                break;
            case 2:
                AngerMaskAnim.SetInteger("Health", health);
                JoyMaskAnim.gameObject.SetActive(false);
                SadnessMaskAnim.gameObject.SetActive(false);
                AngerMaskAnim.gameObject.SetActive(true);
                break;
        }
    }
}
