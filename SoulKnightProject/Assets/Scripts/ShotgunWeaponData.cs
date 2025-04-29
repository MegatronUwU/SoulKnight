using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunWeaponData", menuName = "Scriptable Objects/ShotgunWeaponData")]
public class ShotgunWeaponData : WeaponData
{
    public int PelletCount = 5;
    public float SpreadAngle = 15f;

    public override void Shoot(Transform directionReference, Team team)
    {
        if (ProjectilePrefab == null) return;

        for (int i = 0; i < PelletCount; i++)
        {
            // On calcule une rotation random pour le spread
            float angleOffset = Random.Range(-SpreadAngle / 2f, SpreadAngle / 2f);
            Quaternion spreadRotation = Quaternion.Euler(0f, angleOffset, 0f);

            // On l'applique au renderer
            Vector3 direction = spreadRotation * directionReference.forward;
            Vector3 spawnPosition = directionReference.position + direction * 1f;

            Projectile projectile = Instantiate(ProjectilePrefab, spawnPosition, Quaternion.identity);
            projectile.Initialize(direction, team);
        }
    }

	public override bool CanShoot()
	{
		return true;
	}
}
