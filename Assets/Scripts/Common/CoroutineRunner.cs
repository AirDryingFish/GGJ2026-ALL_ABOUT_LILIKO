using System.Collections;
using UnityEngine;

public sealed class CoroutineRunner : PersistentSingleton<CoroutineRunner>
{
    public static Coroutine Run(IEnumerator routine)
    {
        if (Instance == null || routine == null)
        {
            return null;
        }
        return Instance.StartCoroutine(routine);
    }

    public static void Stop(Coroutine routine)
    {
        if (Instance == null || routine == null)
        {
            return;
        }
        Instance.StopCoroutine(routine);
    }
}
