using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform _renderer;
    [SerializeField] private WeaponData _currentWeapon;
    [SerializeField] private WeaponUI _weaponUI;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _bulletSpawnPoint;

    private int _currentAmmo;



    private void Start()
	{
		if(_currentWeapon != null)
			_currentAmmo = _currentWeapon.MaxAmmo;

		if (_weaponUI != null)
			_weaponUI.UpdateUI(_currentWeapon.WeaponName, _currentAmmo, _currentWeapon.MaxAmmo);

    }

    public void TriggerAttack()
    {
        //if (_currentWeapon != null && _renderer != null && _currentWeapon.CanShoot())
        //{
        //    _currentWeapon.Shoot(_renderer, Team.Player);
        //}
        _animator.SetTrigger("Shoot");

        ShootAuto();
    }


    public void SetWeapon(WeaponData newWeapon)
    {
        _currentWeapon = newWeapon;

        if (_currentWeapon != null)
            _currentAmmo = _currentWeapon.MaxAmmo;

        if(_weaponUI != null)
            _weaponUI.UpdateUI(_currentWeapon.WeaponName, _currentAmmo, _currentWeapon.MaxAmmo); 
    }

    public void ShootAuto()
    {
        if (_currentWeapon == null || !_currentWeapon.CanShoot() || _currentAmmo <= 0)
            return;

        if (TryFindClosestEnemy(out Transform target))
        {
            Vector3 direction = (target.position - _bulletSpawnPoint.position).normalized;
            _bulletSpawnPoint.forward = direction;
        }

        _currentWeapon.Shoot(_bulletSpawnPoint, Team.Player);
        _currentAmmo--;

		if (_weaponUI != null)
			_weaponUI.UpdateUI(_currentWeapon.WeaponName, _currentAmmo, _currentWeapon.MaxAmmo);
	}

    private bool TryFindClosestEnemy(out Transform closestEnemy)
    {
        //TODO: Replace with enemy list in room
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        if (closestEnemy != null)
            return true;

        return false;
    }

    public void AddAmmo(int amount)
    {
        if (_currentWeapon == null) return;

        _currentAmmo += amount;
        _currentAmmo = Mathf.Min(_currentAmmo, _currentWeapon.MaxAmmo);

        _weaponUI?.UpdateUI(_currentWeapon.WeaponName, _currentAmmo, _currentWeapon.MaxAmmo);

        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();

        if (_animator != null)
            _animator.SetTrigger("Reload");
    }

    public int GetCurrentAmmo() => _currentAmmo;

    public int GetMaxAmmo() => _currentWeapon != null ? _currentWeapon.MaxAmmo : 0;

}
