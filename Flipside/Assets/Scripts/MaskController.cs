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

    public MaskType currentMask;

    void Start()
    {
        happyHealth = sadHealth = angryHealth = startingHealth;

        happyDead = sadDead = angryDead = false;

        currentMask = (MaskType)Random.Range(0, 3);

        PositionMasks();
        UpdateActiveMask();
    }

    public void TakeDamage(int damageAmount, bool isCritBumper, bool isSwapBumper)
    {
        if (isCritBumper)
        {
            damageAmount = Mathf.RoundToInt(damageAmount * critMult);
        }

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

        if (isSwapBumper)
        {
            SwapMasks();
        }
    }

    void ApplyDamage(ref int health, ref bool dead, int damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            dead = true;
            SwapMasks();
        }
    }

    public void SwapMasks()
    {
        if (happyDead && sadDead && angryDead)
        {
            WinGame();
            return;
        }

        MaskType nextMask = GetHighestHealthAliveMask();

        if (nextMask == currentMask)
            return;

        currentMask = nextMask;
        PositionMasks();
        UpdateActiveMask();
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

    public void WinGame()
    {
        // all masks ded
    }
}
