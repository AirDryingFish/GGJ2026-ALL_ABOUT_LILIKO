using System.Collections.Generic;
using UnityEngine;

public class SimplePool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int prewarm = 8;
    [SerializeField] private Transform container;

    private readonly Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake()
    {
        if (container == null)
        {
            container = transform;
        }
        if (prefab == null || prewarm <= 0)
        {
            return;
        }
        for (int i = 0; i < prewarm; i++)
        {
            var obj = CreateInstance();
            Release(obj);
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        var obj = _pool.Count > 0 ? _pool.Dequeue() : CreateInstance();
        obj.transform.SetParent(null, false);
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        obj.SetActive(false);
        obj.transform.SetParent(container, false);
        _pool.Enqueue(obj);
    }

    private GameObject CreateInstance()
    {
        return Instantiate(prefab, container);
    }
}
