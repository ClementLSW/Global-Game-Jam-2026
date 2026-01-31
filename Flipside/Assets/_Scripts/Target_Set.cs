using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Target_Set : MonoBehaviour
{
    public List<Target> Targets = new List<Target>();

    private void Awake()
    {
        Targets.AddRange(GetComponentsInChildren<Target>());
    }

    public void ActivateAllTargets()
    {
        foreach (Target target in Targets)
        {
            target.Activate();
        }
    }

    public void OnTargetHit()
    {
        if (Targets.TrueForAll(t => t.State == Target.TargetState.Hit))
        {
            AllTargetsHit();
        }
    }

    private void AllTargetsHit()
    {
        Debug.Log("All targets have been hit!");
        // Implement additional logic for when all targets are hit
        MaskController.Instance.TakeDamage(25);

        ActivateAllTargets();
    }
}
