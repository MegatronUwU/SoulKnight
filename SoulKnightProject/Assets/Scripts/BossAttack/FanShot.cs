using UnityEngine;

[System.Serializable]
public class FanShot : IBossAttack
{
    private WeaponData _weapon;
    private int _bulletCount;
    private float _spreadAngle;

    public FanShot(WeaponData weapon, int bulletCount = 5, float spreadAngle = 30f)
    {
        _weapon = weapon;
        _bulletCount = bulletCount;
        _spreadAngle = spreadAngle;
    }

    public void Execute(Transform origin)
    {
        float startAngle = -_spreadAngle / 2f;
        for (int i = 0; i < _bulletCount; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, startAngle + i * (_spreadAngle / (_bulletCount - 1)), 0);
            Transform tempOrigin = new GameObject("TempOrigin").transform;
            tempOrigin.position = origin.position;
            tempOrigin.rotation = origin.rotation * rotation;
            _weapon.Shoot(tempOrigin, Team.Enemy);
            GameObject.Destroy(tempOrigin.gameObject);
        }
    }
}
