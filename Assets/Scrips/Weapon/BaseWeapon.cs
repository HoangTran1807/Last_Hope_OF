using UnityEngine;

public abstract class BaseWeapon
{
    protected float cooldown;
    protected float timer;

    public BaseWeapon(float cooldown)
    {
        this.cooldown = cooldown;
    }

    public virtual void UpdateWeapon(Vector3 playerPos)
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            Fire(playerPos);
            timer = 0;
        }
    }

    protected abstract void Fire(Vector3 playerPos);
}
