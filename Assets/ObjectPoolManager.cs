using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct PoolBox
{
    public GameObject perfab;
    public int poolSize;
}
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [Header("PoolDataList")]
    public List<PoolBox> Pools;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        } 

        InitializeData();
    }

    private void InitializeData()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
        
        foreach (var pool in Pools) //=> List-> 1 struct Data
        {
            Queue<GameObject> objects = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++) //=> initialize Queue
            {
                GameObject perfab = Instantiate(pool.perfab);
                perfab.SetActive(false);
                objects.Enqueue(perfab);
            }

            poolDictionary.Add(pool.perfab, objects);
        }
         
    }

    public GameObject GetPool(GameObject pool, Vector3 pos, Quaternion quaternion)
    {
        GameObject perfab;
        if (poolDictionary.TryGetValue(pool, out Queue<GameObject> queue))
        {
            //pool instance
            perfab = queue.Dequeue();
            perfab.SetActive(true);
            perfab.transform.position = pos;
            perfab.transform.rotation = quaternion;
        }
        else
        {
            Debug.LogWarning($"[PoolDictionary] null Object in Dictionary");
            return null;
        }
        return perfab;
    }

    public void ReturnPool(GameObject pool)
    {
        if (poolDictionary.TryGetValue(pool, out Queue<GameObject> queue))
        {
            pool.SetActive(false);
            queue.Enqueue(pool);
        }
        else
        {
            Debug.LogWarning($"[PoolDictionary] null Object in Dictionary");
            return;
        }
    }
}
