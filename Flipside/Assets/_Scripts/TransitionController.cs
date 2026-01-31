using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] private Light2D spotlight1, spotlight2;
    [SerializeField] private GameObject map1, map2, map3;

    private TransitionPhase transitionPhase;
    enum TransitionPhase
    {
        BOSS_ACTIVE,
        ANTICIPATION,
        VISUAL_SWAP,
        MECHANIC_SWAP,
    }

    public void TriggerNextLevel()
    {
        StartCoroutine(StartAnticipation());
    }

    IEnumerator StartAnticipation()
    {
        transitionPhase = TransitionPhase.ANTICIPATION;
        //TODO: Play Anticipation SFX
        //TODO: Spotlights stop moving
        yield return new WaitForSeconds(anticipationTime);
        StartCoroutine(StartVisualSwap());
    }

    IEnumerator StartVisualSwap()
    {
        transitionPhase = TransitionPhase.VISUAL_SWAP;
        //TODO: Turn off spotlights
        //TODO: Change spotlight colors
        //TODO: Swap map, disable incoming map mechanics, obstructions 50% opacity
        yield return new WaitForSeconds(spotlightFlickerDuration);
        //TODO: Turn on spotlights
        //TODO: Change BGM
        //TODO: Play MaskChange animation (possibly use a linked list style Animator tree with a trigger to move next sequentially)
        maskAnimator.SetTrigger(MaskChange);
        yield return new WaitForSeconds(visualToMechanicSwapTimeDelay);
        StartCoroutine(StartMechanicSwap());
    }

    IEnumerator StartMechanicSwap()
    {
        transitionPhase = TransitionPhase.MECHANIC_SWAP;
        //TODO: Enable incoming map mechanics, obstructions 100% opacity
        //TODO: Spotlights begin moving.
        transitionPhase = TransitionPhase.BOSS_ACTIVE;
        yield return null;
    }
}
