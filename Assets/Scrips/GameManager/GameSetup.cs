using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public PlayerWeaponManager playerWeaponManager;
    public GameObject rifleBulletPrefab;
    public GameObject sniperBulletPrefab;

    void Start()
    {
        Rifle rifle = new Rifle(rifleBulletPrefab, 10f, 100, 15f, 0.04f, 0.5f);
        rifle.SetTargetingStrategy(new ClosestEnemyStrategy());

        Rifle gun2 = new Rifle(sniperBulletPrefab, 30f, 100, 20f, 2f, 1f);
        gun2.SetTargetingStrategy(new HighestHealthEnemyStrategy());


        playerWeaponManager.AddWeapon(gun2);
        //playerWeaponManager.AddWeapon(rifle);
    }
}
