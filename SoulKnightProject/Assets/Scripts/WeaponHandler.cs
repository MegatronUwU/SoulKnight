using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
	[SerializeField] private Transform _renderer;
	[SerializeField] private WeaponData _currentWeapon;
	[SerializeField] private WeaponUI _weaponUI;
	[SerializeField] private Animator _animator;
	[SerializeField] private Transform _bulletSpawnPoint;

	private int _currentAmmo;

	[SerializeField] private float _maxTargetingDistance = 15f;

	private readonly System.Collections.Generic.Dictionary<WeaponData, int> _ammoByWeapon = new();

	private bool _isShootingHeld = false;
	private float _shootTimer = 0f;


	private void Start()
	{
		if (_currentWeapon != null)
			_currentAmmo = _currentWeapon.MaxAmmo;

		if (_weaponUI != null)
			_weaponUI.UpdateUI(_currentWeapon.WeaponName, _currentAmmo, _currentWeapon.MaxAmmo);

	}

	private void Update()
	{
		if (_isShootingHeld && _currentWeapon != null && _currentWeapon.CanShoot())
		{
			_shootTimer -= Time.deltaTime;
			if (_shootTimer <= 0f)
			{
				ShootAuto();
				_shootTimer = _currentWeapon.FireRate;
			}
		}
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
		if (_currentWeapon != null)
		{
			_ammoByWeapon[_currentWeapon] = _currentAmmo;
		}

		_currentWeapon = newWeapon;

		if (_currentWeapon != null)
		{
			if (_ammoByWeapon.TryGetValue(_currentWeapon, out int savedAmmo))
				_currentAmmo = savedAmmo;
			else
				_currentAmmo = _currentWeapon.MaxAmmo;

			_shootTimer = 0f;
		}

		if (_weaponUI != null)
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

		SoundManager.Instance.Play(_currentWeapon.SFXName);
	}

	private bool TryFindClosestEnemy(out Transform closestEnemy)
	{
		//TODO: Replace with enemy list in room
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		closestEnemy = null;
		float shortestDistance = Mathf.Infinity;

		foreach (GameObject enemy in enemies)
		{
			if (!enemy.activeInHierarchy) continue;

			Health health = enemy.GetComponent<Health>();
			if (health == null || health.IsDead) continue;

			float distance = Vector3.Distance(transform.position, enemy.transform.position);
			if (distance < shortestDistance && distance <= _maxTargetingDistance)
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

	public void StartShooting()
	{
		_isShootingHeld = true;
	}

	public void StopShooting()
	{
		_isShootingHeld = false;
	}

}
