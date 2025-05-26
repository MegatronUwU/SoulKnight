using UnityEngine;

[System.Serializable]
public class SpiralShot : IBossAttack
{
    private WeaponData _weapon;
    private int _bulletCount;
    private float _angleStep;
    private float _currentAngle;

    public SpiralShot(WeaponData weapon, int bulletCount = 12, float angleStep = 30f)
    {
        _weapon = weapon;
        _bulletCount = bulletCount;
        _angleStep = angleStep;
        _currentAngle = 0f;
    }

    public void Execute(Transform origin)
    {
        for (int i = 0; i < _bulletCount; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, _currentAngle + i * _angleStep, 0);
            Transform tempOrigin = new GameObject("TempSpiral").transform;
            tempOrigin.position = origin.position;
            tempOrigin.rotation = origin.rotation * rotation;
            _weapon.Shoot(tempOrigin, Team.Enemy);
            GameObject.Destroy(tempOrigin.gameObject);
        }
        _currentAngle += _angleStep / 2f;
    }
}
