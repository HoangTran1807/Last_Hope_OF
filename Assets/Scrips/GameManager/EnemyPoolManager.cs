using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;

    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Tạo pool cho prefab
    public void CreatePool(GameObject prefab, int initialSize)
    {
        if (!pools.ContainsKey(prefab))
        {
            Queue<GameObject> newPool = new Queue<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                newPool.Enqueue(obj);
            }

            pools[prefab] = newPool;
        }
    }

    // Lấy object từ pool
    public GameObject GetObject(GameObject prefab)
    {
        if (!pools.ContainsKey(prefab))
            CreatePool(prefab, 10);

        if (pools[prefab].Count > 0)
        {
            GameObject obj = pools[prefab].Dequeue();
            obj.SetActive(true);
            obj.transform.SetParent(transform); // gom hết vào PoolManager
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab, transform);
            return obj;
        }
    }

    // Trả object về pool
    public void ReturnObject(GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        pools[prefab].Enqueue(obj);
    }
}
