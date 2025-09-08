using UnityEngine;
using System.Collections.Generic;

public class OrbPool : MonoBehaviour
{
    public static OrbPool Instance { get; private set; }

    [Header("Prefab & Settings")]
    public GameObject orbPrefab;
    public int initialSize = 50;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

       
    }

    private void Start()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject orb = CreateNewOrb();
            orb.SetActive(false);
            pool.Enqueue(orb);
        }
    }

    private GameObject CreateNewOrb()
    {
        GameObject orb = Instantiate(orbPrefab, transform);
        return orb;
    }

    public GameObject GetOrb(Vector3 position, int expAmount)
    {
        GameObject orb;
        if (pool.Count > 0)
            orb = pool.Dequeue();
        else
            orb = CreateNewOrb();

        orb.transform.position = position;
        orb.SetActive(true);

        EXPOrb expOrb = orb.GetComponent<EXPOrb>();
        expOrb.expAmount = expAmount;
        expOrb.isMoving = false;
        expOrb.targetPlayer = null;

        expOrb.UpdateVisual();

        return orb;
    }

    public void ReturnOrb(GameObject orb)
    {
        orb.SetActive(false);
        pool.Enqueue(orb);
    }
}
