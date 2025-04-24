using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]

public abstract class WeaponData : ScriptableObject
{
    public string WeaponName;
    public Sprite Icon;
    public int MaxAmmo;
    public float FireRate;
    public float ProjectileSpeed;
    public Projectile ProjectilePrefab;

    public abstract void Shoot(Transform shootOrigin);
}

