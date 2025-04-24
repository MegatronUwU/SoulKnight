using UnityEngine;

[CreateAssetMenu(fileName = "PistolWeaponData", menuName = "Scriptable Objects/PistolWeaponData")]
public class PistolWeaponData : WeaponData
{
    public override void Shoot(Transform directionReference)
    {
        if (ProjectilePrefab == null) return;

        Vector3 spawnPos = directionReference.position + directionReference.forward;
        Projectile projectile = Instantiate(ProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.Initialize(directionReference.forward);
    }
}