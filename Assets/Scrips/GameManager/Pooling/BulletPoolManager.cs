using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance;

    // Dictionary: mỗi prefab -> queue các object
    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Tạo pool cho 1 prefab
    public void CreatePool(GameObject prefab, int initialSize)
    {
        if (!pools.ContainsKey(prefab))
        {
            Queue<GameObject> newPool = new Queue<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab, transform); // 👈 làm con PoolManager
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
        {
            CreatePool(prefab, 10);
        }

        if (pools[prefab].Count > 0)
        {
            GameObject obj = pools[prefab].Dequeue();
            obj.SetActive(true);
            obj.transform.SetParent(transform); // 👈 gắn lại parent PoolManager
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab, transform); // 👈 spawn con của PoolManager
            return obj;
        }
    }

    // Trả object về pool
    public void ReturnObject(GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform); // 👈 gom lại vào PoolManager
        pools[prefab].Enqueue(obj);
    }
}
