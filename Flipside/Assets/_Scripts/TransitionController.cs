using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Splines;

public class TransitionController : MonoBehaviour
{
    private static readonly int MaskChange = Animator.StringToHash("MaskChange");
    
    [Header("Parameters")]
    [SerializeField] private float anticipationTime = 1.5f;
    [SerializeField] private float spotlightFlickerDuration = 0.2f;
    [SerializeField] private float visualToMechanicSwapTimeDelay = 1.5f;
    
    //References
    [Header("References")]
    //TODO: Potentially to change to reference KL's script to call the SwapMask() function to handle the animation in his script
    [SerializeField] private Animator maskAnimator;

    [SerializeField] private Light2D spotlight1, spotlight2, globalLight, freeformLight;
    [SerializeField] private GameObject happyMap, sadMap, angryMap;

    private TransitionPhase transitionPhase;

    private MaskType previousMaskType, nextMaskType;
    enum TransitionPhase
    {
        BOSS_ACTIVE,
        ANTICIPATION,
        VISUAL_SWAP,
        MECHANIC_SWAP,
    }
    
    public void TriggerNextLevel(MaskType previousMask, MaskType nextMask)
    {
        previousMaskType = previousMask;
        nextMaskType = nextMask;
        StartCoroutine(StartAnticipation());
    }

    IEnumerator StartAnticipation()
    {
        transitionPhase = TransitionPhase.ANTICIPATION;
        //TODO: Play Anticipation SFX
        //TODO: Spotlights stop moving
        spotlight1.GetComponent<SplineAnimate>().Pause();
        spotlight2.GetComponent<SplineAnimate>().Pause();
        yield return new WaitForSeconds(anticipationTime);
        StartCoroutine(StartVisualSwap());
    }

    IEnumerator StartVisualSwap()
    {
        transitionPhase = TransitionPhase.VISUAL_SWAP;
        //TODO: Turn off spotlights
        spotlight1.enabled = false;
        spotlight2.enabled = false;
        globalLight.intensity = 0.1f;
        freeformLight.intensity = 0.2f;
        //TODO: Change spotlight colors
        //TODO: Swap map, disable incoming map mechanics, obstructions 50% opacity
        switch (previousMaskType)
        {
            case MaskType.Happy:
                switch (nextMaskType)
                {
                    case MaskType.Sad:
                        happyMap.SetActive(false);
                        sadMap.SetActive(true);
                        break;
                    case MaskType.Angry:
                        happyMap.SetActive(false);
                        angryMap.SetActive(true);
                        break;
                }
                break;
            case MaskType.Sad:
                switch (nextMaskType)
                {
                    case MaskType.Happy:
                        sadMap.SetActive(false);
                        happyMap.SetActive(true);
                        break;
                    case MaskType.Angry:
                        sadMap.SetActive(false);
                        angryMap.SetActive(true);
                        break;
                }
                break;
            case MaskType.Angry:
                switch (nextMaskType)
                {
                    case MaskType.Happy:
                        angryMap.SetActive(false);
                        happyMap.SetActive(true);
                        break;
                    case MaskType.Sad:
                        angryMap.SetActive(false);
                        sadMap.SetActive(true);
                        break;
                }
                break;
        }
        yield return new WaitForSeconds(spotlightFlickerDuration);
        //TODO: Turn on spotlights
        spotlight1.enabled = true;
        spotlight2.enabled = true;
        globalLight.intensity = 1f;
        freeformLight.intensity = 2f;
        //TODO: Change BGM
        //TODO: Play MaskChange animation (possibly use a linked list style Animator tree with a trigger to move next sequentially)
        //maskAnimator.SetTrigger(MaskChange);
        yield return new WaitForSeconds(visualToMechanicSwapTimeDelay);
        StartCoroutine(StartMechanicSwap());
    }

    IEnumerator StartMechanicSwap()
    {
        transitionPhase = TransitionPhase.MECHANIC_SWAP;
        //TODO: Enable incoming map mechanics, obstructions 100% opacity
        //TODO: Spotlights begin moving.
        spotlight1.GetComponent<SplineAnimate>().Play();
        spotlight2.GetComponent<SplineAnimate>().Play();
        transitionPhase = TransitionPhase.BOSS_ACTIVE;
        yield return null;
    }
}
