using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    [SerializeField]
    GameObject PoolObject;
    private List<GameObject> Pool = new List<GameObject>();
    int PoolSize = 30;
    int IncreaseIncrement = 5;

    private void Start()
    {
        GameObject obj = null;
        for (int i = 0; i < PoolSize; i++) {

            obj = Instantiate(PoolObject);
            Pool.Add(obj);
            obj.SetActive(false);
        }
    }
    public void ReturnToPool(GameObject obj)
    {
        Pool.Add(obj);
        obj.SetActive(false);
    }
    public GameObject FromPool()
    {
        var obj = Pool[Pool.Count - 1];
        obj.SetActive(true);
        Pool.Remove(obj);
        if (Pool.Count == 0)
        {
            IncreasePoolSize();
        }
        return obj;
    }
    private void IncreasePoolSize()
    {
        GameObject obj = null;
        for (int i = 0; i < IncreaseIncrement; i++)
        {
            obj = Instantiate(PoolObject);
            Pool.Add(obj);
            obj.SetActive(false);
        }
    }
}
