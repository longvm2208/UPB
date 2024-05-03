using System.Collections;
using UnityEngine;

public static class CoroutineExtensions
{
    public static void Start(this IEnumerator coroutine, MonoBehaviour behaviour)
    {
        if (behaviour == null) return;

        coroutine.Stop(behaviour);
        behaviour.StartCoroutine(coroutine);
    }

    public static void Stop(this IEnumerator coroutine, MonoBehaviour behaviour)
    {
        if (coroutine == null || behaviour == null) return;

        behaviour.StopCoroutine(coroutine);
    }

    public static void Stop(this Coroutine coroutine, MonoBehaviour behaviour)
    {
        if (coroutine == null || behaviour == null) return;

        behaviour.StopCoroutine(coroutine);
    }
}
