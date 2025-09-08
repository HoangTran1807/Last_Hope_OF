using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [Header("Base Weapon Info")]
    [SerializeField] protected string weaponID;
    [SerializeField] protected int currentLevel = 1;
    [SerializeField] protected int maxLevel = 5;
    [SerializeField] protected float cooldown = 1f;

    protected float timer;

    public string WeaponID => weaponID;
    public bool IsMaxLevel => currentLevel >= maxLevel;

    // -------------- UPDATE LOOP --------------
    public virtual void UpdateWeapon(Vector3 playerPos)
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            Fire(playerPos);
            timer = 0;
        }
    }
    // -------------- OBJECT POOLING (dùng chung) --------------
    protected GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"{WeaponID} tried to spawn null prefab");
            return null;
        }

        GameObject obj = BulletPoolManager.Instance.GetObject(prefab);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }


    // Mỗi vũ khí tự define Fire()
    protected abstract void Fire(Vector3 playerPos);

    // -------------- UPGRADE --------------
    public virtual void ApplyUpgrade(UpgradeData upgrade)
    {
        if (IsMaxLevel) return;
        currentLevel++;
        Debug.Log($"{WeaponID} upgraded to level {currentLevel}");
    }
}
