using UnityEngine;
using System.Collections.Generic;

public class BumperController : MonoBehaviour
{
    public static BumperController Instance;

    [Header("Settings")]
    public int hitsForCrit = 8;
    public int hitsForSwap = 12;

    public static int critHitCounter = 0;
    public static int swapHitCounter = 0;
    public bool swapBumperAssigned = false;

    public CollisionScript[] bumpers;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        GameObject[] bumperObjects = GameObject.FindGameObjectsWithTag("Bumper");

        bumpers = new CollisionScript[bumperObjects.Length];
        for (int i = 0; i < bumperObjects.Length; i++)
        {
            bumpers[i] = bumperObjects[i].GetComponent<CollisionScript>();
        }
    }

    public static void RegisterHit()
    {
        critHitCounter++;

        if (critHitCounter >= Instance.hitsForCrit)
        {
            Instance.AssignNewCritBumper();
            critHitCounter = 0;
        }
    }

    public void AssignNewCritBumper()
    {
        CollisionScript bumper = GetRandomAvailableBumper();

        if (bumper == null) return;

        bumper.SetAsCritBumper();
    }

    CollisionScript GetRandomAvailableBumper()
    {
        List<CollisionScript> available = new List<CollisionScript>();

        foreach (CollisionScript bumper in bumpers)
        {
            if (!bumper.isCritBumper)
            {
                available.Add(bumper);
            }
        }

        if (available.Count == 0)
        {
            Debug.LogWarning("No available bumpers to assign!");
            return null;
        }

        return available[Random.Range(0, available.Count)];
    }
}
