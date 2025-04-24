using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private WeaponData[] _weapons;

    private int _currentIndex = 0;

    public void SwitchWeapon()
    {
        _currentIndex = (_currentIndex + 1) % _weapons.Length;
        _weaponHandler.SetWeapon(_weapons[_currentIndex]);
    }
}
