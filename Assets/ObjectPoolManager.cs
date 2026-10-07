
using System.Collections.Generic;
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

    public Transform PoolParent;

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
                GameObject perfab = Instantiate(pool.perfab, PoolParent);
                perfab.SetActive(false);
                objects.Enqueue(perfab);
            }

            poolDictionary.Add(pool.perfab, objects);
        }
        Debug.Log("[OPM] Object Pool Data Updated");
        Debug.Log($"[OPM] poolDictionary Count: {poolDictionary.Count}");
        Debug.Log($"poolDictionary.Values.Count: {poolDictionary.Values.Count}");
    }

    public GameObject GetPool(GameObject poolPrefab, Vector3 pos, Quaternion quaternion)
    {
        GameObject perfab;
        if (poolDictionary.TryGetValue(poolPrefab, out Queue<GameObject> queue))
        {
            perfab = queue.Dequeue();
            perfab.SetActive(true);
            perfab.transform.position = pos;
            perfab.transform.rotation = quaternion;

            // ★ 꺼낸 총알에게 자신의 원본 프리팹이 무엇인지 알려줍니다.
            if (perfab.TryGetComponent(out Bullet bulletScript))
            {
                bulletScript.SetOriginPrefab(poolPrefab);
            }
        }
        else
        {
            Debug.LogWarning($"[PoolDictionary] null Object in Dictionary");
            return null;
        }
        return perfab;
    }

    // 매개변수를 (어떤 프리팹의 큐인지, 반환할 인스턴스) 형태로 받도록 수정
    public void ReturnPool(GameObject poolPrefab, GameObject instance)
    {
        if (poolDictionary.TryGetValue(poolPrefab, out Queue<GameObject> queue))
        {
            instance.SetActive(false);
            queue.Enqueue(instance);
        }
        else
        {
            Debug.LogWarning($"[PoolDictionary] null Object in Dictionary");
            return;
        }
        Debug.Log($"[OPM] Successed ReturnPool / Queue Count: {queue.Count}");
    }
}
