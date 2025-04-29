using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform _renderer; 
    [SerializeField] private WeaponData _currentWeapon;

    public void TriggerAttack()
    {
        if (_currentWeapon != null && _renderer != null && _currentWeapon.CanShoot())
        {
            _currentWeapon.Shoot(_renderer, Team.Player);
        }
    }

    public void SetWeapon(WeaponData newWeapon)
    {
        _currentWeapon = newWeapon;
    }

}