using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    public string WeaponName;
    public Sprite Icon;
    public int MaxAmmo;
    public float FireRate;
    public float ProjectileSpeed;
    public Projectile ProjectilePrefab;
    public string SFXName;

    public abstract void Shoot(Transform shootOrigin, Team team);

    public abstract bool CanShoot();
}

