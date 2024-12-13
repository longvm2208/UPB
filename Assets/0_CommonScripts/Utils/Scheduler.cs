using System;
using System.Collections;
using UnityEngine;

public class Scheduler
{
    private class MB : MonoBehaviour { }

    private static MB mb;

    public static Coroutine DoTask(float delay, Action task)
    {
        if (task == null) return null;

        if (mb == null)
        {
            GameObject obj = new GameObject("Scheduler");
            mb = obj.AddComponent<MB>();
            UnityEngine.Object.DontDestroyOnLoad(obj);
        }

        return mb.StartCoroutine(DoTaskCoroutine(delay, task));
    }

    private static IEnumerator DoTaskCoroutine(float delay, Action task)
    {
        if (delay < Time.deltaTime)
        {
            yield return null;
        }
        else
        {
            yield return WaitFor.Seconds(delay);
        }

        try
        {
            task?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static void CancelTask(Coroutine coroutine)
    {
        if (mb != null)
        {
            mb.StopCoroutine(coroutine);
        }
    }

    public static void CancelAllTasks()
    {
        if (mb != null)
        {
            mb.StopAllCoroutines();
        }
    }
}
