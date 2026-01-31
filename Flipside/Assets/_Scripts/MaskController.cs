using UnityEngine;

public enum MaskType
{
    Happy,
    Sad,
    Angry
}

public class MaskController : MonoBehaviour
{
    [Header("Mask Objects")]
    public GameObject happyMask;
    public GameObject sadMask;
    public GameObject angryMask;

    [Header("Cached Positions")]
    public Vector2 happyVector;
    public Vector2 sadVector;
    public Vector2 angryVector;

    [Header("Stats")]
    public int startingHealth = 10000;
    public float critMult = 5f;

    private int happyHealth;
    private int sadHealth;
    private int angryHealth;

    private bool happyDead;
    private bool sadDead;
    private bool angryDead;

    public MaskType currentMask;


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

    public void TakeDamage(int damageAmount, bool isCrit)
    {
        if (isCrit)
            damageAmount = Mathf.RoundToInt(damageAmount * critMult);

        switch (currentMask)
        {
            case MaskType.Happy:
                ApplyDamage(ref happyHealth, ref happyDead, damageAmount);
                break;

            case MaskType.Sad:
                ApplyDamage(ref sadHealth, ref sadDead, damageAmount);
                break;

            case MaskType.Angry:
                ApplyDamage(ref angryHealth, ref angryDead, damageAmount);
                break;
        }
    }

    void ApplyDamage(ref int health, ref bool dead, int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0 && !dead)
        {
            dead = true;
            SwapMasks();
        }
    }

    void SwapMasks()
    {
        if (happyDead && sadDead && angryDead)
        {
            WinGame();
            return;
        }

        MaskType previousMask = currentMask;
        MaskType nextMask = GetHighestHealthAliveMask();

        if (previousMask == nextMask)
            return;

        SwapMaskPositions(previousMask, nextMask);

        currentMask = nextMask;
        UpdateActiveMask();
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
        Collider happyCol = happyMask.GetComponent<Collider>();
        Collider sadCol = sadMask.GetComponent<Collider>();
        Collider angryCol = angryMask.GetComponent<Collider>();

        if (happyCol != null) happyCol.enabled = (currentMask == MaskType.Happy);
        if (sadCol != null) sadCol.enabled = (currentMask == MaskType.Sad);
        if (angryCol != null) angryCol.enabled = (currentMask == MaskType.Angry);
    }

    public void WinGame()
    {
        // all masks ded
    }
}
