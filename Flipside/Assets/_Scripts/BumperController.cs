using UnityEngine;
using System.Collections.Generic;

public class BumperController : MonoBehaviour
{
    public static BumperController Instance;

    [Header("Settings")]
    public int hitsForCrit = 8;

    public static int hitCounter = 0;

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

        AssignNewSwapBumper();
    }

    public static void RegisterHit()
    {
        hitCounter++;

        if (hitCounter >= Instance.hitsForCrit)
        {
            Instance.AssignNewCritBumper();
            hitCounter = 0;
        }
    }

    public void AssignNewCritBumper()
    {
        CollisionScript bumper = GetRandomAvailableBumper();

        if (bumper == null) return;

        bumper.SetAsCritBumper();
    }

    public void AssignNewSwapBumper()
    {
        CollisionScript bumper = GetRandomAvailableBumper();

        if (bumper == null) return;

        bumper.SetAsSwapBumper();
    }

    CollisionScript GetRandomAvailableBumper()
    {
        List<CollisionScript> available = new List<CollisionScript>();

        foreach (CollisionScript bumper in bumpers)
        {
            if (!bumper.isCritBumper && !bumper.isSwapBumper)
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
