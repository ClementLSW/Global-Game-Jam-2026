using System.Collections;

using UnityEngine;

public class HitStop : MonoBehaviour
{
    public float speed;
    private bool restoreTime;

    private void Start()
    {
        restoreTime = false;
    }

    private void Update()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }

            else
            {
                Time.timeScale = 1f;
                restoreTime = false;
            }
        }
    }

    public void StopTime(float changeTime, int restoreSpeed, float delay)
    {
        if (restoreTime)
        {
            return;
        }

        speed = restoreSpeed;

        if (delay > 0)
        {
            StopCoroutine(StartTime(delay));
            StartCoroutine(StartTime(delay));
        }

        else
        {
            restoreTime = true;
        }

        Time.timeScale = changeTime;
    }

    public IEnumerator StartTime(float amt)
    {
        restoreTime = true;
        yield return new WaitForSecondsRealtime(amt);
    }
}