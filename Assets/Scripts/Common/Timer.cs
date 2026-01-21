using System;
using System.Collections;
using UnityEngine;

public static class Timer
{
    public static Coroutine After(float seconds, Action callback)
    {
        return CoroutineRunner.Run(WaitCoroutine(seconds, callback));
    }

    private static IEnumerator WaitCoroutine(float seconds, Action callback)
    {
        if (seconds > 0f)
        {
            yield return new WaitForSeconds(seconds);
        }
        callback?.Invoke();
    }
}
