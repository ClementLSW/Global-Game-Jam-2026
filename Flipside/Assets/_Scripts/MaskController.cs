using UnityEngine;
using UnityEngine.Events;

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
        //UpdateActiveMask();
    }

    public void TakeDamage(int damageAmount, bool isCritBumper = false, bool isSwapBumper = false)
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

        //if (isSwapBumper)
        //{
        //    SwapMasks();
        //}
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
        //UpdateActiveMask();
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

    void UpdateActiveMask()
    {
        Collider2D happyCol = happyMask.GetComponent<Collider2D>();
        Collider2D sadCol = sadMask.GetComponent<Collider2D>();
        Collider2D angryCol = angryMask.GetComponent<Collider2D>();
        SetCollider(happyMask, currentMask == MaskType.Happy);
        SetCollider(sadMask, currentMask == MaskType.Sad);
        SetCollider(angryMask, currentMask == MaskType.Angry);

        //happyMask.SetActive(currentMask == MaskType.Happy);
        //sadMask.SetActive(currentMask == MaskType.Sad);
        //angryMask.SetActive(currentMask == MaskType.Angry);
    }

    void SetCollider(GameObject obj, bool enabled)
    {
        Collider col = obj.GetComponent<Collider>();
        if (col != null)
            col.enabled = enabled;
    }
}
