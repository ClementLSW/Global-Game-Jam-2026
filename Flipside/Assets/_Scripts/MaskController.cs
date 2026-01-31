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
        happyHealth = startingHealth;
        sadHealth = startingHealth;
        angryHealth = startingHealth;

        happyDead = false;
        sadDead = false;
        angryDead = false;

        happyVector = happyMask.transform.position;
        sadVector = sadMask.transform.position;
        angryVector = angryMask.transform.position;

        currentMask = (MaskType)Random.Range(0, 3);
        UpdateActiveMask();
    }

    public void TakeDamage(int damageAmount, bool isCrit = false)
    {
        if(AllMasksDead) return;

        if (isCrit)
            damageAmount = Mathf.RoundToInt(damageAmount * critMult);

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

    void ApplyDamage(ref int health, ref bool dead, int damageAmount, MaskType type)
    {
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

    void SwapMasks()
    {
        if (AllMasksDead)
        {
            //WinGame();
            onAllMasksDestroyed?.Invoke();
            return;
        }

        MaskType previousMask = currentMask;
        MaskType nextMask = GetHighestHealthAliveMask();

        if (previousMask == nextMask)
            return;

        SwapMaskPositions(previousMask, nextMask);

        currentMask = nextMask;
        UpdateActiveMask();
        onMaskSwapped?.Invoke(currentMask);
    }

    MaskType GetHighestHealthAliveMask()
    {
        MaskType bestMask = currentMask;
        int bestHealth = -1;

        Check(MaskType.Happy, happyHealth, happyDead);
        Check(MaskType.Sad, sadHealth, sadDead);
        Check(MaskType.Angry, angryHealth, angryDead);

        return bestMask;

        void Check(MaskType type, int health, bool dead)
        {
            if (dead) return;

            if (health > bestHealth)
            {
                bestHealth = health;
                bestMask = type;
            }
        }
    }

    void SwapMaskPositions(MaskType a, MaskType b)
    {
        GameObject maskA = GetMaskObject(a);
        GameObject maskB = GetMaskObject(b);

        ref Vector2 posA = ref GetMaskVector(a);
        ref Vector2 posB = ref GetMaskVector(b);

        // swap cached positions
        Vector2 temp = posA;
        posA = posB;
        posB = temp;

        // apply to transforms
        maskA.transform.position = posA;
        maskB.transform.position = posB;
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

    ref Vector2 GetMaskVector(MaskType type)
    {
        if (type == MaskType.Happy) return ref happyVector;
        if (type == MaskType.Sad) return ref sadVector;
        return ref angryVector;
    }

    void UpdateActiveMask()
    {
        Collider2D happyCol = happyMask.GetComponent<Collider2D>();
        Collider2D sadCol = sadMask.GetComponent<Collider2D>();
        Collider2D angryCol = angryMask.GetComponent<Collider2D>();

        if (happyCol != null) happyCol.enabled = (currentMask == MaskType.Happy);
        if (sadCol != null) sadCol.enabled = (currentMask == MaskType.Sad);
        if (angryCol != null) angryCol.enabled = (currentMask == MaskType.Angry);
    }
}
