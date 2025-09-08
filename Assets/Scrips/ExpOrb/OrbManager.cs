using UnityEngine;
using System.Collections.Generic;

public class OrbManager : MonoBehaviour
{
    public static OrbManager Instance { get; private set; }
    public static bool HasInstance => Instance != null;

    private List<EXPOrb> orbs = new List<EXPOrb>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterOrb(EXPOrb orb)
    {
        if (!orbs.Contains(orb)) orbs.Add(orb);
    }

    public void UnregisterOrb(EXPOrb orb)
    {
        orbs.Remove(orb);
    }

    void Update()
    {
        for (int i = 0; i < orbs.Count; i++)
        {
            EXPOrb orb = orbs[i];
            if (orb != null && orb.isMoving && orb.targetPlayer != null)
            {
                orb.transform.position = Vector2.MoveTowards(
                    orb.transform.position,
                    orb.targetPlayer.position,
                    orb.moveSpeed * Time.deltaTime
                );
            }
        }
        CheckOrbMerge();
    }

    public void CheckOrbMerge()
    {
        for (int i = 0; i < orbs.Count; i++)
        {
            for (int j = i + 1; j < orbs.Count; j++)
            {
                if (orbs[i] == null || orbs[j] == null) continue;

                float dist = Vector3.Distance(orbs[i].transform.position, orbs[j].transform.position);

                // bán kính merge dựa vào orb lớn hơn
                float mergeRadius = Mathf.Max(orbs[i].GetMergeRadius(), orbs[j].GetMergeRadius());

                if (dist < mergeRadius)
                {
                    // cho orb lớn hơn nuốt orb nhỏ hơn
                    if (orbs[i].expAmount >= orbs[j].expAmount)
                        orbs[i].MergeWith(orbs[j]);
                    else
                        orbs[j].MergeWith(orbs[i]);
                }
            }
        }
    }


}
