using UnityEngine;

[CreateAssetMenu(fileName = "FirearmWeaponData", menuName = "Scriptable Objects/FirearmWeaponData")]
public class FirearmWeaponData : WeaponData
{
	public override void Shoot(Transform directionReference)
    {
        // On reprends le même code que le pistol en modifiant les valeurs
        if (ProjectilePrefab == null) return;

        Vector3 spawnPos = directionReference.position + directionReference.forward;
        Projectile projectile = Instantiate(ProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.Initialize(directionReference.forward);
    }

	public override bool CanShoot()
	{
        return true;
	}
}
