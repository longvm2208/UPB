using System;
using System.Collections;
using UnityEngine;

public class ScheduleUtils
{
    private class MonoBehaviourHook : MonoBehaviour { }

    private static MonoBehaviourHook scheduler;

    private static void InitSchedulerIfNeeded()
    {
        if (scheduler == null)
        {
            GameObject obj = new GameObject("Scheduler");
            scheduler = obj.AddComponent<MonoBehaviourHook>();
            UnityEngine.Object.DontDestroyOnLoad(obj);
        }
    }

    public static void StopAllCoroutines()
    {
        if (scheduler != null)
        {
            scheduler.StopAllCoroutines();
        }
    }

    public static Coroutine DelayTask(float delay, Action task)
    {
        if (task == null) return null;

        InitSchedulerIfNeeded();

        return scheduler.StartCoroutine(DelayRoutine(delay, task));
    }

    private static IEnumerator DelayRoutine(float delay, Action task)
    {
        yield return new WaitForSeconds(delay);

        task?.Invoke();
    }
}
