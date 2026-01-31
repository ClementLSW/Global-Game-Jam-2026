using System.Collections;
using UnityEngine;

public class MaskSwitching : MonoBehaviour
{
    public Animator MaskHolderAnim;

    public bool change;     //set trigger of anim

    public int oldMaskInt;  //0-3
    public int newMaskInt;  //0-3
    
    /*by int
    0 - joy
    1 - Sadness
    2 - Anger*/

    public int oldMaskHealth;   //for old mask state
    public int newMaskHealth;   //for new mask state

    public MaskSwitcher newMask;//for function to change the mask
    public MaskSwitcher oldMask;//for funtion to change the mask

    [SerializeField] private GameObject angerMask;
    [SerializeField] private GameObject sadnessMask;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        angerMask.SetActive(false);
        sadnessMask.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (change)
        {
            StartCoroutine(triggerSwitch());
            change = false;
        }
    }

    IEnumerator triggerSwitch()
    {
        newMask.SetActiveMask(newMaskInt, newMaskHealth);
        oldMask.SetActiveMask(oldMaskInt, oldMaskHealth);
        yield return new WaitForSeconds(0.1f);
        MaskHolderAnim.SetTrigger("Switch");
        yield return new WaitForSeconds(0.8f);
        oldMask.SetActiveMask(newMaskInt, newMaskHealth);

    }

}
