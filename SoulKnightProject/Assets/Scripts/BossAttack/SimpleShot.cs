using UnityEngine;

[System.Serializable]
public class SimpleShot : IBossAttack
{
    private WeaponData _weapon;

    public SimpleShot(WeaponData weapon)
    {
        _weapon = weapon;
    }

    public void Execute(Transform origin)
    {
        _weapon.Shoot(origin, Team.Enemy);
    }
}
