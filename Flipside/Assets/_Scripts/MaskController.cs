using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum MaskType
{
    Happy,
    Sad,
    Angry
}

public class MaskController : MonoBehaviour
{
    public static MaskController Instance { get; private set; }

    [Header("Events")]
    public UnityEvent<MaskType, float> onMaskDamaged;
    public UnityEvent<MaskType> onMaskDestroyed;
    public UnityEvent<MaskType> onMaskSwapped;
    public UnityEvent onAllMasksDestroyed;

    [Header("Mask Objects")]
    public GameObject happyMask;
    public GameObject sadMask;
    public GameObject angryMask;

    [Header("Cached Positions")]
    private Vector2 happyVector;
    private Vector2 sadVector;
    private Vector2 angryVector;
    [Header("Mask Slots")]
    public Transform centerSlot;
    public Transform inactiveSlotA;
    public Transform inactiveSlotB;
    public Vector2 centerSlotPosition;

    [Header("Stats")]
    public int startingHealth = 10000;
    public float critMult = 5f;

    private int happyHealth;
    private int sadHealth;
    private int angryHealth;

    private bool happyDead;
    private bool sadDead;
    private bool angryDead;

    private MaskType currentMask;

    public MaskType CurrentMask => currentMask;
    public bool AllMasksDead => happyDead && sadDead && angryDead;

    [Header("Mask Damaging")]
    public Image ringFill;
    public int maskHitsTotal = 20;
    public int maskHitsLeft;
    public GameObject damageTextCanvas;
    private bool isImmune = false;
    public float immuneDuration = 1.5f;


    public float GetHealthPercent(MaskType type)
    {
        return type switch
        {
            MaskType.Happy => (float)happyHealth / startingHealth,
            MaskType.Sad => (float)sadHealth / startingHealth,
            MaskType.Angry => (float)angryHealth / startingHealth,
            _ => 0f
        };
    }

    public float CurrentMaskHealthPercent => GetHealthPercent(currentMask);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        happyHealth = sadHealth = angryHealth = startingHealth;

        happyDead = sadDead = angryDead = false;

        currentMask = (MaskType)Random.Range(0, 3);

        PositionMasks();

        ResetHitsLeft();
    }

    public void TakeBumperDamage(int damageAmount, bool isCritBumper = false, bool isSwapBumper = false)
    {
        if (isImmune == false)
        {
            if (AllMasksDead) return;

            if (isCritBumper)
            {
                damageAmount = Mathf.RoundToInt(damageAmount * critMult);
            }

            switch (currentMask)
            {
                case MaskType.Happy:
                    ApplyDamage(ref happyHealth, ref happyDead, damageAmount, currentMask);
                    break;

                case MaskType.Sad:
                    ApplyDamage(ref sadHealth, ref sadDead, damageAmount, currentMask);
                    break;

                case MaskType.Angry:
                    ApplyDamage(ref angryHealth, ref angryDead, damageAmount, currentMask);
                    break;
            }
        }
    }

    public void TakeDirectDamage(int baseDamage)
    {
        if (isImmune || AllMasksDead) return;

        if (maskHitsLeft > 0)
        {
            int hitNumber = maskHitsTotal - maskHitsLeft + 1;
            int damageAmount = baseDamage * hitNumber;

            Vector2 centerPos = centerSlot.position;
            GameObject instDamageNumber = Instantiate(
                damageTextCanvas,
                centerPos,
                Quaternion.identity
            );

            instDamageNumber
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = damageAmount.ToString();

            switch (currentMask)
            {
                case MaskType.Happy:
                    ApplyDamage(ref happyHealth, ref happyDead, damageAmount, currentMask);
                    break;
                case MaskType.Sad:
                    ApplyDamage(ref sadHealth, ref sadDead, damageAmount, currentMask);
                    break;
                case MaskType.Angry:
                    ApplyDamage(ref angryHealth, ref angryDead, damageAmount, currentMask);
                    break;
            }

            DecrementHits();

            if (maskHitsLeft == 0)
            {
                StartCoroutine(ImmunityCoroutine());
                SwapMasks();
            }
        }
    }

    public IEnumerator ImmunityCoroutine()
    {
        isImmune = true;
        ringFill.enabled = false;

        yield return new WaitForSeconds(immuneDuration);

        isImmune = false;
        ringFill.enabled = true;
        UpdateSlider();
    }

    void ApplyDamage(ref int health, ref bool dead, int damageAmount, MaskType type)
    {
        Debug.Log($"Mask {type} took {damageAmount} damage.");
        health -= damageAmount;
        health = Mathf.Max(0, health);

        onMaskDamaged?.Invoke(type, (float)health / startingHealth);

        if (health <= 0 && !dead)
        {
            dead = true;
            onMaskDestroyed?.Invoke(type);
            SwapMasks();
        }
    }

    public void SwapMasks()
    {
        Camera.main.GetComponent<CameraController>().Shake(0.3f, 1f);

        if (AllMasksDead)
        {
            //WinGame();
            onAllMasksDestroyed?.Invoke();
            return;
        }

        MaskType nextMask = GetHighestHealthAliveMask();

        if (nextMask == currentMask)
            return;

        currentMask = nextMask;
        PositionMasks();
        ResetHitsLeft();

        onMaskSwapped?.Invoke(currentMask);
    }

    MaskType GetHighestHealthAliveMask()
    {
        MaskType best = currentMask;
        int bestHealth = -1;

        Check(MaskType.Happy, happyHealth, happyDead);
        Check(MaskType.Sad, sadHealth, sadDead);
        Check(MaskType.Angry, angryHealth, angryDead);

        return best;

        void Check(MaskType type, int health, bool dead)
        {
            if (dead) return;

            if (health > bestHealth)
            {
                bestHealth = health;
                best = type;
            }
        }
    }

    void PositionMasks()
    {
        GetMaskObject(currentMask).transform.position = centerSlot.position;

        MaskType[] inactive = GetInactiveMasks();

        GetMaskObject(inactive[0]).transform.position = inactiveSlotA.position;
        GetMaskObject(inactive[1]).transform.position = inactiveSlotB.position;
    }

    MaskType[] GetInactiveMasks()
    {
        MaskType[] result = new MaskType[2];
        int index = 0;

        foreach (MaskType type in System.Enum.GetValues(typeof(MaskType)))
        {
            if (type == currentMask) continue;
            result[index++] = type;
        }

        return result;
    }

    GameObject GetMaskObject(MaskType type)
    {
        return type switch
        {
            MaskType.Happy => happyMask,
            MaskType.Sad => sadMask,
            MaskType.Angry => angryMask,
            _ => null
        };
    }

    public void DecrementHits(int amount = 1)
    {
        maskHitsLeft -= amount;
        maskHitsLeft = Mathf.Clamp(maskHitsLeft, 0, maskHitsTotal);

        UpdateSlider();
    }

    public void ResetHitsLeft()
    {
        maskHitsLeft = maskHitsTotal;
        UpdateSlider();
        ringFill.enabled = false;
    }

    void UpdateSlider()
    {
        ringFill.fillAmount = (float)maskHitsLeft / maskHitsTotal;
    }
}
